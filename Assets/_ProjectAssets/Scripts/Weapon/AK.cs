using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AK : Firearm
{

    public MeshRenderer[] meshRenderers;
    
    
    public override bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f);
    
}
