using System.Collections.Generic;
using UnityEngine;

public class LevelFileCreator : MonoBehaviour
{
    public bool createLevel = true;
    public bool createLevels = true;
    void Start()
    {
        if (createLevel)
        {
            GenerateAndSaveMultipleLevels(1);
        }
        if(createLevels && !createLevel)
        {
            GenerateAndSaveMultipleLevels(Helper.LEVELS_COUNT);
        }
    }

    public void GenerateAndSaveMultipleLevels(int levelsAmount)
    {
        for (int i = 0; i < levelsAmount; i++)
        {
            int levelId = i + 1;
            float startSpeed = Helper.GAME_SPEED_START + (Helper.GAME_SPEED_END / Helper.LEVELS_COUNT * i);
            float endSpeed = Helper.GAME_SPEED_START + (Helper.GAME_SPEED_END / Helper.LEVELS_COUNT * (i + 1));
            LevelModel level = LevelsGenerator.GenerateNewLevel(startSpeed, endSpeed, levelId, 50 + i);
            var levelName = FilenameDictionary.LEVEL + levelId;
            SaveLoadFile.SaveAsJSON(level, PathsDictionary.LEVELS, levelName, true);
            Debug.Log($"** Level {levelId} created!");
        }
    }
}
