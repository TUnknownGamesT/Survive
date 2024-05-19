using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    public void HighLight();
    
    public void UnHighLight();

    public void HoldInteract()
    {
        Debug.Log("Hold Interact");
    }
    
    public void CancelHoldInteract()
    {
        Debug.Log("Cancel Hold Interact");
    }
    
    
}
