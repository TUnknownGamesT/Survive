using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public static Action<float> onPlayerXpChanged;
    public static Action<int> onPlayerLevelUp;
    public static Action<float> onPlayerXpThresholdChanged;

    public float currentXp = 0.0f;
    public int currentLevel = 1;
    
    private float levelXpThreshold = 100.0f;

    public void IncreaseXp(float value)
    {
        currentXp += value;
        if (ShouldLevelUp())
        {
            LevelUp();
        }
        onPlayerXpChanged.Invoke(currentXp);
    }

    private void LevelUp()
    {
        currentLevel += 1;
        IncreaseLevelXpThreshold();
        ResetXp();
        onPlayerLevelUp.Invoke(currentLevel);
    }

    private bool ShouldLevelUp()
    {
        return currentXp >= levelXpThreshold;
    }
    
    private void IncreaseLevelXpThreshold()
    {
        levelXpThreshold += 50 * currentLevel;
        onPlayerXpThresholdChanged.Invoke(levelXpThreshold);
    }

    private void ResetXp()
    {
        currentXp = 0;
    }
}
