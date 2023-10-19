using UnityEngine;

public static class Helper
{
    public static char SIDE_LEFT { get; } = 'L';
    public static char SIDE_RIGHT { get; } = 'R';
    public static readonly Vector2 BACKGROUND_INITIALIZE_POSITION = new Vector2(0, -20);
    public static readonly Vector2 BACKGROUND_DESTRUCTION_POSITION = new Vector2(0, 20);
    public static readonly Vector2 BACKGROUND_NEW_INIT_POSITION = new Vector2(0, 4);
    public static readonly Vector2 BACKGROUND_FIRST_INIT_POSITION = new Vector2(0, -5);
    public static readonly string GO_NAME_INITIAL_TREE = "InitialTree";
    public static readonly string HIGHSCORE_LABEL = "New Highscore!";
    public static readonly string SCORE_LABEL = "Score";
    public static readonly int LEVELS_COUNT = 45;
    public static readonly float GAME_SPEED_START = 5F;
    public static readonly float GAME_SPEED_END = 7F;
}
