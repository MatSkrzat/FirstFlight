using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TreeModuleChildren
{
    branch,
    brokenBranch,
    coin
}

public class TreeManager : MonoBehaviour
{
    #region STATIC
    public static TreeManager instance;
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
    public static GameObject initialTree;
    public static readonly Vector2 INITIALIZE_POSITION = new Vector2(0F, -8.3F);
    public static readonly Vector2 DESTRUCTION_POSITION = new Vector2(0F, 10F);
    public static readonly Vector2 NEW_TREE_MODULE_INIT_POSITION = new Vector2(0, INITIALIZE_POSITION.y + 2.8F);

    private static int currentModuleID = 0;

    public void Start()
    {
        treeModulePrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.TREE_PREFAB));
        currentModuleID = LevelsManager.currentLevel.treeModules.First().moduleID;
        initialTree = GameObject.Find(Helper.GO_NAME_INITIAL_TREE);
        InitializeNewTreeModule();
    }

    public static void StartMovingTree()
    {
        if (GameManager.IsGameStarted)
        {
            var initialTreeBehaviour = initialTree.GetComponent<InitialTreeBehaviour>();
            var treeBehaviours = treeModulesPrefabsPool.Select(x => x.GetComponent<TreeBehaviour>()).ToList();
            if (initialTreeBehaviour != null && treeBehaviours.Count > 0)
            {
                initialTreeBehaviour.StartMoving();
                treeBehaviours.ForEach(x => x.StartMoving());
            }
        }

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
        treeBehaviour.moduleId = currentModuleID;
        if (GameManager.IsGameStarted)
        {
            treeBehaviour.StartMoving();
        }
        treeBehaviour.ChangeSpeed(LevelsManager.currentLevel.endSpeed);

        SetupBonusesForTreeModule(newTreeModule, LevelsManager.currentLevel.treeModules, currentModuleID);

        SetupBranchForTreeModule(newTreeModule);
        currentModuleID++;

        if (LevelsManager.currentLevel.treeModules.Count - currentModuleID == 10)
        {
            LevelsManager.LoadNextLevel();
        }

        if(currentModuleID >= LevelsManager.currentLevel.treeModules.Count)
        {
            LevelsManager.SwitchToNextLevel();
            currentModuleID = LevelsManager.currentLevel.treeModules.First().moduleID;
        }

        treeModulesPrefabsPool.Add(newTreeModule);
    }

    private static void SetupBonusesForTreeModule(GameObject treeModule, List<TreeModuleModel> treeModules, int currentModuleID)
    {
        //-2 is to make it safe and not go out of index
        if (treeModules.Count - 2 < currentModuleID) return;

        var currentTreeModule = treeModules[currentModuleID];
        var nextTreeModule = treeModules[currentModuleID + 1];

        if (currentTreeModule.hasBonus)
        {

        }
        else if (currentTreeModule.branch.side != nextTreeModule.branch.side)
        {
            treeModule.transform.GetChild((int)TreeModuleChildren.coin).gameObject.SetActive(Random.Range(0, 2) == 0);
        }
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
            ChangeObjectSide(branchGameObject);
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

    public static void BreakModuleBranch(GameObject branch)
    {
        if (branch == null) return;
        branch.SetActive(false);
        var parent = branch.transform.parent.gameObject;
        var parentBehaviour = parent.GetComponent<TreeBehaviour>();
        var brokenBranch = parent.transform.GetChild((int)TreeModuleChildren.brokenBranch).gameObject;
        if (branch.transform.position.x > 0)
            ChangeObjectSide(brokenBranch);
        brokenBranch.GetComponent<SpriteRenderer>().sprite = LoadSprite(
            LevelsManager.currentLevel.branchesPath, 
            LevelsManager.currentLevel.treeModules[parentBehaviour.moduleId].branch.brokenBranchSpriteName);
        brokenBranch.SetActive(true);
    }

    private static void ChangeObjectSide(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
        gameObject.transform.position = new Vector2(
            -gameObject.transform.position.x,
            gameObject.transform.position.y
        );
        var collider = gameObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
        }
    }

    public static void SetValuesToDefault()
    {
        treeModulesPrefabsPool = new List<GameObject>();
        currentLevelModules = new List<TreeModuleModel>();
        treeModulePrefab = null;
    }
}
