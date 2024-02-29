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
        SetUnlockedCharacters();
        SetSelectedCharacter();
        CoinsManager.SetCoins(CurrentGameState.ownedCoins);
    }

    public static void UpdateLastLevel(int level)
    {
        CurrentGameState.lastLevel = level;
    }

    public static void UpdateAndSaveHighScore(int highscore)
    {
        CurrentGameState.highScore = highscore;
        SaveCurrentGameState();
    }

    public static void AddOwnedCharacter(int characterId)
    {
        var characters = CurrentGameState.unlockedCharacters.ToList();
        characters.Add(characterId);
        CurrentGameState.unlockedCharacters = characters.ToArray();
        SaveCurrentGameState();
        SetUnlockedCharacters();
    }

    public static void SetSelectedCharacter(int characterId)
    {
        CurrentGameState.selectedCharacterId = characterId;
        SetSelectedCharacter();
        SaveCurrentGameState();
    }

    public static void SetIsFirstUseAndSave(bool isFirstUse)
    {
        CurrentGameState.isFirstUse = isFirstUse;
        SaveCurrentGameState();
    }

    public static void UpdateLastLevelAndSaveGameState(int level)
    {
        if (PlayerManager.IsDead) return;
        CurrentGameState.lastLevel = level;
        SaveCurrentGameState();
    }

    public static void UpdateOwnedCoins(int amount)
    {
        CurrentGameState.ownedCoins = amount;
    }

    public static void UpdateAndSaveOwnedCoins(int amount)
    {
        UpdateOwnedCoins(amount);
        SaveCurrentGameState();
    }

    public static void LoadGameState()
    {
        try
        {
            CurrentGameState = SaveLoadFile.LoadFromJson<GameStateModel>(PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
        }
        catch
        {
            Debug.Log("Game state not found, creating one");
            GameStateCreator.CreateAndSaveSampleGameState();
            LoadGameState();
        }
    }

    public static void SaveCurrentGameState()
    {
        SaveLoadFile.SaveAsJSON(CurrentGameState, PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
    }

    public static void SaveGameState(GameStateModel gameState)
    {
        SaveLoadFile.SaveAsJSON(gameState, PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
    }

    private static void SetUnlockedCharacters()
    {
        foreach (var unlockedCharacterId in CurrentGameState.unlockedCharacters)
        {
            PlayerHelper.CHARACTERS.Find(x => x.ID == unlockedCharacterId).IsOwned = true;
        }
    }

    private static void SetSelectedCharacter()
    {
        foreach (var character in PlayerHelper.CHARACTERS)
        {
            character.IsSelected = false;
        }
        PlayerHelper.CHARACTERS.Find(x => x.ID == CurrentGameState.selectedCharacterId).IsSelected = true;
    }
}
