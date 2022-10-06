using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject jumpSecurityPanel;
    public GameObject mainMenuPanel;
    public GameObject healthStatusPanel;
    public GameObject scorePanel;
    public GameObject coinsAmountText;

    private List<GameObject> heartGameObjects = new List<GameObject>();
    private Sprite grayHeartSprite;
    private GameObject heartPrefab;
    private const float HEART_GAMEOBJECT_SEPARATION = 120F;

    private void Start()
    {
        LoadHearts();
    }

    private void LoadHearts()
    {
        grayHeartSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.UI_HEARTS, FilenameDictionary.UI_HEART_GRAY));
        heartPrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.UI_HEART_PREFAB));

        for(int i = 0; i < PlayerManager.NumberOfLives; i++)
        {
            var heartGameObject = Instantiate(heartPrefab);
            heartGameObject.transform.SetParent(healthStatusPanel.transform);

            if (heartGameObjects.Count > 0)
                heartGameObject.transform.localPosition = new Vector3(heartGameObjects.Last().transform.localPosition.x + HEART_GAMEOBJECT_SEPARATION,
                    heartGameObject.transform.position.y,
                    heartGameObject.transform.position.z);
            else
                heartGameObject.transform.localPosition = new Vector3(heartGameObject.transform.position.x,
                    heartGameObject.transform.position.y,
                    heartGameObject.transform.position.z);

            heartGameObjects.Add(heartGameObject);

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
        for (int i = heartGameObjects.Count; i > numberOfLives; i--)
        {
            var heartGameObjectSprite = heartGameObjects[i - 1].GetComponent<Image>();

            if (heartGameObjectSprite.overrideSprite != grayHeartSprite)
            {
                heartGameObjectSprite.gameObject.GetComponent<HeartAnimations>().PlayChange();
                heartGameObjectSprite.overrideSprite = grayHeartSprite;
            }
        }
    }

    public void UpdateCoinsAmount(int coinsAmount)
    {
        //if(coinsAmountText.GetComponent<TextMeshPro>() != null)
        //    coinsAmountText.GetComponent<TextMeshPro>().text = coinsAmount.ToString();
    }
}
