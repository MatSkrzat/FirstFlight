using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCreator : MonoBehaviour
{
    public bool createSampleGameState = false;
    void Start()
    {
        if (createSampleGameState)
        {
            Debug.Log("** Creating sample game state");
            SaveLoadFile.SaveAsJSON(CreateSampleGameState(), PathsDictionary.GAME_STATE, FilenameDictionary.GAME_STATE);
        }
    }

    private GameStateModel CreateSampleGameState()
    {
        return new()
        {
            finishedLevels = new int[] { 0 },
            unlockedCharacters = new string[] { "Tom" },
            lastLevel = 1
        };
    }
}
