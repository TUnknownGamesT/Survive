
using System;
using UnityEngine;

public abstract class IAIBrain : MonoBehaviour
{

    public Action onLocalEnemyDeath;
    public bool _alive = true;

    public abstract void BaseInView(Transform basePoint);
    public abstract void PlayerInView();
    public abstract void PlayerOutOfView();

    public virtual void Death()
    {
        onLocalEnemyDeath?.Invoke();
    }
}
