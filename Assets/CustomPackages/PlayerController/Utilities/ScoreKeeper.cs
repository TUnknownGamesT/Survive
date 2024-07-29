using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static int score=0;
    public int visualScore;
    public static  int Score
    {
        get =>score;
        set => score += 1;
    }


    private void OnEnable()
    {
        AIBrain.onEnemyDeath += IncreaseScore;
        AIGroupBrain.onEnemyDeath += IncreaseScore;
    }
    
    private void OnDisable()
    {
        AIBrain.onEnemyDeath -= IncreaseScore;
        AIGroupBrain.onEnemyDeath -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        visualScore++;
        Debug.LogWarning("Score increased");
        Score++;
    }
}
