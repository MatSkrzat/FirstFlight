using System;
using System.Collections.Generic;

[Serializable]
public class GameStateModel
{
    public int[] finishedLevels;
    public int[] unlockedCharacters;
    public int lastLevel;
    public int ownedCoins;
}
