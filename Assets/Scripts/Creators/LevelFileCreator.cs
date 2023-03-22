using System.Collections.Generic;
using UnityEngine;

public class LevelFileCreator : MonoBehaviour
{
    public bool createLevel = true;
    const int LEVEL_ID = 1;
    public void Start()
    {
        if (createLevel)
        {
            LevelModel level = new LevelModel()
            {
                ID = LEVEL_ID,
                treeModules = GetSampleTreeModules(),
                backgroundsPath = PathsDictionary.BACKGROUND_DEFAULT,
                treeModulesPath = PathsDictionary.TREE_MODULES,
                branchesPath = PathsDictionary.TREE_MODULES_BRANCHES,
                endSpeed = 10f,
                startSpeed = 5f
            };
            var levelName = FilenameDictionary.LEVEL + LEVEL_ID;
            SaveLoadFile.SaveAsJSON(level, PathsDictionary.LEVELS, levelName);
            Debug.Log("** Level template created!");
        }
    }

    private List<TreeModuleModel> GetSampleTreeModules()
    {
        var treeModules = new List<TreeModuleModel>();
        const int NUMBER_OF_TREE_MODULES_TO_CREATE = 50;
        for (int i = 0; i < NUMBER_OF_TREE_MODULES_TO_CREATE; i++)
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
