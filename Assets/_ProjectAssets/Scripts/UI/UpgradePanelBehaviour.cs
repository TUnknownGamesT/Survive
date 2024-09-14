using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;
using ConstantsValues;


public class UpgradePanelBehaviour : MonoBehaviour
{
    public static Action<UpgradeType> onUpgradeCardInFront;

    public RectTransform playerCard;
    public RectTransform gunsCard;
    public RectTransform baseCard;
    public RectTransform enemyCard;
    public VisualEffect sparks;

    public static Action onSecondaryCardDisappear;
    public static Action onPanelDisappear;

    private UpgradeType currentUpgradeType;

    private void OnEnable()
    {
        PlayerUpgrades.onPlayerUpgradeSelected += UpgradeSelected;
        GunsUpgrade.onGunsUpgradeSelected += UpgradeSelected;
        BaseUpgrade.onBaseUpgradeSelected += UpgradeSelected;
    }

    private void OnDisable()
    {
        PlayerUpgrades.onPlayerUpgradeSelected -= UpgradeSelected;
        GunsUpgrade.onGunsUpgradeSelected -= UpgradeSelected;
        BaseUpgrade.onBaseUpgradeSelected -= UpgradeSelected;
    }


    public void Activate()
    {
        Time.timeScale = 0;
        sparks.Play();
        LeanTween.value(0, 1, 1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = true;
        }).setEaseInElastic().setIgnoreTimeScale(true);
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), ignoreTimeScale: true);
            CameraController.ShakeCameraAsync(0.3f, 25f);
        });
    }


    private void UpgradeSelected(UpgradeType upgradeType)
    {
        currentUpgradeType = upgradeType;
        switch (upgradeType)
        {
            case UpgradeType.Player:
                UpgradeEffect(new List<RectTransform> { playerCard, enemyCard }
                    , new List<RectTransform> { gunsCard, baseCard });
                break;
            case UpgradeType.Guns:
                UpgradeEffect(new List<RectTransform> { gunsCard, enemyCard }
                    , new List<RectTransform> { playerCard, baseCard });
                break;
            case UpgradeType.Base:
                UpgradeEffect(new List<RectTransform> { baseCard, enemyCard }
                    , new List<RectTransform> { playerCard, gunsCard });
                break;
        }
    }


    private void UpgradeEffect(List<RectTransform> cardsRemain, List<RectTransform> cardToDisappear)
    {
        LeanTween.value(1, 0, 1.2f).setOnUpdate(value =>
        {
            cardToDisappear.ForEach(card => card.localScale = new Vector3(value, value, value));
        }).setEaseInOutElastic().setIgnoreTimeScale(true);

        LeanTween.value(1f, 0, 1f).setOnUpdate(val =>
        {
            cardsRemain[0].localScale = new Vector3(val, val, val);
            Cursor.visible = false;
        }).setIgnoreTimeScale(true).setEaseInOutElastic().setOnComplete(() =>
        {
            enemyCard.gameObject.SetActive(true);
            onSecondaryCardDisappear?.Invoke();
            cardToDisappear.ForEach(card => card.gameObject.SetActive(false));
            UniTask.Void(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.55f), ignoreTimeScale: true);
                CameraController.ShakeCameraAsync(0.3f, 25f);
            });
            LeanTween.value(0, 1, 1f).setOnUpdate(value =>
            {
                cardsRemain.ForEach(card => card.localScale = new Vector3(value, value, value));
            }).setIgnoreTimeScale(true).setEaseInOutElastic().setOnComplete(() =>
            {
                onUpgradeCardInFront?.Invoke(currentUpgradeType);
                LeanTween.value(1, 1.5f, 0.5f).setOnUpdate(value =>
                {
                    cardsRemain[0].localScale = new Vector3(value, value, value);
                    cardsRemain[1].localScale = new Vector3(1 - value + 1, 1 - value + 1, 1 - value + 1);
                }).setIgnoreTimeScale(true).setEaseInOutElastic().setOnComplete(() =>
                {
                    onUpgradeCardInFront?.Invoke(UpgradeType.Enemy);
                    LeanTween.value(1.5f, 0.5f, 1f).setOnUpdate(value =>
                    {
                        cardsRemain[0].localScale = new Vector3(value, value, value);
                        cardsRemain[1].localScale =
                            new Vector3(0.5f + 1.5f - value, 0.5f + 1.5f - value, 0.5f + 1.5f - value);
                    }).setIgnoreTimeScale(true).setEaseInOutElastic().setDelay(0.3f).setOnComplete(() =>
                    {
                        LeanTween.value(0.5f, 1, 0.3f).setOnUpdate(value =>
                        {
                            cardsRemain[0].localScale = new Vector3(value, value, value);
                            cardsRemain[1].localScale = new Vector3(1.5f - value + 0.5f, 1.5f - value + 0.5f, 1.5f - value + 0.5f);
                        }).setIgnoreTimeScale(true).setEaseInQuad().setDelay(0.3f).setOnComplete(() =>
                        {
                            LeanTween.value(1, 0, 1f).setOnUpdate(value =>
                            {
                                cardsRemain.ForEach(card => card.localScale = new Vector3(value, value, value));
                            }).setIgnoreTimeScale(true).setEaseInOutElastic().setDelay(1f).setOnComplete(() =>
                            {
                                sparks.Stop();
                                cardToDisappear.ForEach(card => card.gameObject.SetActive(true));
                                onPanelDisappear?.Invoke();
                                enemyCard.gameObject.SetActive(false);
                                Time.timeScale = 1;
                            });
                        });
                    });
                });
            });
        });
    }
}

