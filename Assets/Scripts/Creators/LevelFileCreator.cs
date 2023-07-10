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
            GenerateMultipleLevels(1);
        }
        if(createLevels && !createLevel)
        {
            GenerateMultipleLevels(Helper.LEVELS_COUNT);
        }
    }

    public void GenerateMultipleLevels(int levelsAmount)
    {
        for(int i = 0; i < levelsAmount; i++)
        {
            int levelId = i + 1;
            LevelModel level = new LevelModel()
            {
                ID = levelId,
                treeModules = GetSampleTreeModules(50 + i),
                backgroundsPath = PathsDictionary.BACKGROUND_DEFAULT,
                treeModulesPath = PathsDictionary.TREE_MODULES,
                branchesPath = PathsDictionary.TREE_MODULES_BRANCHES,
                endSpeed = Helper.GAME_SPEED_START + (Helper.GAME_SPEED_END / Helper.LEVELS_COUNT * (i + 1)),
                startSpeed = Helper.GAME_SPEED_START + (Helper.GAME_SPEED_END / Helper.LEVELS_COUNT * i)
            };
            var levelName = FilenameDictionary.LEVEL + levelId;
            SaveLoadFile.SaveAsJSON(level, PathsDictionary.LEVELS, levelName, true);
            Debug.Log($"** Level {levelId} created!");
        }
    }

    private List<TreeModuleModel> GetSampleTreeModules(int modulesToCreate)
    {
        var treeModules = new List<TreeModuleModel>();
        for (int i = 0; i < modulesToCreate; i++)
        {
            treeModules.Add(GetSampleTreeModule(i));
        }
        return treeModules;
    }

    private TreeModuleModel GetSampleTreeModule(int moduleId)
    {
        int randomTreeModuleIndex = Random.Range(
            0,
            FilenameDictionary.DEFAULT_TREE_MODULES_NAMES.Length
        );
        int randomTreeBranchIndex = Random.Range(
            0,
            FilenameDictionary.DEFAULT_BRANCH_NAMES.Length
        );
        char randomBranchSide = Random.Range(0, 2) == 0 ? Helper.SIDE_LEFT : Helper.SIDE_RIGHT;
        return new TreeModuleModel
        {
            spriteName = FilenameDictionary.DEFAULT_TREE_MODULES_NAMES[randomTreeModuleIndex],
            moduleID = moduleId,
            hasBonus = Random.Range(0, 10) == 0,
            branch = new BranchModel
            {
                spriteName = FilenameDictionary.DEFAULT_BRANCH_NAMES[randomTreeBranchIndex].BranchName,
                brokenBranchSpriteName = FilenameDictionary.DEFAULT_BRANCH_NAMES[randomTreeBranchIndex].BrokenBranchName,
                side = randomBranchSide
            }
        };
    }
}
