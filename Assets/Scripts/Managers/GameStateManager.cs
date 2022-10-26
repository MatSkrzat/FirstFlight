using System.Linq;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public static GameStateModel CurrentGameState { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        LoadGameState();
    }

    public static void UpdateLastLevel(int level)
    {
        CurrentGameState.lastLevel = level;
    }

    public static void UpdateFinishedLevels(int lastLevelId)
    {
        var finishedLevels = CurrentGameState.finishedLevels.ToList();
        finishedLevels.Add(lastLevelId - 1);
        CurrentGameState.finishedLevels = finishedLevels.ToArray();
    }

    public static void UnlockCharacter(string characterName)
    {
        var characters = CurrentGameState.unlockedCharacters.ToList();
        characters.Add(characterName);
        CurrentGameState.unlockedCharacters = characters.ToArray();
    }

    public static void UpdateOwnedCoins(int amount)
    {
        CurrentGameState.ownedCoins = amount;
    }

    public static void LoadGameState()
    {
        CurrentGameState = SaveLoadFile.LoadFromJson<GameStateModel>(PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
    }

    public static void SaveCurrentGameState()
    {
        SaveLoadFile.SaveAsJSON(CurrentGameState, PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
    }

    public static void SaveGameState(GameStateModel gameState)
    {
        SaveLoadFile.SaveAsJSON(gameState, PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
    }
}
