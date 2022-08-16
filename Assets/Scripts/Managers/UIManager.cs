using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject JumpSecurityPanel;
    public GameObject MainMenuPanel;

    public bool IsSecurityPanelClicked() => 
        EventSystem.current.currentSelectedGameObject == JumpSecurityPanel;
    public void StartGame()
    {
        MainMenuPanel.SetActive(false);
        GameManager.StartGame();
    }
}
