using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;
using ConstantsValues;
using System.Linq;


public class UpgradePanelBehaviour : MonoBehaviour
{
    public static Action<UpgradeType> onUpgradeCardInFront;

    //TODO rethink the animation 
    public List<RectTransform> cards;
    public RectTransform enemyCard;
    public VisualEffect sparks;

    public static Action onSecondaryCardDisappear;
    public static Action onPanelDisappear;

    private UpgradeType currentUpgradeType;

    private void OnEnable()
    {
        // PlayerUpgrades.onPlayerUpgradeSelected += UpgradeSelected;
        // GunsUpgrade.onGunsUpgradeSelected += UpgradeSelected;
        // BaseUpgrade.onBaseUpgradeSelected += UpgradeSelected;
        CardConstructor.onCardClicked += UpgradeSelected;
    }

    private void OnDisable()
    {
        // PlayerUpgrades.onPlayerUpgradeSelected -= UpgradeSelected;
        // GunsUpgrade.onGunsUpgradeSelected -= UpgradeSelected;
        // BaseUpgrade.onBaseUpgradeSelected -= UpgradeSelected;
        CardConstructor.onCardClicked -= UpgradeSelected;
    }


    public void Activate()
    {
        Time.timeScale = 0;
        sparks.Play();
        LeanTween.value(0, 1.5f, 1f).setOnUpdate(val =>
        {
            cards.ForEach(card => card.localScale = new Vector3(val, val, val));
        }).setEaseInElastic().setIgnoreTimeScale(true);
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f), ignoreTimeScale: true);
            CameraController.ShakeCameraAsync(0.3f, 25f);
        });
    }


    private void UpgradeSelected(int index)
    {
        List<RectTransform> deactivateCards = cards.Where((item, idx) => idx != index - 1).ToList();

        UpgradeEffect(new List<RectTransform> { cards[index - 1], enemyCard }
           , deactivateCards);
    }


    private void UpgradeEffect(List<RectTransform> cardsRemain, List<RectTransform> cardToDisappear)
    {
        LeanTween.value(1.5f, 0, 1.2f).setOnUpdate(value =>
        {
            cardToDisappear.ForEach(card => card.localScale = new Vector3(value, value, value));
        }).setEaseInOutElastic().setIgnoreTimeScale(true);

        LeanTween.value(1.5f, 0, 1f).setOnUpdate(val =>
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
            LeanTween.value(0, 1.5f, 1f).setOnUpdate(value =>
            {
                cardsRemain.ForEach(card => card.localScale = new Vector3(value, value, value));
            }).setIgnoreTimeScale(true).setEaseInOutElastic().setOnComplete(() =>
            {
                // onUpgradeCardInFront?.Invoke(currentUpgradeType);
                LeanTween.value(1.5f, 2f, 0.5f).setOnUpdate(value =>
                {
                    cardsRemain[0].localScale = new Vector3(value, value, value);
                    cardsRemain[1].localScale = new Vector3(1.5f - value + 1.5f, 1.5f - value + 1.5f, 1.5f - value + 1.5f);
                }).setIgnoreTimeScale(true).setEaseInOutElastic().setOnComplete(() =>
                {
                    // onUpgradeCardInFront?.Invoke(UpgradeType.Enemy);
                    LeanTween.value(2f, 1f, 1f).setOnUpdate(value =>
                    {
                        cardsRemain[0].localScale = new Vector3(value, value, value);
                        cardsRemain[1].localScale =
                            new Vector3(1f + 2f - value, 1f + 2f - value, 1f + 2f - value);
                    }).setIgnoreTimeScale(true).setEaseInOutElastic().setDelay(0.3f).setOnComplete(() =>
                    {
                        LeanTween.value(1f, 1.5f, 0.3f).setOnUpdate(value =>
                        {
                            cardsRemain[0].localScale = new Vector3(value, value, value);
                            cardsRemain[1].localScale = new Vector3(2f - value + 1f, 2f - value + 1f, 2f - value + 1f);
                        }).setIgnoreTimeScale(true).setEaseInQuad().setDelay(0.3f).setOnComplete(() =>
                        {
                            LeanTween.value(1.5f, 0, 1f).setOnUpdate(value =>
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

