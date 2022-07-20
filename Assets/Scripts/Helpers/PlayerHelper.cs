using UnityEngine;

public static class PlayerHelper
{
    public static readonly float RIGHT_X_POSITION = 2F;
    public static readonly float LEFT_X_POSITION = -2F;
    public static readonly float GRAVITY_SCALE_IDLE = 0F;
    public static readonly float GRAVITY_SCALE_JUMP = 100F;
    public static readonly float GRAVITY_SCALE_FALL = 10F;
    public static readonly Vector2 INITIAL_POSITION = new Vector2(2F, 0F);
    public static readonly float PLAYER_Z_ROTATION = -25F;
    public static readonly Vector2 JUMP_FORCE = new Vector2(-85F, 55F);
    public static readonly Vector2 DEATH_FORCE = new Vector2(-10F, 10F);
    public static readonly float DEATH_TORQUE = 2000F;
    public static readonly int INITIAL_NUMBER_OF_LIVES = 3;
    public static readonly Vector2 PLAYER_DEATH_HIDE_POSITION = new Vector2(0F, -20F);
}
