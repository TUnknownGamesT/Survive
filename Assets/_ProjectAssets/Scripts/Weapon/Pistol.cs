using UnityEngine;

public class Pistol : Firearm
{
    public override void Shoot()
    {
        if (currentAmunition > 0 && CanShoot())
        {

            CrossHairWrapper.instance.IncreaseScaleMultiplayer(1f);

            Transform bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).transform;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);
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
}
