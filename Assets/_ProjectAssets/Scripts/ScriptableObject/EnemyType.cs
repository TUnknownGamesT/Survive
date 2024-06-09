using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyType", order = 1)]
public class EnemyType : ScriptableObject
{
    public GameObject armPrefab;
    public Constants.EnemyType enemyType;
    public int health;
    public float damping;
    public float stoppingDistance;
    public float pauseBetweenMovement;
    public float speed;

    //References to be set in the AIBrain and pass forward to
    // to states
    [HideInInspector]
    public SoundComponent soundComponent;
    [HideInInspector]
    public Transform armSpawnPoint;
    [HideInInspector]
    public GameObject aiBody;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector] 
    public List<Transform> travelPoints = new();


}
