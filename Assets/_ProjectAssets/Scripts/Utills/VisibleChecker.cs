using UnityEngine;

public class VisibleChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isVisible;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}
