using System.Collections;
using System.Collections.Generic;
using ConstantsValues;
using UnityEngine;

public class UpgradeObject
{
    public CardClass cardClass;
    public Texture2D cardImage;
    public string cardMainUpgrade;
    public string shortDescription;
    public string upgradeDescription;

    public UpgradeObject(CardClass cardClass, Texture2D cardImage, string cardMainUpgrade, string upgradeDescription, string shortDescription)
    {
        this.shortDescription = shortDescription;
        this.cardClass = cardClass;
        this.cardImage = cardImage;
        this.cardMainUpgrade = cardMainUpgrade;
        this.upgradeDescription = upgradeDescription;
    }

}
