using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuElementBehaviour : MonoBehaviour, IHoverBehaviour
{
    public Image image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.enabled = false;
    }
}
