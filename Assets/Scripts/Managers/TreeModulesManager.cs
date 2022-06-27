using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TreeModuleChildren
{
    branch,
    brokenBranch
}

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

    private static int currentModuleID = 0;

    public void Start()
    {
        treeModulePrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.TREE_PREFAB));
        InitializeNewTreeModule();
        currentModuleID = LevelsManager.currentLevel.treeModules.First().moduleID;
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

        treeModuleSpriteRenderer.sprite = LoadSprite(
            LevelsManager.currentLevel.treeModulesPath,
            LevelsManager.currentLevel.treeModules[currentModuleID].spriteName
        );
        treeModuleSpriteRenderer.flipX = LevelsManager.currentLevel.treeModules[currentModuleID].flipX;

        var treeBehaviour = newTreeModule.GetComponent<TreeBehaviour>();
        treeBehaviour.shouldMove = true;
        treeBehaviour.ChangeSpeed(LevelsManager.currentLevel.endSpeed);

        SetupBranchForTreeModule(newTreeModule);

        currentModuleID++;

        treeModulesPrefabsPool.Add(newTreeModule);
    }

    private static void SetupBranchForTreeModule(GameObject treeModule)
    {
        var branchGameObject = treeModule.transform.GetChild((int)TreeModuleChildren.branch).gameObject;
        if (branchGameObject == null) return;
        var branchSpriteRenderer = branchGameObject.GetComponent<SpriteRenderer>();
        if (branchSpriteRenderer == null) return;

        branchGameObject.SetActive(true);

        //loading branch sprite
        branchSpriteRenderer.sprite = LoadSprite(
            LevelsManager.currentLevel.branchesPath,
            LevelsManager.currentLevel.treeModules[currentModuleID].branch.spriteName
        );

        //flipping sprite if side is RIGHT
        if (LevelsManager.currentLevel.treeModules[currentModuleID].branch.side != Helper.SIDE_LEFT)
        {
            branchSpriteRenderer.flipX = true;
            branchGameObject.transform.position = new Vector2(
                branchGameObject.transform.position.x * -1,
                branchGameObject.transform.position.y
            );
        }
    }

    private static Sprite LoadSprite(string path, string spriteName)
    {
        var spritePath = path + spriteName;
        var sprite = Resources.Load<Sprite>(spritePath);
        return sprite;
    }

    public static void DestroyOldTreeModules()
    {
        //TODO: don't check all array every frame, need to optimize this
        var oldTreeModules = treeModulesPrefabsPool.Where(item => item.transform.position.y > DESTRUCTION_POSITION.y);
        oldTreeModules.ToList().ForEach(item => Destroy(item));
        //assign new list without old tree modules
        treeModulesPrefabsPool = treeModulesPrefabsPool.Except(oldTreeModules).ToList();
    }
}
