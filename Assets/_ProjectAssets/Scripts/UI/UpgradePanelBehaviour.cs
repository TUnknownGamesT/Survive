using System;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Player,
    Guns,
    Base,
    Enemy
}

public class UpgradePanelBehaviour : MonoBehaviour
{
    public RectTransform playerCard;
    public RectTransform gunsCard;
    public RectTransform baseCard;
    public RectTransform enemyCard;

    public static Action onSecondaryCardDisappear;
    public static Action onPanelDisappear;

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
        LeanTween.value(0, 1, 1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = true;
        }).setEaseInOutElastic();
    }


    private void UpgradeSelected(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Player:
                UpgradeEffect(new List<RectTransform> {playerCard,enemyCard}
                    , new List<RectTransform> {gunsCard, baseCard});
                break;
            case UpgradeType.Guns:
                UpgradeEffect(new List<RectTransform> {gunsCard,enemyCard}
                    , new List<RectTransform> {playerCard, baseCard});
                break;
            case UpgradeType.Base:
                UpgradeEffect(new List<RectTransform> {baseCard,enemyCard}
                    , new List<RectTransform> {playerCard, gunsCard});
                break;
        }
    }


    private void UpgradeEffect(List<RectTransform> cardsRemain, List<RectTransform> cardToDisappear) 
    {
        LeanTween.value(1,0,1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = false;
        }).setEaseInOutElastic().setOnComplete(() =>
        {
            enemyCard.gameObject.SetActive(true);
            onSecondaryCardDisappear?.Invoke();
            cardToDisappear.ForEach(card => card.gameObject.SetActive(false));
            LeanTween.value(0, 1, 1f).setOnUpdate(value =>
            {
                cardsRemain.ForEach(card => card.localScale = new Vector3(value, value, value));
            }).setEaseInOutElastic().setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1f).setOnUpdate(value =>
                {
                    cardsRemain.ForEach(card => card.localScale = new Vector3(value, value, value));
                }).setEaseInOutElastic().setDelay(1f).setOnComplete(() =>
                {
                    cardToDisappear.ForEach(card => card.gameObject.SetActive(true));
                    onPanelDisappear?.Invoke();
                    enemyCard.gameObject.SetActive(false);
                });
            });
        });
    }
    

    

   
}
