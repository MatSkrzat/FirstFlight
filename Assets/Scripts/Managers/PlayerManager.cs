using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region STATIC
    public static PlayerManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static int NumberOfLives { get; private set; } = PlayerHelper.INITIAL_NUMBER_OF_LIVES;
    public static bool IsDead { get; private set; } = false;
    public static char PositionSide { get; set; } = Helper.SIDE_RIGHT;
    public static bool IsJumping { get; set; } = false;

    public static void SubstractLives(int livesToSubstract)
    {
        if (IsDead) return;

        NumberOfLives -= livesToSubstract;
        if (NumberOfLives <= 0)
        {
            PlayerAnimations.PlayDeathAnimation();
            IsDead = true;
        }
    }
    public static void ResetLives()
    {
        NumberOfLives = PlayerHelper.INITIAL_NUMBER_OF_LIVES;
    }

    public static void SetValuesToDefault()
    {
        NumberOfLives = PlayerHelper.INITIAL_NUMBER_OF_LIVES;
        IsDead = false;
        PositionSide = Helper.SIDE_RIGHT;
        IsJumping = false;
    }
}
