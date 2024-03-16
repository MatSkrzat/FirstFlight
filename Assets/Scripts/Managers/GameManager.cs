using System.Collections;
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
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
    #endregion
    public static bool IsGamePaused { get; private set; } = true;
    public static bool IsGameStarted { get; private set; } = false;
    public static bool IsGameRandom { get; private set; } = false;
    public static UIManager UI { get; private set; }
    public static SoundManager SM { get; private set; }
    public static Camera MainCamera { get; private set; }
    public static int FirstSelectedLevel { get; private set; } = 1;

    private void Start()
    {
        UI = gameObject.GetComponent<UIManager>();
        SM = gameObject.GetComponent<SoundManager>();
        MainCamera = Camera.main;
        SM.PlayLoopedSound(SM.Wind);
    }
    public static void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public static void ResumeGame()
    {
        UI.LoadCountdownPanel();
        UI.CloseCornerButton();
        UI.ClosePausePanel();
        instance.StartCoroutine(Invoke_SubstractCountdownValue(4));
    }

    private static IEnumerator Invoke_SubstractCountdownValue(int countdownValue)
    {
        while (true)
        {
            if (Time.timeScale != 0) break;
            countdownValue -= 1;
            if (countdownValue < 1)
            {
                UnpauseGame();
                UI.CloseCountdownPanel();
                UI.LoadCornerButton(FilenameDictionary.PAUSE_ICON);
                break;
            }
            UI.UpdateCountdownValue(countdownValue);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public static IEnumerator Invoke_StartDelayedGame(int countdownValue, int level = -1)
    {
        UI.LoadCountdownPanel();
        while (true)
        {
            countdownValue -= 1;
            if (countdownValue < 1)
            {
                UnpauseGame();
                UI.CloseCountdownPanel();
                break;
            }
            UI.UpdateCountdownValue(countdownValue);
            yield return new WaitForSecondsRealtime(1f);
        }

        if (level >= 0)
        {
            StartGame(level);
        }
        else
        {
            StartRandomGame();
        }

    }

    public static void EndGame()
    {
        GameStateManager.UpdateOwnedCoins(CoinsManager.ownedCoins);
        GameStateManager.SaveCurrentGameState();
        UI.LoadEndGamePanel();
    }

    public static void StartGame(int level)
    {
        FirstSelectedLevel = level;
        LevelsManager.LoadAndSetLevel(level);
        IsGameStarted = true;
        TreeManager.StartMovingTree();
        BackgroundsManager.SetAllBackgroundsSpeedToLevelSpeed();
        CoinsManager.SetCoins(GameStateManager.CurrentGameState.ownedCoins);
        UI.LoadCornerButton(FilenameDictionary.PAUSE_ICON);
        UI.StartPlayTrailEmmiter();
        UI.LoadTapPanel();
    }

    public static void StartRandomGame()
    {
        LevelsManager.currentLevel = null;
        IsGameRandom = true;
        LevelsManager.LoadRandomLevel();
        IsGameStarted = true;
        TreeManager.StartMovingTree();
        BackgroundsManager.SetAllBackgroundsSpeedToLevelSpeed();
        CoinsManager.SetCoins(GameStateManager.CurrentGameState.ownedCoins);
        UI.LoadCornerButton(FilenameDictionary.PAUSE_ICON);
        UI.StartPlayTrailEmmiter();
    }

    public static void SetValuesToDefault() 
    {
        IsGameStarted = false;
        IsGamePaused = true;
        IsGameRandom = false;
        Time.timeScale = 1;
    }

    public static void ResetGame()
    {
        TreeManager.SetValuesToDefault();
        PlayerManager.SetValuesToDefault();
        BackgroundsManager.SetValuesToDefault();
        LevelsManager.SetValuesToDefault();
        ScoreManager.SetValuesToDefault();
        SetValuesToDefault();
        SceneManager.LoadScene("GameScene");
    }
}
