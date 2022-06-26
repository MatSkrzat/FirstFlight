using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    public const int INITIAL_LEVEL = 0;
    public const string LEVEL_NAME = "level";
    public static LevelModel currentLevel = new LevelModel();

    public GameObject UIDebug;

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
        UIDebug.GetComponent<Text>().text = string.Format("{0}/{1}{2}.json", Application.streamingAssetsPath, PathsDictionary.LEVELS, LEVEL_NAME + INITIAL_LEVEL).Replace(" ", "_");
    }
    public static LevelModel LoadLevel(int levelID)
    {
        string levelName = LEVEL_NAME + levelID;

        return SaveLoadFile.LoadFromJson<LevelModel>(PathsDictionary.LEVELS, levelName);
    }
};
