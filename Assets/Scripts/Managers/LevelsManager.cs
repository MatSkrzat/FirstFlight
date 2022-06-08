using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public const int INITIAL_LEVEL = 0;
    public const string LEVEL_NAME = "level";
    public static LevelModel currentLevel = new LevelModel();

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
        TreeModulesManager.currentLevelModules = currentLevel.treeModules;
    }
    #endregion


    private void Start()
    {
    }
    public static LevelModel LoadLevel(int levelID)
    {
        string levelName = LEVEL_NAME + levelID;
        return SaveLoadFile.LoadFromJson<LevelModel>(PathsDictionary.LEVELS, levelName);
    }
};
