using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject backGroundObject;
    public List<TextMeshProUGUI> textMeshProUGUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = Color.red;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = Color.white;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = Color.white;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = Color.black;
        }

    }

    void OnDisable()
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = Color.white;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = Color.black;
        }
    }

}
