using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public const int INITIAL_LEVEL = 1;
    public static LevelModel currentLevel = new LevelModel();
    public static LevelModel nextLevel = new LevelModel();

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

        return SaveLoadFile.LoadFromJson<LevelModel>(PathsDictionary.LEVELS, levelName);
    }

    public static void LoadAndSetLevel(int levelID)
    {
        currentLevel = LoadLevel(levelID);
    }

    public static LevelModel LoadNextLevel()
    {
        Debug.Log("**Loading new level " + (currentLevel.ID + 1));
        nextLevel = LoadLevel(currentLevel.ID + 1);
        return nextLevel;
    }

    public static void SwitchToNextLevel()
    {
        Debug.Log("**Switching levels to next level with ID: " + nextLevel.ID);
        currentLevel = nextLevel;
        nextLevel = new LevelModel();
        Debug.Log("**Levels switched new level ID: " + currentLevel.ID);
    }

    public static void SetValuesToDefault()
    {
        currentLevel = new LevelModel();
        nextLevel = new LevelModel();
    }
}