using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EndGamePanelChildren
{
    gameOverLabel,
    scoreLabel,
    scoreValueLabel,
    coinIcon,
    coinValueLabel,
    reloadButton,
    closeButton
}

public class UIManager : MonoBehaviour
{
    public GameObject jumpSecurityPanel;
    public GameObject mainMenuPanel;
    public GameObject healthStatusPanel;
    public GameObject scorePanel;
    public GameObject coinsAmountText;
    public GameObject levelsPanel;
    public GameObject endGamePanel;
    public GameObject debugText;
    public Button[] levelsButtons;

    private const float HEART_GAMEOBJECT_SEPARATION = 120F;
    private readonly int LAST_LEVEL = Helper.LEVELS_COUNT;
    private const int FIRST_LEVEL = 1;
    private const int LEVELS_PER_SITE = 9;

    private List<GameObject> heartGameObjects = new List<GameObject>();
    private Sprite grayHeartSprite;
    private GameObject heartPrefab;
    private int[] displayedLevels = new int[LEVELS_PER_SITE];

    private int logCounter = 0;

    private void Start()
    {
        LoadHearts();
        LoadCoins();
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    //Called when there is an exception
    void LogCallback(string condition, string stackTrace, LogType type)
    {
        UpdateDebugText(condition, stackTrace, type);
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }

    private void AddListenersTolevelsButtons()
    {
        for(int i = 0; i < displayedLevels.Length; i++)
        {
            int index = i;
            levelsButtons[index].onClick.RemoveAllListeners();
            levelsButtons[index].onClick.AddListener(delegate { StartGame(displayedLevels[index]); });
        }
    }

    private void LoadCoins()
    {
        UpdateCoinsAmount(GameStateManager.CurrentGameState.ownedCoins);
    }

    private void LoadHearts()
    {
        grayHeartSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.UI_HEARTS, FilenameDictionary.UI_HEART_GRAY));
        heartPrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.UI_HEART_PREFAB));

        for(int i = 0; i < PlayerManager.NumberOfLives; i++)
        {
            var heartGameObject = Instantiate(heartPrefab);
            heartGameObject.transform.SetParent(healthStatusPanel.transform);

            if (heartGameObjects.Count > 0)
                heartGameObject.transform.localPosition = new Vector3(heartGameObjects.Last().transform.localPosition.x + HEART_GAMEOBJECT_SEPARATION,
                    heartGameObject.transform.position.y,
                    heartGameObject.transform.position.z);
            else
                heartGameObject.transform.localPosition = new Vector3(heartGameObject.transform.position.x,
                    heartGameObject.transform.position.y,
                    heartGameObject.transform.position.z);

            heartGameObjects.Add(heartGameObject);

        }
    }

    public bool IsSecurityPanelClicked() => 
        EventSystem.current.currentSelectedGameObject == jumpSecurityPanel;
    public void StartGame(int level)
    {
        endGamePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(false);
        jumpSecurityPanel.SetActive(true);
        healthStatusPanel.SetActive(true);
        scorePanel.SetActive(true);
        GameManager.StartGame(level);
    }

    public void UpdateDisplayedHealth(int numberOfLives)
    {
        for (int i = heartGameObjects.Count; i > numberOfLives; i--)
        {
            var heartGameObjectSprite = heartGameObjects[i - 1].GetComponent<Image>();

            if (heartGameObjectSprite.overrideSprite != grayHeartSprite)
            {
                heartGameObjectSprite.gameObject.GetComponent<HeartAnimations>().PlayChange();
                heartGameObjectSprite.overrideSprite = grayHeartSprite;
            }
        }
    }

    public void UpdateCoinsAmount(int coinsAmount)
    {
        if (coinsAmountText.GetComponent<TextMeshProUGUI>() != null)
            coinsAmountText.GetComponent<TextMeshProUGUI>().text = coinsAmount.ToString();
    }

    public void LoadLevelsPanel()
    {
        scorePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        levelsPanel.SetActive(true);
        FillLevelButtons(FIRST_LEVEL);
    }

    public void UpdateDebugText(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            debugText.GetComponent<Text>().color = Color.red;
        }
        else
        {
            debugText.GetComponent<Text>().color = Color.white;
        }
        debugText.GetComponent<Text>().text += $"\n{logCounter}: {message}";
        logCounter++;

    }

    public void LoadEndGamePanel()
    {
        scorePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.scoreValueLabel).GetComponent<TextMeshProUGUI>().text = "0";
        endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.coinValueLabel).GetComponent<TextMeshProUGUI>().text = GameStateManager.CurrentGameState.ownedCoins.ToString();
        endGamePanel.SetActive(true);
    }

    public void LoadNextLevelsPage()
    {
        if (displayedLevels.Last() >= LAST_LEVEL)
            FillLevelButtons(FIRST_LEVEL);
        else
            FillLevelButtons(displayedLevels.Last() + 1);
    }

    public void LoadPreviousLevelsPage()
    {
        if (displayedLevels.First() <= FIRST_LEVEL)
            FillLevelButtons(LAST_LEVEL + FIRST_LEVEL - LEVELS_PER_SITE);
        else
            FillLevelButtons(displayedLevels.First() - LEVELS_PER_SITE);
    }

    private void FillLevelButtons(int firstLevelToLoadOnPage)
    {
        int level = firstLevelToLoadOnPage;
        for(int i = 0; i < levelsButtons.Length; i++)
        {
            levelsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();
            if (level > GameStateManager.CurrentGameState.lastLevel)
            {
                levelsButtons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                levelsButtons[i].GetComponent<Button>().interactable = true;
            }
            displayedLevels[i] = level;
            level++;
        }
        AddListenersTolevelsButtons();
    }
}
