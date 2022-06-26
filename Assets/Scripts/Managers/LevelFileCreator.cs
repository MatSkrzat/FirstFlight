using System.Collections.Generic;
using UnityEngine;

public class LevelFileCreator : MonoBehaviour
{
    public bool createLevel = true;
    public void Start()
    {
        if (createLevel)
        {
            LevelModel level = new LevelModel()
            {
                ID = 0,
                treeModules = GetSampleTreeModules(),
                backgroundsPath = PathsDictionary.BACKGROUND_DEFAULT,
                treeModulesPath = PathsDictionary.TREE_MODULES,
                branchesPath = PathsDictionary.TREE_MODULES_BRANCHES,
                endSpeed = 10f,
                startSpeed = 5f
            };
            SaveLoadFile.SaveAsJSON(level, PathsDictionary.LEVELS, FilenameDictionary.LEVELS_TEMPLATE);
            SaveLoadFile.SaveAsJSON(level, PathsDictionary.LEVELS, FilenameDictionary.LEVEL0);
            Debug.Log("** Level template created!");
        }
    }

    private List<TreeModuleModel> GetSampleTreeModules()
    {
        var treeModules = new List<TreeModuleModel>();
        var numberOfTreeModulesToCreate = 100;
        for (int i = 0; i < numberOfTreeModulesToCreate; i++)
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
        char randomBranchSide = Random.Range(0, 2) == 0 ? BranchHelper.LEFT : BranchHelper.RIGHT;
        return new TreeModuleModel
        {
            spriteName = FilenameDictionary.DEFAULT_TREE_MODULES_NAMES[randomTreeModuleIndex],
            moduleID = moduleId,
            branch = new BranchModel
            {
                spriteName = FilenameDictionary.DEFAULT_BRANCH_NAMES[randomTreeBranchIndex],
                side = randomBranchSide
            }
        };
    }
}
