using TMPro;
using UnityEngine;
using ConstantsValues;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class CardConstructor : MonoBehaviour
{
    public static Action<int> onCardClicked;

    [Header("CardIndex")]
    public int index;
    [Header(" ")]
    public TextMeshProUGUI cardClass;
    public RawImage image;
    public TextMeshProUGUI cardMainUpgrade;
    public TextMeshProUGUI upgradeDescription;
    public RawImage arrow;



    public void ConstructCard(UpgradeObject upgradeObject)
    {
        this.cardClass.text = upgradeObject.cardClass.ToString();
        this.cardMainUpgrade.text = $"{upgradeObject.cardMainUpgrade}% <color=#29ABE2>{upgradeObject.shortDescription}</color>";
        this.upgradeDescription.text = upgradeObject.upgradeDescription;
        SetArrowColor(upgradeObject.cardClass);
        ResizeImage(upgradeObject.cardImage, 95.6704f, 112.5304f, 90, 110);
    }


    private void SetArrowColor(CardClass cardClass)
    {
        switch (cardClass)
        {
            case CardClass.COMMON:
                arrow.color = Constants.instance.commonColor;
                break;

            case CardClass.RARE:
                arrow.color = Constants.instance.rareColor;
                break;

            case CardClass.EPIC:
                arrow.color = Constants.instance.epicColor;
                break;
            default:
                UnityEngine.Debug.LogError("CardClass not found");
                break;
        }
    }

    private void ResizeImage(Texture2D texture, float maxWidth, float maxHeight, float minWidth, float minHeight)
    {
        if (texture == null)
        {
            UnityEngine.Debug.LogError("Texture is null");
            return;
        }

        float width = texture.width;
        float height = texture.height;

        float aspectRatio = width / height;

        if (width > maxWidth || height > maxHeight)
        {
            if (width / maxWidth > height / maxHeight)
            {
                width = maxWidth;
                height = width / aspectRatio;
            }
            else
            {
                height = maxHeight;
                width = height * aspectRatio;
            }
        }

        if (width < minWidth || height < minHeight)
        {
            if (width / minWidth < height / minHeight)
            {
                width = minWidth;
                height = width / aspectRatio;
            }
            else
            {
                height = minHeight;
                width = height * aspectRatio;
            }
        }

        image.texture = texture;
        image.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void OnClick()
    {
        Debug.Log("Card Clicked");
        onCardClicked?.Invoke(index);
    }


}
