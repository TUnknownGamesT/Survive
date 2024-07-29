using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextHighLight : MonoBehaviour,IPointerExitHandler,IPointerEnterHandler
{

    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textMeshPro.fontStyle = FontStyles.Normal|FontStyles.Bold;
        _textMeshPro.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textMeshPro.fontStyle = FontStyles.Bold;
        _textMeshPro.color = Color.yellow;
    }
}
