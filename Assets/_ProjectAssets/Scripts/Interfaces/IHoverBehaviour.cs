using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverBehaviour : IPointerEnterHandler, IPointerExitHandler
{
    // This method is called when the pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered the UI element.");
        // Add your hover logic here
    }

    // This method is called when the pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited the UI element.");
        // Add your hover exit logic here
    }
}