using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static int score=100;
    public static  int Score
    {
        get =>score;
        set => score += value;
    }


    private void OnEnable()
    {
        AIBrain.onEnemyDeath += IncreaseScore;
    }
    
    private void OnDisable()
    {
        AIBrain.onEnemyDeath -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        Score++;
    }
}
