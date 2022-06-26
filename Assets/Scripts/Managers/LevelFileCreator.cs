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
        treeModules.Add(GetSampleTreeModule(8));
        treeModules.Add(GetSampleTreeModule(9));
        treeModules.Add(GetSampleTreeModule(10));
        treeModules.Add(GetSampleTreeModule(11));
        treeModules.Add(GetSampleTreeModule(12));
        treeModules.Add(GetSampleTreeModule(13));
        treeModules.Add(GetSampleTreeModule(14));
        treeModules.Add(GetSampleTreeModule(15));
        treeModules.Add(GetSampleTreeModule(16));
        treeModules.Add(GetSampleTreeModule(17));
        treeModules.Add(GetSampleTreeModule(18));
        treeModules.Add(GetSampleTreeModule(19));
        treeModules.Add(GetSampleTreeModule(20));
        treeModules.Add(GetSampleTreeModule(21));
        treeModules.Add(GetSampleTreeModule(22));
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
        char randomBranchSide = Random.Range(0, 2) == 0 ? 'L' : 'P';
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
