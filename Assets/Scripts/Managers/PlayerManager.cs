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
    private const int INITIAL_NUMBER_OF_LIVES = 3;
    public static int NumberOfLives { get; private set; } = INITIAL_NUMBER_OF_LIVES;
    public static bool IsDead { get; private set; } = false;

    public static void SubstractLives(int livesToSubstract)
    {
        NumberOfLives -= livesToSubstract;
        if (NumberOfLives <= 0)
        {
            IsDead = true;
        }
    }
    public static void ResetLives()
    {
        NumberOfLives = INITIAL_NUMBER_OF_LIVES;
    }


}
