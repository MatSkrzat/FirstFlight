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
    public bool GamePaused { get; private set; } = true;
    public bool GameStarted { get; private set; } = false;
    public bool GameOver { get; private set; } = false;
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

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
