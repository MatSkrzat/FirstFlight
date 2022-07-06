using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region STATIC
    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static bool GamePaused { get; private set; } = true;
    public static bool GameStarted { get; private set; } = false;
    public static bool GameOver { get; private set; } = false;
    private void Update()
    {
        if (PlayerManager.IsDead)
        {
            GameOver = true;
            StopGame();
        }
    }
    public void StopGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
    }

    public static void SetValuesToDefault()
    {
        GamePaused = true;
        GameStarted = false;
        GameOver = false;
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
        TreeModulesManager.SetValuesToDefault();
        PlayerManager.SetValuesToDefault();
        BackgroundsManager.SetValuesToDefault();
        LevelsManager.SetValuesToDefault();
        SetValuesToDefault();
        SceneManager.LoadScene(0);
    }
}
