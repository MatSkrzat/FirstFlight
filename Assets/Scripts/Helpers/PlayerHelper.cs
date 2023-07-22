using System.Collections.Generic;
using UnityEngine;

public enum CharacterIds
{
    tom,
    zoe,
    philip,
    arthur,
    sylvie,
    jack,
    pinguin
};
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
    public static List<CharacterModel> CHARACTERS = new List<CharacterModel>()
    {
        new CharacterModel()
        {
            Name = "Tom",
            ID = (int)CharacterIds.tom,
            Price = 0,
        },
        new CharacterModel()
        {
            Name = "Zoe",
            ID = (int)CharacterIds.zoe,
            Price = 250,
        },
        new CharacterModel()
        {
            Name = "Philip",
            ID = (int)CharacterIds.philip,
            Price = 250,
        },
        new CharacterModel()
        {
            Name = "Arthur",
            ID = (int)CharacterIds.arthur,
            Price = 500,
        },
        new CharacterModel()
        {
            Name = "Sylvie",
            ID = (int)CharacterIds.sylvie,
            Price = 500,
        },
        new CharacterModel()
        {
            Name = "Jack",
            ID = (int)CharacterIds.jack,
            Price = 1000,
        },
        new CharacterModel()
        {
            Name = "Mr. Pinguin",
            ID = (int)CharacterIds.pinguin,
            Price = 1000,
        },
    };
}
