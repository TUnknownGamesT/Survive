using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LvlSettings", menuName = "ScriptableObjects/LvlSettings", order = 3)]
public class LvlSettings : ScriptableObject
{
    public int enemiesToSpawn;
    public float pauseBetweenRounds;
    [Tooltip("How many rounds to pass to increase the number of enemies per round")]
    public int roundsToPass;
    public int enemiesPerRoundIncrease;
}
