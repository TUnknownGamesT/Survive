using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Gun
{
    public override void Shoot()
    {
        if(CanShoot())
        {
            TimeSinceLastShot = 0;
            _soundComponent.PlaySound(shootSound);
            onShoot?.Invoke();
        }
    }
}
