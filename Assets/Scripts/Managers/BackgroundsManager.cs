using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundsManager : MonoBehaviour
{
    #region STATIC
    public static BackgroundsManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static List<GameObject> backgroundPrefabsPool = new List<GameObject>();
    public static GameObject backgroundPrefab;
    public static readonly Vector2 INITIALIZE_POSITION = new Vector2(0, -20);
    public static readonly Vector2 DESTRUCTION_POSITION = new Vector2(0, 20);
    public static readonly Vector2 NEW_BACKGROUND_INIT_POSITION = new Vector2(0, 4);

    public void Start()
    {
        backgroundPrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.BACKGROUND_PREFAB));
        InitializeNewBackground();
    }
    public static void ManageBackgrounds()
    {
        InitializeNewBackground();
        DestroyOldBackgrounds();
    }
    private static void InitializeNewBackground()
    {
        var newBackground = Instantiate(backgroundPrefab, INITIALIZE_POSITION, Quaternion.identity);
        var backgroundSpriteRenderer = newBackground.GetComponent<SpriteRenderer>();

        backgroundSpriteRenderer.sprite = LoadSprite(LevelsManager.currentLevel.backgroundsPath);
        backgroundSpriteRenderer.flipX = Random.value > 0.5F;

        var backgroundBehaviour = newBackground.GetComponent<BackgroundBehaviour>();
        backgroundBehaviour.shouldMove = true;
        backgroundBehaviour.SetSpeed(LevelsManager.currentLevel.endSpeed * 0.5F);

        backgroundPrefabsPool.Add(newBackground);
    }

    private static Sprite LoadSprite(string backgroundsPath) => Resources.Load<Sprite>(
            backgroundsPath +
            FilenameDictionary.BACKGROUND_DEFAULT_SPRITES_NAMES[
                Random.Range(0, FilenameDictionary.BACKGROUND_DEFAULT_SPRITES_NAMES.Length)
            ]
        );

    public static void DestroyOldBackgrounds()
    {
        var oldBackgrounds = backgroundPrefabsPool.Where(item => item.transform.position.y > DESTRUCTION_POSITION.y);
        oldBackgrounds.ToList().ForEach(item => Destroy(item));
        backgroundPrefabsPool = backgroundPrefabsPool.Except(oldBackgrounds).ToList();
    }
}
