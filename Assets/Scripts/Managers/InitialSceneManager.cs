using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialSceneManager : MonoBehaviour
{
    public static InitialSceneManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public GameObject hidingPanel;
    public GameObject skipButton;

    private void Start()
    {
        if (GameStateManager.CurrentGameState.isFirstUse)
        {
            skipButton.SetActive(false);
        }
    }

    public void LoadHidingPanel()
    {
        hidingPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        GameStateManager.SetIsFirstUseAndSave(false);
    }
}
