using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsGenerator : MonoBehaviour
{
    #region STATIC
    public static LevelsGenerator instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static LevelModel GenerateRandomLevel(LevelModel previousLevel, float speedJump = 0.5F)
    {
        const int LEVELS_JUMP = 25;
        const int INITIAL_LEVEL = 0;
        if (previousLevel == default)
        {
            return GenereteNewLevel(Helper.GAME_SPEED_START, Helper.GAME_SPEED_END, INITIAL_LEVEL, LEVELS_JUMP);
        }
        return GenereteNewLevel(previousLevel.endSpeed, previousLevel.endSpeed + speedJump, previousLevel.ID + LEVELS_JUMP, LEVELS_JUMP);
    }

    public static LevelModel GenereteNewLevel(float startSpeed, float endSpeed, int id = 0, int treeModulesCount = 50) => new LevelModel()
    {
        ID = id,
        treeModules = GetSampleTreeModules(treeModulesCount),
        backgroundsPath = PathsDictionary.BACKGROUND_DEFAULT,
        treeModulesPath = PathsDictionary.TREE_MODULES,
        branchesPath = PathsDictionary.TREE_MODULES_BRANCHES,
        endSpeed = endSpeed,
        startSpeed = startSpeed
    };

    public static List<TreeModuleModel> GetSampleTreeModules(int modulesToCreate)
    {
        var treeModules = new List<TreeModuleModel>();
        for (int i = 0; i < modulesToCreate; i++)
        {
            treeModules.Add(GetSampleTreeModule(i));
        }
        return treeModules;
    }

    public static TreeModuleModel GetSampleTreeModule(int moduleId)
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