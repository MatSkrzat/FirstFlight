using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region STATIC
    public static ScoreManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    private static int currentScore = 0;

    public static void StartCountingScore()
    {
        instance.InvokeRepeating("AddScore", 0F, 0.4F);
    }

    public static void ResetScore()
    {
        currentScore = 0;
    }

    public static int GetCurrentScore()
    {
        return currentScore;
    }

    public void AddScore()
    {
        currentScore += 1;
        GameManager.UI.UpdateScoreAmount(currentScore);
    }

    public static bool SaveWhenNewHighscore()
    {
        var highscore = GameStateManager.CurrentGameState.highScore;
        if (currentScore > highscore)
        {
            GameStateManager.UpdateAndSaveHighScore(currentScore);
            return true;
        }
        return false;
    }

    public static void SetValuesToDefault()
    {
        currentScore = 0;
    }
}
