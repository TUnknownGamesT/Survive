using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(SoundComponent))]
public abstract class Firearm : MonoBehaviour
{
    
    public static  Action<int,int> onPickUpNewWeapon;
    public  Action onShoot;
    public  Action<int,int> onFinishReload;

    public Sprite weaponIcon;
    public Constants.EnemyType enemyDrop;
    [Header("Shooting")]
    public float damage;
    public VisualEffect vfx;
    [Range(0,1f)]
    public float spread;
    [Header("Reloading")]
    public float fireRate;
    public float reloadTime;
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
    protected float timeSinceLastShot;
    public int currentAmunition;
    
    protected AnimationManager _animationManager;
    private int bulletRezerSize;
    
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

    public void SetArmHandler(AnimationManager arm)
    {
        if (arm as PlayerAnimationsManager != null)
        {
            _animationManager = arm as PlayerAnimationsManager;
        }
        
        if(arm as ZombieAnimationManager != null)
        {
            _animationManager = arm as ZombieAnimationManager;
        }
        
    } 
    
    public virtual bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f);
    
    public virtual void Shoot()
    {

        if(currentAmunition>0&& CanShoot())
        {
            
            float xSpread = UnityEngine.Random.Range(-spread, spread);
            float YSpread = UnityEngine.Random.Range(-spread, spread);
            
            Rigidbody rb =  Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Rigidbody>();
            rb.AddRelativeForce((Vector3.forward + new Vector3(xSpread,YSpread,0)) * bulletSpeed, ForceMode.Impulse);
            vfx.Play();
            currentAmunition--;
            timeSinceLastShot = 0;
            _soundComponent.PlaySound(shootSound);
            _animationManager.Attack();
            onShoot?.Invoke();
            CameraController.ShakeCamera(0.2f,2f);
        }
        else if(currentAmunition<=0 && rezervAmo>0)
        {
            if (!reloading)
            {
                Reload();
            }
        }
    }

    public void Reload()
    {
        reloading = true;
        _soundComponent.PlaySound(reloadSound);
        _animationManager.Reload();
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(reloadTime));
            reloading = false;

            int difference = magSize - currentAmunition;
            if ( rezervAmo - difference >= 0)
            {
                currentAmunition += difference;
                rezervAmo -= difference;
            }
            else
            {
                currentAmunition = rezervAmo;
                rezervAmo = 0;
            }
            
            onFinishReload?.Invoke(currentAmunition,rezervAmo);
        });
    }
    

    public void RefillAmmunition(int amount)
    {
        if(rezervAmo+amount<=bulletRezerSize)
        {
            rezervAmo += amount;
        }
        else
        {
            rezervAmo = bulletRezerSize;
        }
        onFinishReload?.Invoke(currentAmunition,rezervAmo);
    }
}
