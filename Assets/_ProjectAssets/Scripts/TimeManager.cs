using System;
using Cysharp.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject sonGameObject;
    public Color lightColor;
    public Color nightColor;
    private Light son;



    void Awake()
    {
        son = sonGameObject.GetComponent<Light>();
    }


    private void OnEnable()
    {
        EnemySpawner.onPauseStart += LowerSon;
        EnemySpawner.onAllEnemiesDead += RiseSon;
    }

    private void OnDisable()
    {
        EnemySpawner.onPauseStart -= LowerSon;
        EnemySpawner.onAllEnemiesDead -= RiseSon;
    }

    [ContextMenu("RiseSon")]
    public void RiseSon()
    {
        LeanTween.rotateX(sonGameObject, 45, 2f).setEase(LeanTweenType.easeOutBounce);
        LerpColors(son.color, lightColor, 2f);
    }

    [ContextMenu("LowerSon")]
    public void LowerSon(float pauseDuration)
    {
        UniTask.Void(async () =>
        {
            LerpColors(son.color, nightColor, pauseDuration);
            LeanTween.rotateX(sonGameObject, 0, pauseDuration).setEaseInQuad();
        });

    }

    private void LerpColors(Color currentCulor, Color targetColor, float duration)
    {
        try
        {
            LeanTween.value(0, 1, duration).setOnUpdate((float val) =>
        {
            son.color = Color.Lerp(currentCulor, targetColor, val);
        }).setEaseInQuad();

        }
        catch (Exception e)
        {
            LeanTween.cancel(sonGameObject);
        }

    }

}
