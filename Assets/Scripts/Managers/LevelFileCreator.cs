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
        treeModules.Add(GetSampleTreeModule(0));
        treeModules.Add(GetSampleTreeModule(1));
        treeModules.Add(GetSampleTreeModule(2));
        treeModules.Add(GetSampleTreeModule(3));
        treeModules.Add(GetSampleTreeModule(4));
        treeModules.Add(GetSampleTreeModule(5));
        treeModules.Add(GetSampleTreeModule(6));
        treeModules.Add(GetSampleTreeModule(7));

        return treeModules;
    }

    private TreeModuleModel GetSampleTreeModule(int moduleId)
    {
        int treeModuleLastIndex = FilenameDictionary.DEFAULT_TREE_MODULES_NAMES.Length;
        int randomTreeModuleIndex = Random.Range(
                0,
                treeModuleLastIndex
            );
        return new TreeModuleModel
        {
            spriteName = FilenameDictionary.DEFAULT_TREE_MODULES_NAMES[randomTreeModuleIndex],
            moduleID = moduleId,
            branch = new BranchModel
            {
                spriteName = FilenameDictionary.DEFAULT_BROKEN_BRANCH_NAMES[0],
                side = 'L'
            }
        };
    }
}
