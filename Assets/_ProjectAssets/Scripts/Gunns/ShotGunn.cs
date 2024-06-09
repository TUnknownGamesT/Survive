using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunn : Gun
{
    public float numberOfBulletsPerShoot;
    public MeshRenderer meshRenderer;

    public override void Shoot()
    {
        if (currentAmunition > 0 && CanShoot())
        {
            
            // _armHandler.animation.Shoot();
            for (int i = 0; i < numberOfBulletsPerShoot; i++)
            {
                float xSpread = UnityEngine.Random.Range(-spread, spread);
                float YSpread = UnityEngine.Random.Range(-spread, spread);
                
                Rigidbody rb = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation)
                    .GetComponent<Rigidbody>();
                rb.AddRelativeForce((Vector3.forward + new Vector3(xSpread, YSpread, 0)) * bulletSpeed,
                    ForceMode.Impulse);
            }

            vfx.Play();
            currentAmunition--;
            TimeSinceLastShot = 0;
            CameraController.ShakeCamera(0.2f,2);
            _soundComponent.PlaySound(shootSound);
            onShoot?.Invoke();
            //_armHandler.animation.StopShooting();
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