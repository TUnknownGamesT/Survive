using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject backGroundObject;
    public List<TextMeshProUGUI> textMeshProUGUI;

    [Header("3D Object")]
    public Color32 idleColor3DObject = Color.white;
    public Color32 hoverColor3DObject = Color.black;

    [Header("Text")]
    public Color32 idleColorText = Color.black;
    public Color32 hoverColorText = Color.white;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = hoverColor3DObject;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = hoverColorText;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = idleColor3DObject;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = idleColorText;
        }

    }

    void OnDisable()
    {
        backGroundObject.GetComponent<MeshRenderer>().material.color = idleColor3DObject;
        foreach (var textMeshProUGUI in textMeshProUGUI)
        {
            textMeshProUGUI.color = idleColorText;
        }
    }

}
