public static class PathsDictionary
{
    #region PUBLIC
    public static string BACKGROUND_DEFAULT { get; } = "Textures/Backgrounds/Default/";
    public static string FEATHER_MATERIALS { get; } = "Materials/FeatherMaterials/";
    public static string LEVELS { get; } = "Levels/";
    public static string PREFABS { get; } = "Prefabs/";
    public static string GAME_STATE { get; } = "GameState/";
    public static string TREE_MODULES { get; } = "Textures/TreeModules/Default/Roots/";
    public static string TREE_MODULES_BRANCHES { get; } = "Textures/TreeModules/Default/Branches/";
    public static string UI_HEARTS { get; } = "Textures/UI/Hearts/";
    public static string UI_BUTTONS { get; } = "Textures/UI/Buttons/";
    #endregion
    #region PRIVATE
    private static string PLAYER { get; } = "Textures/Player/{name}/";
    #endregion

    public static string GetFullPath(string path, string fileName) => string.Format("{0}{1}", path, fileName);
    public static string GetPlayerPath(string playerName) => PLAYER.Replace("{name}", playerName);
}
