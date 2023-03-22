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

    public static void AddCoins(int amount = 1)
    {
        ownedCoins += amount;
        GameManager.UI.UpdateCoinsAmount(ownedCoins);
    }

    public static void SetCoins(int amount)
    {
        ownedCoins = amount;
        GameManager.UI.UpdateCoinsAmount(ownedCoins);
    }
}
