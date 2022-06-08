public static class FilenameDictionary
{
    #region backgrounds
    public static string[] BACKGROUND_DEFAULT_SPRITES_NAMES { get; } = new string[] { "sky1", "sky2" };
    public static string BACKGROUND_PREFAB { get; } = "Background";
    #endregion
    #region levels
    public static string LEVEL1 { get; } = "level1";
    public static string LEVELS_TEMPLATE { get; } = "levelsTemplate";
    #endregion
    #region tree_modules
    public static string[] DEFAULT_TREE_MODULES_NAMES { get; } = new string[] { "tree1", "tree2", "tree3", "tree4", "tree5" };
    public static string TREE_PREFAB { get; } = "TreeModule";
    #endregion
    #region branches
    public static string[] DEFAULT_BRANCH_NAMES { get; } = new string[] { "branch1", "branch2", "branch3" };
    public static string BRANCH_PREFAB { get; } = "Branch";
    public static string[] DEFAULT_BROKEN_BRANCH_NAMES { get; } = new string[] { "broken_branch1", "broken_branch2" };
    public static string BROKEN_BRANCH_PREFAB { get; } = "BrokenBranch";
    #endregion

}
