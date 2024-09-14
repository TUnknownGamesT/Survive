using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static int score = 0;

    private static int daysSurvived;
    private static int kills;
    public static int Score { get => score; set => score += 1; }
    public static int DaysSurvived { get => daysSurvived; set => daysSurvived = +1; }
    public static int Kills { get => kills; set => kills = +1; }

    private void OnEnable()
    {
        AIBrain.onEnemyDeath += IncreaseScore;
        AIGroupBrain.onEnemyDeath += IncreaseScore;
        GameManager.onNextLvl += IncreaseDaysSurvived;
        AIBrain.onEnemyDeath += IncreaseKills;
        AIGroupBrain.onEnemyDeath += IncreaseKills;
    }

    private void OnDisable()
    {
        AIBrain.onEnemyDeath -= IncreaseScore;
        AIGroupBrain.onEnemyDeath -= IncreaseScore;
        GameManager.onNextLvl -= IncreaseDaysSurvived;
        AIBrain.onEnemyDeath -= IncreaseKills;
        AIGroupBrain.onEnemyDeath -= IncreaseKills;
    }

    private void IncreaseScore()
    {
        Score++;
    }

    private void IncreaseDaysSurvived()
    {
        DaysSurvived++;
    }

    private void IncreaseKills()
    {
        Kills++;
    }
}
