public static class PathsDictionary
{
    public static string BACKGROUND_DEFAULT { get; } = "Textures/Backgrounds/Default/";
    public static string LEVELS { get; } = "Levels/";
    public static string PREFABS { get; } = "Prefabs/";
    public static string TREE_MODULES { get; } = "Textures/TreeModules/Default/Roots/";
    public static string TREE_MODULES_BRANCHES { get; } = "Textures/TreeModules/Default/Branches/";

    public static string CreateFullPath(string path, string fileName) => string.Format("{0}{1}", path, fileName);
}
