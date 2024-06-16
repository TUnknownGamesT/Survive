using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject son;

    private void OnEnable()
    {
        EnemySpawner.onPauseStart += StartCycle;
    }

    private void OnDisable()
    {
        EnemySpawner.onPauseStart -= StartCycle;
    }


    private void StartCycle(int pauseTime)
    {
        UniTask.Void(async () =>
        {
            RiseSon();
            await UniTask.Delay(TimeSpan.FromSeconds(pauseTime));
            LowerSon();
        });
    }

    [ContextMenu("RiseSon")]
    public void RiseSon()
    {
        LeanTween.rotateX(son,45,1f).setEase(LeanTweenType.easeOutBounce);
    }
    
    [ContextMenu("LowerSon")]
    public void LowerSon()
    {
        LeanTween.rotateX(son,-20,1f).setEase(LeanTweenType.easeOutBounce);
    }
}
