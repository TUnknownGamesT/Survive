using UnityEngine;

public class WalkingStateGroup : IState
{
    
    private Transform aiBody;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is AIGroupBrain aiGroupBrain) {
            aiBody = aiGroupBrain.transform;
        }
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        ShootRaycast();
    }

    public void OnExit()
    {
       
    }
    
    private void ShootRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(aiBody.position, GameManager.playerBaseRef.position, out hit, 20, Constants.instance.baseLayer))
        {
            Debug.Log("Base in View");
            GameObject playerBaseHitPoint = new GameObject("PlayerBaseHitPoint");
            playerBaseHitPoint.transform.position = hit.point;
            aiBody.gameObject.GetComponent<IAIBrain>().BaseInView(playerBaseHitPoint.transform);
        }
    }
    
    
    

}
