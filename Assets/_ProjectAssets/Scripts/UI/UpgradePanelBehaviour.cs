using System;
using UnityEngine;


public class UpgradePanelBehaviour : MonoBehaviour
{
    public RectTransform playerCard;
    public RectTransform gunsCard;
    public RectTransform baseCard;

    public static Action onSecondaryCardDisappear;
    public static Action onPanelDisappear;

    private void OnEnable()
    {
        PlayerUpgrades.onPlayerUpgradeSelected += UpgradePlayer;
        GunsUpgrade.onGunsUpgradeSelected += UpgradeGuns;
        BaseUpgrade.onBaseUpgradeSelected += UpgradeBase;
    }

    private void OnDisable()
    {
        PlayerUpgrades.onPlayerUpgradeSelected -= UpgradePlayer;
        GunsUpgrade.onGunsUpgradeSelected -= UpgradeGuns;
        BaseUpgrade.onBaseUpgradeSelected -= UpgradeBase;
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
    


    private void UpgradePlayer()
    {
        LeanTween.value(1,0,1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = false;
        }).setEaseInOutElastic().setOnComplete(() =>
        {
            onSecondaryCardDisappear?.Invoke();
            gunsCard.gameObject.SetActive(false);
            baseCard.gameObject.SetActive(false);
            LeanTween.value(0, 1, 1f).setOnUpdate(value =>
            {
                playerCard.localScale = new Vector3(value, value, value);
            }).setEaseInOutElastic().setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1f).setOnUpdate(value =>
                {
                    playerCard.localScale = new Vector3(value, value, value);
                }).setEaseInOutElastic().setDelay(1f).setOnComplete(() =>
                {
                    gunsCard.gameObject.SetActive(true);
                    baseCard.gameObject.SetActive(true);
                    onPanelDisappear?.Invoke();
                });
            });
        });
    }

    private void UpgradeGuns()
    {
        LeanTween.value(1,0,1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = false;
        }).setEaseInOutElastic().setOnComplete(() =>
        {
            onSecondaryCardDisappear?.Invoke();
            playerCard.gameObject.SetActive(false);
            baseCard.gameObject.SetActive(false);
            LeanTween.value(0, 1, 1f).setOnUpdate(value =>
            {
                gunsCard.localScale = new Vector3(value, value, value);
            }).setEaseInOutElastic().setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1f).setOnUpdate(value =>
                {
                    gunsCard.localScale = new Vector3(value, value, value);
                }).setEaseInOutElastic().setDelay(1f).setOnComplete(() =>
                {
                    playerCard.gameObject.SetActive(true);
                    baseCard.gameObject.SetActive(true);
                    onPanelDisappear?.Invoke();
                });
            });
        });
    }

    private void UpgradeBase()
    {
        LeanTween.value(1,0,1f).setOnUpdate(val =>
        {
            playerCard.localScale = new Vector3(val, val, val);
            gunsCard.localScale = new Vector3(val, val, val);
            baseCard.localScale = new Vector3(val, val, val);
            Cursor.visible = false;
        }).setEaseInOutElastic().setOnComplete(() =>
        {
            onSecondaryCardDisappear?.Invoke();
            playerCard.gameObject.SetActive(false);
            gunsCard.gameObject.SetActive(false);
            LeanTween.value(0, 1, 1f).setOnUpdate(value =>
            {
                baseCard.localScale = new Vector3(value, value, value);
            }).setEaseInOutElastic().setOnComplete(() =>
            {
                LeanTween.value(1, 0, 1f).setOnUpdate(value =>
                {
                    baseCard.localScale = new Vector3(value, value, value);
                }).setEaseInOutElastic().setDelay(1f).setOnComplete(() =>
                {
                    playerCard.gameObject.SetActive(true);
                    gunsCard.gameObject.SetActive(true);
                    onPanelDisappear?.Invoke();
                });
            });
        });
    }
}
