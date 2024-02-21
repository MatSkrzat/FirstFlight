using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("InitialScene");
    }
}
