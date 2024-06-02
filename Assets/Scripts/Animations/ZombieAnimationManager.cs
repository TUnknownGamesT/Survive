using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationManager : AnimationManager
{

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }
    
}
