using System;
using Cysharp.Threading.Tasks;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TextPopUpBehaviour : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private TypewriterByWord _typewriterByCharacter;

    private void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _typewriterByCharacter = transform.GetChild(0).GetComponent<TypewriterByWord>();
    }

    private void OnEnable()
    {
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            _typewriterByCharacter.StartShowingText();
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            _typewriterByCharacter.StartDisappearingText();
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            Destroy(gameObject);
        });


        LeanTween.move(gameObject, RandomDirection(), 0.3f).setEaseInOutQuad();
    }

    private Vector3 RandomDirection()
    {
        Vector3 positon = transform.position;
        return new Vector3(positon.x + Random.Range(-2f, 2f), positon.y, positon.z + Random.Range(-2f, 2f));
    }
    public void SetTextAndColor(Color32 color, string text)
    {
        _text.text = text;
        _text.color = color;
    }

}
