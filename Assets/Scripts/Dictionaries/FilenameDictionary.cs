public static class FilenameDictionary
{
    #region backgrounds
    public static string[] BACKGROUND_DEFAULT_SPRITES_NAMES { get; } = new string[] { "sky1", "sky2" };
    public static string BACKGROUND_PREFAB { get; } = "Background";
    #endregion
    #region levels
    public static string LEVEL { get; } = "level";
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
    #region player
    public static string BODY { get; } = "body";
    public static string BODY_CLOSED_BEAK { get; } = "body_closed_beak";
    public static string BODY_DAMAGED { get; } = "body_damaged";
    public static string WING { get; } = "wing";
    public static string OPENED_EYE { get; } = "opened_eye";
    public static string BLACK_EYE { get; } = "black_eye";
    public static string CLOSED_EYE { get; } = "closed_eye";
    public static string MINIATURE { get; } = "miniature";
    public static string FEATHER { get; } = "feather";
    #endregion
    #region game_states
    public static string GAME_STATE { get; } = "gameState";
    #endregion
    #region ui
    public static string UI_HEART_RED { get; } = "heart_red";
    public static string UI_HEART_GRAY { get; } = "heart_gray";
    public static string UI_HEART_PREFAB { get; } = "heart";
    #endregion
}
