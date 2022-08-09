using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject JumpSecurityPanel;

    public bool IsSecurityPanelClicked() => 
        EventSystem.current.currentSelectedGameObject == JumpSecurityPanel;
}
