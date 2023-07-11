using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static void UnlockCharacter(int characterId)
    {
        var character = PlayerHelper.CHARACTERS.Find(x => x.ID == characterId);
        if (CoinsManager.ownedCoins >= character.Price)
        {
            CoinsManager.SubstractCoins(character.Price);
            GameStateManager.AddOwnedCharacter(characterId);
        }
    }
}
