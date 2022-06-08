using System;
using System.Collections.Generic;

[Serializable]
public class LevelModel
{
    public int ID;
    public List<TreeModuleModel> treeModules;
    public string backgroundsPath;
    public string treeModulesPath;
    public string branchesPath;
    public float startSpeed;
    public float endSpeed;
}
