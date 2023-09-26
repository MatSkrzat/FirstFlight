using System;
using System.Collections.Generic;

[Serializable]
public class GameStateModel
{
    public int[] unlockedCharacters;
    public int lastLevel;
    public int ownedCoins;
    public int selectedCharacterId;
    public int infinityUnblockLevel;
}
