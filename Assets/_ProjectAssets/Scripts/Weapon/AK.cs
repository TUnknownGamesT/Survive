using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AK : Firearm
{

    public MeshRenderer[] meshRenderers;


    public override bool CanShoot()
    {
        if (GameManager.instance._isPaused) return false;

        return !reloading && timeSinceLastShot > 1f / (fireRate / 60f);
    }

}
