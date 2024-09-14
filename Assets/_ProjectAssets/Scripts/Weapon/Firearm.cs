using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using Random = UnityEngine.Random;


[RequireComponent(typeof(SoundComponent))]
public abstract class Firearm : Weapon
{

    public Action onShoot;
    public Action<int, int> onFinishReload;

    [Range(0, 1f)]
    public float spread;
    [Header("Reloading")]
    public int magSize;
    public int rezervAmo;
    public float bulletSpeed;
    public bool reloading;
    [Header("References")]
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    public AudioClip shootSound;
    public AudioClip reloadSound;

    protected SoundComponent _soundComponent;
    public int currentAmunition;


    protected int bulletRezerSize;

    private void Awake()
    {
        currentAmunition = magSize;
        _soundComponent = GetComponent<SoundComponent>();
    }

    private void Start()
    {
        bulletRezerSize = rezervAmo;
    }

    protected void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public override void SetArmHandler(AnimationManager animationManager)
    {
        if (animationManager as PlayerAnimationsManager != null)
        {
            _animationManager = animationManager as PlayerAnimationsManager;
        }

        if (animationManager as ZombieAnimationManager != null)
        {
            _animationManager = animationManager as ZombieAnimationManager;
        }

    }

    public override bool CanShoot()
    {
        if (GameManager.instance._isPaused) return false;

        return !reloading && timeSinceLastShot > 1f / (fireRate / 60f);
    }


    public virtual void Shoot()
    {


        if (currentAmunition > 0 && CanShoot())
        {

            spreadAmount += 0.1f;
            CrossHairWrapper.instance.IncreaseScaleMultiplayer(0.3f);

            Vector3 spread = GetSpread(Mathf.Lerp(0, maxSpreadAmount, Mathf.Clamp01(spreadAmount / maxSpreadAmount)));

            Transform bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).transform;
            bullet.GetComponent<Rigidbody>().AddRelativeForce((Vector3.forward + new Vector3(spread.x, spread.y, 0)) * bulletSpeed, ForceMode.Impulse);
            bullet.GetComponent<BulletBehaviour>().damage = damage;
            vfx.Play();
            vfx.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            currentAmunition--;
            timeSinceLastShot = 0;
            _soundComponent.PlaySound(shootSound);
            _animationManager.Attack();
            onShoot?.Invoke();
            CameraController.ShakeCamera(0.2f, 2f);
        }
        else if (currentAmunition <= 0 && rezervAmo > 0)
        {
            if (!reloading)
            {
                Reload();
            }
        }
    }

    private Vector3 GetSpread(float shootTime = 0)
    {
        Vector3 spread = Vector3.Lerp(Vector3.zero,
        new Vector3(Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread),
        Random.Range(-this.spread, this.spread)),
        Mathf.Clamp01(shootTime));

        //Debug.Log($"{shootTime} / {maxSpreadTime} = {shootTime / maxSpreadTime}");

        return spread;
    }

    public override void Tick(bool wantsToAttack)
    {
        if (wantsToAttack)
        {
            Shoot();
        }
        else
        {
            spreadAmount = 0;
        }
    }



    public override void Reload()
    {
        reloading = true;
        _soundComponent.PlaySound(reloadSound);
        _animationManager.Reload();
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(reloadTime));
            reloading = false;

            int difference = magSize - currentAmunition;
            if (rezervAmo - difference >= 0)
            {
                currentAmunition += difference;
                rezervAmo -= difference;
            }
            else
            {
                currentAmunition = rezervAmo;
                rezervAmo = 0;
            }

            onFinishReload?.Invoke(currentAmunition, rezervAmo);
        });
    }


    public override void RefillAmmunition(int amount)
    {
        if (rezervAmo + amount <= bulletRezerSize)
        {
            rezervAmo += amount;
        }
        else
        {
            rezervAmo = bulletRezerSize;
        }
        onFinishReload?.Invoke(currentAmunition, rezervAmo);
    }
}
