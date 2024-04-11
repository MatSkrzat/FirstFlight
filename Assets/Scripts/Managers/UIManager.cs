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
    scoreIcon,
    scoreValueLabel,
    coinIcon,
    coinValueLabel,
    reloadButton,
    closeButton
}

public enum LevelButtonChildren
{
    text,
    star
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

public class UIManager : MonoBehaviour
{
    public GameObject jumpSecurityPanel;
    public GameObject mainMenuPanel;
    public GameObject healthStatusPanel;
    public GameObject coinPanel;
    public GameObject coinsAmountText;
    public GameObject levelsPanel;
    public GameObject endGamePanel;
    public GameObject debugText;
    public GameObject shopPanel;
    public GameObject scorePanel;
    public GameObject countdownPanel;
    public GameObject pausePanel;
    public GameObject hidingPanel;
    public GameObject tapPanel;
    public GameObject cornerButton;
    public GameObject player;
    public GameObject finishComicPanel;
    public Material featherMaterial;
    public Button[] levelsButtons;
    public Button infinityModeButton;

    private const float HEART_GAMEOBJECT_SEPARATION = 120F;
    private readonly int LAST_LEVEL = Helper.LEVELS_COUNT;
    private const int FIRST_LEVEL = 1;
    private const int LEVELS_PER_SITE = 9;

    private List<GameObject> heartGameObjects = new List<GameObject>();
    private Sprite grayHeartSprite;
    private Sprite redHeartSprite;
    private GameObject heartPrefab;
    private ParticleSystem playerTrailEmmitterParticleSystem;
    private int[] displayedLevels = new int[LEVELS_PER_SITE];
    private int currentSelectedCharacterId;

    private List<string> logsList = new List<string>();

    private void Start()
    {
        LoadHearts();
        LoadCoins();
        LoadCornerButton(FilenameDictionary.SOUND_ON_ICON);
        SelectCharacter(GameStateManager.CurrentGameState.selectedCharacterId);
        playerTrailEmmitterParticleSystem = player.transform.GetChild((int)PlayerChildren.trailEmitter).GetComponent<ParticleSystem>();
        currentSelectedCharacterId = GameStateManager.CurrentGameState.selectedCharacterId;
    }

