using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    #region STATIC
    public static CoinsManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public static int ownedCoins { get; private set; }

    public static void AddCoins(int amount = 0)
    {
        ownedCoins += amount;
        GameManager.UI.UpdateCoinsAmount(ownedCoins);
        GameStateManager.UpdateOwnedCoins(ownedCoins);
    }

    public static void SubstractCoins(int amount = 0)
    {
        ownedCoins -= amount;
        GameStateManager.UpdateAndSaveOwnedCoins(ownedCoins);
        GameManager.UI.UpdateCoinsAmount(ownedCoins);
    }

    public static void SetCoins(int amount)
    {
        ownedCoins = amount;
        if(GameManager.UI != null)
            GameManager.UI.UpdateCoinsAmount(ownedCoins);
    }
}
