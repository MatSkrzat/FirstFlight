using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
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
        currentLevel = LoadLevel(1);
        TreeModulesManager.currentLevelModules = currentLevel.treeModules;
    }
    #endregion
    

    private void Start()
    {
    }
    public static LevelModel LoadLevel(int levelIdx)
    {
        string levelName =  LEVEL_NAME + levelIdx;
        return SaveLoadFile.LoadFromJson<LevelModel>(PathsDictionary.LEVELS, levelName);
    }
};