    private void AddListenersTolevelsButtons()
    {
        for(int i = 0; i < displayedLevels.Length; i++)
        {
            int index = i;
            levelsButtons[index].onClick.RemoveAllListeners();
            levelsButtons[index].onClick.AddListener(delegate { StartGame(false, displayedLevels[index]); });
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
        redHeartSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.UI_HEARTS, FilenameDictionary.UI_HEART_RED));
        heartPrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.UI_HEART_PREFAB));

        for(int i = 0; i < PlayerManager.NumberOfLives; i++)
        {
            var heartGameObject = Instantiate(heartPrefab);
            heartGameObject.transform.SetParent(healthStatusPanel.transform);
            heartGameObject.transform.localScale = Vector3.one;

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
    public void StartGame(bool isInfinity, int level = 0)
    {
        GameManager.SM.PlaySingleSound(GameManager.SM.Button);
        CloseLevelsPanel();
        endGamePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(true);
        healthStatusPanel.SetActive(true);
        coinPanel.SetActive(true);
        cornerButton.SetActive(false);
        if (isInfinity)
        {
            GameManager.instance.StartCoroutine(GameManager.Invoke_StartDelayedGame(4));
            scorePanel.SetActive(true);
        }
        else
        {
            GameManager.instance.StartCoroutine(GameManager.Invoke_StartDelayedGame(4, level));
            scorePanel.SetActive(false);
        }
    }

    public void StartPlayTrailEmmiter()
    {
        playerTrailEmmitterParticleSystem.Play();
    }

    public void SetPlayerTrailEmmiterSpeed(float particleSpeed)
    {
        var mainStartSpeed = playerTrailEmmitterParticleSystem.main.startSpeed;
        mainStartSpeed.constant = particleSpeed;
    }

    public void UpdateDisplayedHealth(int numberOfLives)
    {
        for (int i = 0; i < heartGameObjects.Count; i++)
        {
            var heartGameObjectSprite = heartGameObjects[i].GetComponent<Image>();
            heartGameObjectSprite.gameObject.GetComponent<HeartAnimations>().PlayChange();
            heartGameObjectSprite.overrideSprite = redHeartSprite;
        }
        for (int i = heartGameObjects.Count; i > numberOfLives; i--)
        {
            var heartGameObjectSprite = heartGameObjects[i - 1].GetComponent<Image>();
            heartGameObjectSprite.overrideSprite = grayHeartSprite;
        }
    }

    public void UpdateCoinsAmount(int coinsAmount)
    {
        if (coinsAmountText.GetComponent<TextMeshProUGUI>() != null)
            coinsAmountText.GetComponent<TextMeshProUGUI>().text = coinsAmount.ToString();
    }

    public void LoadLevelsPanel()
    {
        coinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        levelsPanel.SetActive(true);
        cornerButton.SetActive(false);
        infinityModeButton.interactable = GameStateManager.CurrentGameState.lastLevel >= LevelsManager.INFINITY_LEVEL;
        FillLevelButtons(FIRST_LEVEL);
    }

    public void LoadHidingPanelAndResetGame()
    {
        hidingPanel.GetComponent<HidingPanelAnimations>().PlayClose();
    }

    public void CloseHidingPanel()
    {
        hidingPanel.GetComponent<HidingPanelAnimations>().PlayOpen();
    }

    public void CloseLevelsPanel()
    {
        coinPanel.SetActive(true);
        mainMenuPanel.SetActive(true);
        jumpSecurityPanel.SetActive(false);
        cornerButton.SetActive(true);
        levelsPanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void LoadCountdownPanel()
    {
        jumpSecurityPanel.SetActive(false);
        countdownPanel.SetActive(true);
    }

    public void CloseCountdownPanel()
    {
        jumpSecurityPanel.SetActive(true);
        countdownPanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void UpdateCountdownValue(int value)
    {
        if(value < 0)
        {
            CloseCountdownPanel();
            return;
        }
        countdownPanel.GetComponent<PanelAnimations>().PlayRefresh();
        countdownPanel.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
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
        logsList.Add(message);
        if (logsList.Count > 5)
        {
            logsList.RemoveAt(0);
        }
        debugText.GetComponent<Text>().text = "";
        foreach (var log in logsList)
        {
            debugText.GetComponent<Text>().text += $"\n#: {log}";
        }
    }

    public void LoadEndGamePanel()
    {
        coinPanel.SetActive(false);
        scorePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        cornerButton.SetActive(false);

        var score = endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.scoreValueLabel);
        var scoreLabel = endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.scoreLabel);
        scoreLabel.gameObject.SetActive(false);

        // show score panel
        if (ScoreManager.GetCurrentScore() != 0)
        {
            // show highscore notification
            if (ScoreManager.GetCurrentScore() > GameStateManager.CurrentGameState.highScore)
            {
                scoreLabel.gameObject.SetActive(true);
                scoreLabel.GetComponent<TextMeshProUGUI>().text = Helper.HIGHSCORE_LABEL;
                PlayConfettiParticles();
                GameManager.SM.PlaySingleSound(GameManager.SM.Highscore);
            }
            ScoreManager.SaveWhenNewHighscore();
            score.gameObject.SetActive(true);
            score.GetComponent<TextMeshProUGUI>().text = ScoreManager.GetCurrentScore().ToString();
        }
        // hide score panel when not infinite mode
        else
        {
            score.gameObject.SetActive(false);
            endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.scoreLabel).gameObject.SetActive(false);
            endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.scoreIcon).gameObject.SetActive(false);
        }
        endGamePanel.transform.GetChild(0).GetChild((int)EndGamePanelChildren.coinValueLabel).GetComponent<TextMeshProUGUI>().text = GameStateManager.CurrentGameState.ownedCoins.ToString();

        endGamePanel.SetActive(true);
    }

    public void CloseEndGamePanel()
    {
        // this function will also reset the scene in animation state "exit" attached to EndGamePanel
        coinPanel.SetActive(false);
        scorePanel.SetActive(false);
        endGamePanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void LoadFinishComicPanel()
    {
        coinPanel.SetActive(false);
        scorePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        cornerButton.SetActive(false);

        var characterName = PlayerHelper.CHARACTERS.Find(x => x.ID == currentSelectedCharacterId).Name.ToLower();

        finishComicPanel.GetComponent<Image>().sprite
            = Resources.Load<Sprite>(PathsDictionary.COMICSES + characterName);

        finishComicPanel.SetActive(true);
        GameManager.SM.PlaySingleSound(GameManager.SM.Highscore);
    }

    public void CloseFinishComicPanel()
    {
        // this function will also reset the scene in animation state "exit" attached to EndGamePanel
        coinPanel.SetActive(false);
        scorePanel.SetActive(false);
        finishComicPanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void LoadShopPanel()
    {
        coinPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        endGamePanel.SetActive(false);
        cornerButton.SetActive(false);
        currentSelectedCharacterId = GameStateManager.CurrentGameState.selectedCharacterId;
        DisplayCharacter(currentSelectedCharacterId);
        shopPanel.SetActive(true);
    }

    public void CloseShopPanel()
    {
        mainMenuPanel.SetActive(true);
        jumpSecurityPanel.SetActive(false);
        healthStatusPanel.SetActive(false);
        endGamePanel.SetActive(false);
        cornerButton.SetActive(true);
        shopPanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void LoadTapPanel()
    {
        tapPanel.SetActive(true);
    }

    public void LoadNextCharacter()
    {
        shopPanel.GetComponent<PanelAnimations>().PlayRefresh();
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
        shopPanel.GetComponent<PanelAnimations>().PlayRefresh();
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
        if (character.IsOwned)
        {
            shopPanelBackground.GetChild((int)ShopPanelChildren.coinImage).gameObject.SetActive(false);
            shopPanelBackground.GetChild((int)ShopPanelChildren.priceLabel).gameObject.SetActive(false);
        }
        else
        {
            shopPanelBackground.GetChild((int)ShopPanelChildren.coinImage).gameObject.SetActive(true);
            shopPanelBackground.GetChild((int)ShopPanelChildren.priceLabel).gameObject.SetActive(true);
            shopPanelBackground.GetChild((int)ShopPanelChildren.priceLabel).GetComponent<TextMeshProUGUI>().text = character.Price.ToString();
        }
        shopPanelBackground.GetChild((int)ShopPanelChildren.nameLabel).GetComponent<TextMeshProUGUI>().text = character.Name;
        UpdateBuySelectButton(characterId);

    }

    public void BuySelectCharacter()
    {
        var character = PlayerHelper.CHARACTERS.Find(x => x.ID == currentSelectedCharacterId);
        if (character.IsOwned)
        {
            GameStateManager.SetSelectedCharacter(currentSelectedCharacterId);
            SelectCharacter(GameStateManager.CurrentGameState.selectedCharacterId);
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
        {
            shopPanelBackground.GetChild((int)ShopPanelChildren.buySelectButton).GetComponent<Image>().sprite
                = Resources.Load<Sprite>(PathsDictionary.GetFullPath(PathsDictionary.UI_BUTTONS, FilenameDictionary.CONFIRM_ICON));
            shopPanelBackground.GetChild((int)ShopPanelChildren.priceLabel).gameObject.SetActive(false);
            shopPanelBackground.GetChild((int)ShopPanelChildren.coinImage).gameObject.SetActive(false);
        }
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
        levelsPanel.GetComponent<PanelAnimations>().PlayRefresh();
        if (displayedLevels.Last() >= LAST_LEVEL)
            FillLevelButtons(FIRST_LEVEL);
        else
            FillLevelButtons(displayedLevels.Last() + 1);
    }

    public void LoadPreviousLevelsPage()
    {
        levelsPanel.GetComponent<PanelAnimations>().PlayRefresh();
        if (displayedLevels.First() <= FIRST_LEVEL)
            FillLevelButtons(LAST_LEVEL + FIRST_LEVEL - LEVELS_PER_SITE);
        else
            FillLevelButtons(displayedLevels.First() - LEVELS_PER_SITE);
    }

    public void StartInfinityMode()
    {
        TreeManager.isInfinityMode = true;
        StartGame(true);
    }

    public void SelectCharacter(int characterId)
    {
        for(int i = 0; i < PlayerHelper.CHARACTERS.Count; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }
        var selectedCharacterGameObject = player.transform.GetChild(characterId).gameObject;
        selectedCharacterGameObject.SetActive(true);
        PlayerAnimations.ChangeAnimatorForSelectedCharacter(selectedCharacterGameObject);
        PlayerManager.UpdateSelectedCharacter(selectedCharacterGameObject);

        // feather color setup by changing the particle material
        var characterName = PlayerHelper.CHARACTERS.First(x => x.ID == characterId).Name;
        var featherMaterial = Resources.Load<Material>(PathsDictionary.FEATHER_MATERIALS + characterName);
        var particleRenderer = player.transform.GetChild((int)PlayerChildren.featherEmitter)
            .GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
        particleRenderer.material = featherMaterial;
        particleRenderer.sharedMaterial = featherMaterial;
    }

    public void UpdateScoreAmount(int amount)
    {
        if (scorePanel.GetComponentInChildren<TextMeshProUGUI>() != null)
            scorePanel.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
    }

    public void CloseCornerButton()
    {
        cornerButton.SetActive(false);
    }

    public void LoadCornerButton(string spriteName)
    {
        if (spriteName != null)
        {
            cornerButton.GetComponent<Image>().sprite
                = Resources.Load<Sprite>(PathsDictionary.GetFullPath(PathsDictionary.UI_BUTTONS, spriteName));
        }
        cornerButton.SetActive(true);
    }

    public void CornerButtonOnClick()
    {
        var cornerButtonAction = cornerButton.GetComponent<Image>().sprite.name;

        if (cornerButtonAction == FilenameDictionary.PAUSE_ICON)
        {
            GameManager.PauseGame();
            cornerButton.SetActive(false);
            pausePanel.SetActive(true);
        }
        else if (cornerButtonAction == FilenameDictionary.PLAY_ICON)
        {
            GameManager.ResumeGame();
        }
        else if (cornerButtonAction == FilenameDictionary.SOUND_ON_ICON)
        {
            AudioListener.volume = 0;
            LoadCornerButton(FilenameDictionary.SOUND_OFF_ICON);
        }
        else
        {
            AudioListener.volume = 1;
            LoadCornerButton(FilenameDictionary.SOUND_ON_ICON);
        }
    }

    public void ClosePausePanel()
    {
        pausePanel.GetComponent<PanelAnimations>().PlayClose();
    }

    public void PlayConfettiParticles()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
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
            if (level == LevelsManager.INFINITY_LEVEL)
            {
                levelsButtons[i].transform.GetChild((int)LevelButtonChildren.star).gameObject.SetActive(true);
            }
            else
            {
                levelsButtons[i].transform.GetChild((int)LevelButtonChildren.star).gameObject.SetActive(false);
            }
            displayedLevels[i] = level;
            level++;
        }
        AddListenersTolevelsButtons();
    }
}
