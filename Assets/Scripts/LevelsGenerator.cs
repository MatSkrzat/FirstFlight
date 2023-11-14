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
            return GenerateNewLevel(Helper.GAME_SPEED_START, Helper.GAME_SPEED_END, INITIAL_LEVEL, LEVELS_JUMP);
        }
        return GenerateNewLevel(previousLevel.endSpeed, previousLevel.endSpeed + speedJump, previousLevel.ID + LEVELS_JUMP, LEVELS_JUMP);
    }

    public static LevelModel GenerateNewLevel(float startSpeed, float endSpeed, int id = 0, int treeModulesCount = 50) => new LevelModel()
    {
        ID = id,
        treeModules = GenerateRandomTreeModules(treeModulesCount),
        backgroundsPath = PathsDictionary.BACKGROUND_DEFAULT,
        treeModulesPath = PathsDictionary.TREE_MODULES,
        branchesPath = PathsDictionary.TREE_MODULES_BRANCHES,
        endSpeed = endSpeed,
        startSpeed = startSpeed
    };

    public static List<TreeModuleModel> GenerateRandomTreeModules(int modulesToCreate)
    {
        var treeModules = new List<TreeModuleModel>();
        for (int i = 0; i < modulesToCreate; i++)
        {
            treeModules.Add(GenerateNewTreeModule(i, treeModules.Count > 0 ? treeModules[i == 0 ? 0 : i - 1] : null));
        }
        return treeModules;
    }

    public static TreeModuleModel GenerateNewTreeModule(int moduleId, TreeModuleModel previousTreeModule)
    {
        int randomTreeBranchIndex = GetRandomBranchIndex();

        char randomBranchSide = GetRandomBranchSide(previousTreeModule?.branch?.side);

        return new TreeModuleModel
        {
            spriteName = GetRandomTreeModuleName(previousTreeModule?.spriteName),
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

    private static string GetRandomTreeModuleName(string previousName)
    {
        var newTreeModules = new List<string>(FilenameDictionary.DEFAULT_TREE_MODULES_NAMES);
        newTreeModules.Remove(previousName);

        if (newTreeModules.Contains(FilenameDictionary.DEFAULT_TREE_MODULES_NAMES[0]))
        {
            var randomNumber = Random.Range(0, 13);
            if (randomNumber == 0) return newTreeModules[0];
            else if (randomNumber >= 1 && randomNumber <= 4) return newTreeModules[1];
            else if (randomNumber >= 5 && randomNumber <= 8) return newTreeModules[2];
            else if (randomNumber >= 9 && randomNumber <= 12) return newTreeModules[3];
        }

        var random = Random.Range(0, 4);
        return newTreeModules[random];
    }

    private static int GetRandomBranchIndex()
    {
        return Random.Range(0, FilenameDictionary.DEFAULT_BRANCH_NAMES.Length);
    }

    private static char GetRandomBranchSide(char? previousSide)
    {
        if (previousSide == Helper.SIDE_NONE)
        {
            return Random.Range(0, 2) == 0 ? Helper.SIDE_LEFT : Helper.SIDE_RIGHT;
        }

        if (previousSide == Helper.SIDE_LEFT)
        {
            var random = Random.Range(0, 10);
            if (random <= 4) return Helper.SIDE_RIGHT;
            if (random >= 5 && random <= 8) return Helper.SIDE_LEFT;
            return Helper.SIDE_NONE;
        }

        if (previousSide == Helper.SIDE_RIGHT)
        {
            var random = Random.Range(0, 10);
            if (random <= 4) return Helper.SIDE_LEFT;
            if (random >= 5 && random <= 8) return Helper.SIDE_RIGHT;
            return Helper.SIDE_NONE;
        }

        var randomNumber = Random.Range(0, 12);
        if (randomNumber <= 4) return Helper.SIDE_LEFT;
        if (randomNumber >= 5 && randomNumber <= 9) return Helper.SIDE_RIGHT;
        return Helper.SIDE_NONE;
    }
}
