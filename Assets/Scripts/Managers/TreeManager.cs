using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TreeModuleChildren
{
    branch,
    brokenBranch,
    coin,
    levelCanvas,
    peanut,
    carrot,
    heart,
    bonusCanvas
}

public enum BonusPanelChildren
{
    bonusLabel,
    bonusIcon,
    bonusLabel2,
    bonusIcon2
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
    public static bool isInfinityMode = false;
    private static Sprite coinSprite;
    private static Sprite heartSprite;
    private static Sprite scoreSprite;

    public void Start()
    {
        treeModulePrefab = Resources.Load<GameObject>(
            PathsDictionary.GetFullPath(PathsDictionary.PREFABS, FilenameDictionary.TREE_PREFAB));
        initialTree = GameObject.Find(Helper.GO_NAME_INITIAL_TREE);
        coinSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.BONUSES, FilenameDictionary.COIN));
        heartSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.UI_HEARTS, FilenameDictionary.UI_HEART_RED));
        scoreSprite = Resources.Load<Sprite>(
            PathsDictionary.GetFullPath(PathsDictionary.UI_OTHER, FilenameDictionary.STAR));
    }

    public static void StartMovingTree()
    {
        if (GameManager.IsGameStarted)
        {
            InitializeNewTreeModules(INITIALIZE_POSITION, LevelsManager.currentLevel);
            var initialTreeBehaviour = initialTree.GetComponent<InitialTreeBehaviour>();
            var treeBehaviours = treeModulesPrefabsPool.Select(x => x.GetComponent<TreeBehaviour>()).ToList();
            if (initialTreeBehaviour != null && treeBehaviours.Count > 0)
            {
                treeBehaviours.ForEach(x => x.StartMoving());
                initialTreeBehaviour.StartMoving();
            }
        }

    }

    public static void UpdateModulesSpeed()
    {
        var treeBehaviours = treeModulesPrefabsPool.Select(x => x.GetComponent<TreeBehaviour>()).ToList();
        foreach (var item in treeBehaviours)
        {
            item.ChangeSpeed(LevelsManager.currentLevel.endSpeed);
        }
    }

    public static void ManageTreeModules()
    {
        UpdateModulesSpeed();
        DestroyOldTreeModules();
        if (isInfinityMode)
            LoadNextRandomLevel();
        else
            LoadNextLevel();
    }

    private static void InitializeNewTreeModules(Vector2 startPosition, LevelModel levelToLoad)
    {
        if (levelToLoad?.ID == default) return;
        GameManager.UI.SetPlayerTrailEmmiterSpeed(PlayerManager.CalculateTrailEmmitterSpeed());

        var moduleSizeY = treeModulePrefab.GetComponent<BoxCollider2D>().size.y;
        Vector2 positionToInstantiate = new Vector2(startPosition.x, startPosition.y + (moduleSizeY * 0.01F));

        for(int i = 0; i < levelToLoad.treeModules.Count; i++)
        {
            var newTreeModule = Instantiate(treeModulePrefab, positionToInstantiate, Quaternion.identity);
            var treeModuleSpriteRenderer = newTreeModule.GetComponent<SpriteRenderer>();
            if (i == 0 && (GameManager.IsGameRandom || LevelsManager.currentLevel.ID <= Helper.LEVELS_COUNT))
            {
                var canvas = newTreeModule.transform.GetChild((int)TreeModuleChildren.levelCanvas).gameObject;
                canvas.SetActive(true);
                canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = LevelsManager.currentLevel.ID.ToString();
            }

            treeModuleSpriteRenderer.sprite = LoadSprite(
                levelToLoad.treeModulesPath,
                levelToLoad.treeModules[i].spriteName
            );
            treeModuleSpriteRenderer.flipX = levelToLoad.treeModules[i].flipX;

            var treeBehaviour = newTreeModule.GetComponent<TreeBehaviour>();
            treeBehaviour.moduleId = i;
            if (GameManager.IsGameStarted)
            {
                treeBehaviour.StartMoving();
            }
            treeBehaviour.ChangeSpeed(LevelsManager.currentLevel.endSpeed);

            SetupBonusesForTreeModule(newTreeModule, LevelsManager.currentLevel.treeModules, i);

            SetupBranchForTreeModule(newTreeModule, i);

            treeModulesPrefabsPool.Add(newTreeModule);

            positionToInstantiate = new Vector2(
                startPosition.x,
                newTreeModule.transform.position.y
                - moduleSizeY
                + (Time.deltaTime * treeBehaviour.Speed));
        }

        
    }

    private static void SetupBonusesForTreeModule(GameObject treeModule, List<TreeModuleModel> treeModules, int currentModuleID)
    {
        //-2 is to make it safe and not go out of index
        if (currentModuleID == 0) return;

        var currentTreeModule = treeModules[currentModuleID];
        var previousTreeModule = treeModules[currentModuleID - 1];

        if (previousTreeModule.hasBonus)
        {
            return;
        }
        
        int randomBonus = Random.Range(0, 110);
        if (randomBonus > 25) 
        { 
            return; 
        }
        currentTreeModule.hasBonus = true;
        if (randomBonus == 0)
        {
            treeModule.transform.GetChild((int)TreeModuleChildren.peanut).gameObject.SetActive(true);
            SetBonusLabelsForTreeModule(treeModule, (int)TreeModuleChildren.peanut);
        } 
        else if (randomBonus == 1)
        {
            treeModule.transform.GetChild((int)TreeModuleChildren.heart).gameObject.SetActive(true);
            SetBonusLabelsForTreeModule(treeModule, (int)TreeModuleChildren.heart);
        }
        else if (randomBonus >= 2 && randomBonus <= 3)
        {
            treeModule.transform.GetChild((int)TreeModuleChildren.carrot).gameObject.SetActive(true);
            SetBonusLabelsForTreeModule(treeModule, (int)TreeModuleChildren.carrot);
        }
        else
        {
            treeModule.transform.GetChild((int)TreeModuleChildren.coin).gameObject.SetActive(true);
            SetBonusLabelsForTreeModule(treeModule, (int)TreeModuleChildren.coin);
        }
    }

    private static void SetBonusLabelsForTreeModule(GameObject treeModule, int bonusChildrenId)
    {
        var bonusCanvas = treeModule.transform.GetChild((int)TreeModuleChildren.bonusCanvas);
        var bonusPanel = bonusCanvas.transform.GetChild(0);
        switch (bonusChildrenId)
        {
            case (int)TreeModuleChildren.heart:
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon).GetComponent<Image>().sprite = heartSprite;
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel).GetComponent<TextMeshProUGUI>().text = "x 1";
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon2).gameObject.SetActive(false);
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel2).gameObject.SetActive(false);
                break;
            case (int)TreeModuleChildren.coin:
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon).GetComponent<Image>().sprite = coinSprite;
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel).GetComponent<TextMeshProUGUI>().text = "x 1";
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon2).gameObject.SetActive(false);
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel2).gameObject.SetActive(false);
                break;
            case (int)TreeModuleChildren.carrot:
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon).GetComponent<Image>().sprite = coinSprite;
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel).GetComponent<TextMeshProUGUI>().text = "x 20";
                if (GameManager.IsGameRandom)
                {
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon2).GetComponent<Image>().sprite = scoreSprite;
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel2).GetComponent<TextMeshProUGUI>().text = "x 50";
                }
                else
                {
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon2).gameObject.SetActive(false);
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel2).gameObject.SetActive(false);
                }
                break;
            case (int)TreeModuleChildren.peanut:
                if (GameManager.IsGameRandom)
                {
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon).GetComponent<Image>().sprite = scoreSprite;
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel).GetComponent<TextMeshProUGUI>().text = "x 100";
                }
                else
                {
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon).gameObject.SetActive(false);
                    bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel).gameObject.SetActive(false);
                }
                bonusPanel.GetChild((int)BonusPanelChildren.bonusIcon2).gameObject.SetActive(false);
                bonusPanel.GetChild((int)BonusPanelChildren.bonusLabel2).gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private static void SetupBranchForTreeModule(GameObject treeModule, int currentModuleID)
    {
        var branchGameObject = treeModule.transform.GetChild((int)TreeModuleChildren.branch).gameObject;
        if (branchGameObject == null) return;
        var branchSpriteRenderer = branchGameObject.GetComponent<SpriteRenderer>();
        if (branchSpriteRenderer == null) return;

        //loading branch sprite
        branchSpriteRenderer.sprite = LoadSprite(
            LevelsManager.currentLevel.branchesPath,
            LevelsManager.currentLevel.treeModules[currentModuleID].branch.spriteName
        );

        if (LevelsManager.currentLevel.treeModules[currentModuleID].branch.side == Helper.SIDE_NONE)
            branchGameObject.SetActive(false);
        else
            branchGameObject.SetActive(true);

        //flipping sprite if side is RIGHT
        if (LevelsManager.currentLevel.treeModules[currentModuleID].branch.side == Helper.SIDE_RIGHT)
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

    private static void LoadNextRandomLevel()
    {
        bool shouldLoad = treeModulesPrefabsPool.Count < 10;
        // Load next random level
        if (shouldLoad)
        {
            LevelsManager.LoadNextRandomLevel();
        }

        bool shouldSwitch = treeModulesPrefabsPool.Count < 7;
        // Switch to next random level
        if (shouldSwitch)
        {
            LevelsManager.SwitchToNextRandomLevel();
            var lastModule = treeModulesPrefabsPool.Last();
            var lastModuleCollider = lastModule.GetComponent<Collider2D>();
            InitializeNewTreeModules(
                new Vector2(lastModule.transform.position.x,
                    lastModule.transform.position.y - lastModuleCollider.bounds.size.y + (Time.deltaTime * LevelsManager.currentLevel.endSpeed)),
                LevelsManager.currentLevel);
        }
    }

    private static void LoadNextLevel()
    {
        bool shouldLoad = treeModulesPrefabsPool.Count < 15 && !LevelsManager.isNextLevelReady;
        // Load next level
        if (shouldLoad)
        {
            LevelsManager.LoadNextLevel();
        }

        bool shouldSwitch = treeModulesPrefabsPool.Count < 10;
        // Switch to next level
        if (shouldSwitch)
        {
            LevelsManager.SwitchToNextLevel();
            var lastModule = treeModulesPrefabsPool.Last();
            var lastModuleCollider = lastModule.GetComponent<Collider2D>();
            InitializeNewTreeModules(
                new Vector2(lastModule.transform.position.x,
                    lastModule.transform.position.y - lastModuleCollider.bounds.size.y + (Time.deltaTime * LevelsManager.currentLevel.endSpeed)),
                LevelsManager.currentLevel);
        }
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

        if (LevelsManager.currentLevel.treeModules.Count < parentBehaviour.moduleId)
        {
            brokenBranch.GetComponent<SpriteRenderer>().sprite = LoadSprite(
            LevelsManager.currentLevel.branchesPath,
            FilenameDictionary.DEFAULT_BRANCH_NAMES[0].BrokenBranchName);
            brokenBranch.SetActive(true);
            return;
        }

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
        isInfinityMode = false;
    }
}
