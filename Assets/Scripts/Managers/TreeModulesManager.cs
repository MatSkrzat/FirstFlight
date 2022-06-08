using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeModulesManager : MonoBehaviour
{
    #region STATIC
    public static TreeModulesManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static List<GameObject> treeModulesPrefabsPool = new List<GameObject>();
    public static List<TreeModuleModel> currentLevelModules = new List<TreeModuleModel>();
    public static GameObject treeModulePrefab;
    public static readonly Vector2 INITIALIZE_POSITION = new Vector2(0, -10);
    public static readonly Vector2 DESTRUCTION_POSITION = new Vector2(0, 10);
    public static readonly Vector2 NEW_TREE_MODULE_INIT_POSITION = new Vector2(0, -7);

    public void Start()
    {
        treeModulePrefab = Resources.Load<GameObject>(
            PathsDictionary.CreateFullPath(PathsDictionary.PREFABS, FilenameDictionary.TREE_PREFAB));
        InitializeNewTreeModule();
    }
    public static void ManageTreeModules()
    {
        InitializeNewTreeModule();
        DestroyOldTreeModules();
    }
    private static void InitializeNewTreeModule()
    {
        var newTreeModule = Instantiate(treeModulePrefab, INITIALIZE_POSITION, Quaternion.identity);
        var treeModuleSpriteRenderer = newTreeModule.GetComponent<SpriteRenderer>();

        treeModuleSpriteRenderer.sprite = LoadSprite(LevelsManager.currentLevel.treeModulesPath);
        treeModuleSpriteRenderer.flipX = Random.value > 0.5F;

        var treeBehaviour = newTreeModule.GetComponent<TreeBehaviour>();
        treeBehaviour.shouldMove = true;
        treeBehaviour.ChangeSpeed(LevelsManager.currentLevel.endSpeed);

        treeModulesPrefabsPool.Add(newTreeModule);
    }

    private static Sprite LoadSprite(string treesPath)
    {
        var path = treesPath +
              FilenameDictionary.DEFAULT_TREE_MODULES_NAMES[
                  Random.Range(0, FilenameDictionary.DEFAULT_TREE_MODULES_NAMES.Length)
              ];
        var sprite = Resources.Load<Sprite>(path);
        Debug.Log(path);
        return sprite;
    }

    public static void DestroyOldTreeModules()
    {
        var oldTreeModules = treeModulesPrefabsPool.Where(item => item.transform.position.y > DESTRUCTION_POSITION.y);
        oldTreeModules.ToList().ForEach(item => Destroy(item));
        treeModulesPrefabsPool = treeModulesPrefabsPool.Except(oldTreeModules).ToList();
    }
}
