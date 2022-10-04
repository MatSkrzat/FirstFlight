using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject jumpSecurityPanel;
    public GameObject mainMenuPanel;
    public GameObject healthStatusPanel;

    private List<GameObject> heartsGameObjects = new List<GameObject>();
    private readonly List<string> HEARTS_NAMES = new List<string>() { "heart_0", "heart_1", "heart_2" };
    private Sprite grayHeartSprite;

    private void Start()
    {
        grayHeartSprite = Resources.Load<Sprite>(PathsDictionary.UI_HEARTS + FilenameDictionary.UI_HEART_GRAY);

        foreach (Transform child in healthStatusPanel.transform)
        {
            if(HEARTS_NAMES.Contains(child.gameObject.name))
                heartsGameObjects.Add(child.gameObject);
        }
    }

    public bool IsSecurityPanelClicked() => 
        EventSystem.current.currentSelectedGameObject == jumpSecurityPanel;
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        healthStatusPanel.SetActive(true);
        GameManager.StartGame();
    }

    public void UpdateDisplayedHealth(int numberOfLives)
    {
        for (int i = heartsGameObjects.Count; i > numberOfLives; i--)
        {
            heartsGameObjects[i - 1].GetComponent<Image>().overrideSprite = grayHeartSprite;
        }
    }
}
