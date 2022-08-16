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
    public static bool IsGamePaused { get; private set; } = true;
    public static bool IsGameStarted { get; private set; } = false;
    public static UIManager UI { get; private set; }
    public static Camera MainCamera { get; private set; }

    private void Start()
    {
        UI = gameObject.GetComponent<UIManager>();
        MainCamera = Camera.main;
    }
    public void StopGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public static void StartGame()
    {
        IsGameStarted = true;
        TreeManager.StartMovingTree();
    }

    public static void SetValuesToDefault() 
    {
        IsGameStarted = false;
        IsGamePaused = true;        
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
        TreeManager.SetValuesToDefault();
        PlayerManager.SetValuesToDefault();
        BackgroundsManager.SetValuesToDefault();
        LevelsManager.SetValuesToDefault();
        SetValuesToDefault();
        SceneManager.LoadScene(0);
    }
}
