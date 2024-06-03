using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AKA47 : Gun
{

    public MeshRenderer[] meshRenderers;
    
    
    public override bool CanShoot() => !reloading && TimeSinceLastShot > 1f / (fireRate / 60f);
    
}
