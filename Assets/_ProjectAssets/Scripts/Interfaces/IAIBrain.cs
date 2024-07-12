
using UnityEngine;

public interface IAIBrain
{
    public void BaseInView(Transform basePoint);
    public void PlayerInView();
    public void PlayerOutOfView();
}
