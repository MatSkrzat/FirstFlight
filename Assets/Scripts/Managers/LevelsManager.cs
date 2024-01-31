using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public const int INITIAL_LEVEL = 1;
    public const int INFINITY_LEVEL = 27;
    public static LevelModel currentLevel = new LevelModel();
    public static LevelModel nextLevel = new LevelModel();
    public static bool isNextLevelReady = false;

    #region STATIC
    public static LevelsManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //TODO: Create a better way to load all resources and levels
        currentLevel = LoadLevel(INITIAL_LEVEL);
        TreeManager.currentLevelModules = currentLevel.treeModules;
    }
    #endregion

    public static LevelModel LoadLevel(int levelID)
    {
        string levelName = FilenameDictionary.LEVEL + levelID;

        return SaveLoadFile.LoadFromJson<LevelModel>(PathsDictionary.LEVELS, levelName, true);
    }

    public static void LoadAndSetLevel(int levelID)
    {
        currentLevel = LoadLevel(levelID);
    }

    public static LevelModel LoadNextLevel()
    {
        if(currentLevel.ID >= Helper.LEVELS_COUNT)
        {
            LoadEndGameLevel();
            return nextLevel;
        }
        Debug.Log("**Loading new level " + (currentLevel.ID + 1));
        nextLevel = LoadLevel(currentLevel.ID + 1);
        isNextLevelReady = true;
        return nextLevel;
    }

    public static void SwitchToNextLevel()
    {
        Debug.Log("**Switching levels to next level with ID: " + nextLevel.ID);
        currentLevel = nextLevel;
        nextLevel = new LevelModel();
        isNextLevelReady = false;
        Debug.Log("**Levels switched new level ID: " + currentLevel.ID);
        if (GameStateManager.CurrentGameState.lastLevel < currentLevel.ID)
        {
            GameStateManager.UpdateLastLevelAndSaveGameState(currentLevel.ID);
        }
    }

    public static void SwitchToNextRandomLevel()
    {
        currentLevel = nextLevel;
        nextLevel = new LevelModel();
        isNextLevelReady = false;
    }

    public static void LoadRandomLevel()
    {
        Debug.Log("**Loading new random level");
        currentLevel = LevelsGenerator.GenerateRandomLevel(currentLevel);
    }

    public static void LoadNextRandomLevel()
    {
        Debug.Log("**Loading next random level");
        nextLevel = LevelsGenerator.GenerateRandomLevel(currentLevel);
    }

    private static void LoadEndGameLevel()
    {
        nextLevel = LevelsGenerator.GenerateEndGameLevel();
    }

    public static void SetValuesToDefault()
    {
        currentLevel = new LevelModel();
        nextLevel = new LevelModel();
    }
}