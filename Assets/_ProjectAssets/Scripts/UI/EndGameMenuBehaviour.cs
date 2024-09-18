using TMPro;
using UnityEngine;

public class EndGameMenuBehaviour : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI daysSurvivedText;
    public TextMeshProUGUI killsText;

    public void UpdateScoreText()
    {
        scoreText.text = $"Score: {ScoreKeeper.Score}";
        daysSurvivedText.text = $"Days Survived: {ScoreKeeper.DaysSurvived}";
        killsText.text = $"Kills: {ScoreKeeper.Kills}";
    }

}
