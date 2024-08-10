using UnityEngine;
using ConstantsValues;

public class WalkingStateGroup : IState
{
    
    private Transform aiBody;
    private bool hitPlayerBase;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is AIGroupBrain aiGroupBrain) {
            aiBody = aiGroupBrain.transform;
        }
    }

    public void OnEnter()
    {
        hitPlayerBase = false;
    }

    public void OnUpdate()
    {
        if(hitPlayerBase)
            return;
        ShootRaycast();
    }

    public void OnExit()
    {
       
    }
    
    private void ShootRaycast()
    {
        RaycastHit hit;
        Vector3 direction =  GameManager.playerBaseRef.position - aiBody.position;
        if (Physics.Raycast(aiBody.position, direction, out hit, Mathf.Infinity,Constants.instance.baseLayer))
        {
            hitPlayerBase = true;
            Debug.Log(hit.point);
            GameObject playerBaseHitPoint = new GameObject("PlayerBaseHitPoint");
            playerBaseHitPoint.transform.position = hit.point;
            aiBody.GetComponent<IAIBrain>().BaseInView(playerBaseHitPoint.transform);
        }
    }
    
    
    

}
