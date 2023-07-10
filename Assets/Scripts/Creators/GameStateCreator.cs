using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCreator : MonoBehaviour
{
    public static GameStateCreator instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool createSampleGameState = false;
    void Start()
    {
        if (createSampleGameState)
        {
            CreateAndSaveSampleGameState();
        }
    }

    public static GameStateModel GetSampleGameState()
    {
        return new GameStateModel()
        {
            finishedLevels = new int[] { 0 },
            unlockedCharacters = new string[] { "Tom" },
            lastLevel = 1,
            ownedCoins = 0
        };
    }

    public static bool  CreateAndSaveSampleGameState()
    {
        Debug.Log("** Creating sample game state");
        SaveLoadFile.SaveAsJSON(GetSampleGameState(), PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
        return true;
    }


}
