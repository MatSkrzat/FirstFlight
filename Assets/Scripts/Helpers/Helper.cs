using UnityEngine;

public static class Helper
{
    public static char SIDE_LEFT { get; } = 'L';
    public static char SIDE_RIGHT { get; } = 'R';
    public static readonly Vector2 BACKGROUND_INITIALIZE_POSITION = new Vector2(0, -20);
    public static readonly Vector2 BACKGROUND_DESTRUCTION_POSITION = new Vector2(0, 20);
    public static readonly Vector2 BACKGROUND_NEW_INIT_POSITION = new Vector2(0, 4);
    public static readonly Vector2 BACKGROUND_FIRST_INIT_POSITION = new Vector2(0, -5);
}
