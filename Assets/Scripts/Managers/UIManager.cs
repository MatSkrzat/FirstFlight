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

public enum ShopPanelChildren
{
    buttonLeft,
    buttonRight,
    priceLabel,
    nameLabel,
    coinImage,
    buySelectButton,
    iconBackground,
    icon
}

public enum PlayerChildren
{
    openedEye,
    wing,
    closedEye
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
    public GameObject shopPanel;
    public GameObject player;
    public Material featherMaterial;
    public Button[] levelsButtons;

    private const float HEART_GAMEOBJECT_SEPARATION = 120F;
    private readonly int LAST_LEVEL = Helper.LEVELS_COUNT;
    private const int FIRST_LEVEL = 1;
    private const int LEVELS_PER_SITE = 9;
    private const int DEFAULT_CHARACTER_ID = (int)CharacterIds.tom;

    private List<GameObject> heartGameObjects = new List<GameObject>();
    private Sprite grayHeartSprite;
    private GameObject heartPrefab;
    private int[] displayedLevels = new int[LEVELS_PER_SITE];
    private int currentSelectedCharacterId;

    private int logCounter = 0;

    private void Start()
    {
        LoadHearts();
        LoadCoins();
        SetCharacterTextures(GameStateManager.CurrentGameState.selectedCharacterId);
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

    public void LoadShopPanel()
    {
        scorePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        endGamePanel.SetActive(false);
        currentSelectedCharacterId = GameStateManager.CurrentGameState.selectedCharacterId;
        DisplayCharacter(currentSelectedCharacterId);
        shopPanel.SetActive(true);
    }

    public void LoadNextCharacter()
    {
        if (currentSelectedCharacterId >= PlayerHelper.CHARACTERS.Last().ID)
        {
            currentSelectedCharacterId = PlayerHelper.CHARACTERS.First().ID;
            DisplayCharacter(currentSelectedCharacterId);
        }
        else
        {
            currentSelectedCharacterId++;
            DisplayCharacter(currentSelectedCharacterId);
        }
    }

    public void LoadPreviousCharacter()
    {
        if (currentSelectedCharacterId <= PlayerHelper.CHARACTERS.First().ID)
        {
            currentSelectedCharacterId = PlayerHelper.CHARACTERS.Last().ID;
            DisplayCharacter(currentSelectedCharacterId);
        }
        else
        {
            currentSelectedCharacterId--;
            DisplayCharacter(currentSelectedCharacterId);
        }
    }

    public void DisplayCharacter(int characterId)
    {
        var character = PlayerHelper.CHARACTERS.Find(x => x.ID == characterId);
        var shopPanelBackground = shopPanel.transform.GetChild(0);
        shopPanelBackground.GetChild((int)ShopPanelChildren.icon).GetComponent<Image>().sprite
            = Resources.Load<Sprite>(PathsDictionary.GetPlayerPath(character.Name) + FilenameDictionary.CHARACTER_ICON);
        shopPanelBackground.GetChild((int)ShopPanelChildren.priceLabel).GetComponent<TextMeshProUGUI>().text = character.Price.ToString();
        shopPanelBackground.GetChild((int)ShopPanelChildren.nameLabel).GetComponent<TextMeshProUGUI>().text = character.Name;
        UpdateBuySelectButton(characterId);

    }

    public void BuySelectCharacter()
    {
        var character = PlayerHelper.CHARACTERS.Find(x => x.ID == currentSelectedCharacterId);
        if (character.IsOwned)
        {
            GameStateManager.SetSelectedCharacter(currentSelectedCharacterId);
            SetCharacterTextures(GameStateManager.CurrentGameState.selectedCharacterId);
        }
        else
        {
            ShopManager.UnlockCharacter(currentSelectedCharacterId);
        }
        UpdateBuySelectButton(currentSelectedCharacterId);
    }

    public void UpdateBuySelectButton(int characterId)
    {
        var character = PlayerHelper.CHARACTERS.Find(x => x.ID == characterId);
        var shopPanelBackground = shopPanel.transform.GetChild(0);
        if (character.IsOwned)
            shopPanelBackground.GetChild((int)ShopPanelChildren.buySelectButton).GetComponent<Image>().sprite
                = Resources.Load<Sprite>(PathsDictionary.GetFullPath(PathsDictionary.UI_BUTTONS, FilenameDictionary.CONFIRM_ICON));
        else
            shopPanelBackground.GetChild((int)ShopPanelChildren.buySelectButton).GetComponent<Image>().sprite
                = Resources.Load<Sprite>(PathsDictionary.GetFullPath(PathsDictionary.UI_BUTTONS, FilenameDictionary.SHOP_ICON));
        if (character.IsSelected)
            shopPanelBackground.GetChild((int)ShopPanelChildren.buySelectButton).GetComponent<Button>().interactable = false;
        else
            shopPanelBackground.GetChild((int)ShopPanelChildren.buySelectButton).GetComponent<Button>().interactable = true;
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

    public void SetCharacterTextures(int characterId)
    {
        string characterName = PlayerHelper.CHARACTERS.Find(x => x.ID == characterId).Name;
        string characterPath = PathsDictionary.GetPlayerPath(characterName);
        Debug.Log("CHANGING TEXTURE FOR: " + characterName);

        // changing the sprites for player gameobject
        player.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<Sprite>(characterPath + FilenameDictionary.BODY);
        player.transform.GetChild((int)PlayerChildren.openedEye).GetComponent<SpriteRenderer>().sprite
            = Resources.Load<Sprite>(characterPath + FilenameDictionary.OPENED_EYE);
        player.transform.GetChild((int)PlayerChildren.wing).GetComponent<SpriteRenderer>().sprite
            = Resources.Load<Sprite>(characterPath + FilenameDictionary.WING);
        player.transform.GetChild((int)PlayerChildren.closedEye).GetComponent<SpriteRenderer>().sprite
            = Resources.Load<Sprite>(characterPath + FilenameDictionary.CLOSED_EYE);

        // feather color setup by changing the particle material
        var featherMaterial = Resources.Load<Material>(PathsDictionary.FEATHER_MATERIALS + characterName);
        var particleRenderer = player.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
        particleRenderer.material = featherMaterial;
        particleRenderer.sharedMaterial = featherMaterial;
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
