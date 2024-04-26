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
    public static List<GameObject> treeModulesPool;
    public static List<TreeBehaviour> treeBehavioresPool;
    public static List<SpriteRenderer> treeSpritesPool;
    public static GameObject treeModulePrefab;
    public static GameObject initialTree;
    public static readonly Vector2 INITIALIZE_POSITION = new Vector2(0F, -8.3F);
    public static readonly Vector2 DESTRUCTION_POSITION = new Vector2(0F, 10F);
    public static readonly Vector2 NEW_TREE_MODULE_INIT_POSITION = new Vector2(0, INITIALIZE_POSITION.y + 2.8F);
    public static readonly Vector2 BRANCH_DEFAULT_POSITION = new Vector2(-1.72F, 0);
    public static readonly Vector2 BROKEN_BRANCH_DEFAULT_POSITION = new Vector2(-1.115F, -0.6F);
    public static bool isInfinityMode = false;
    private static Sprite coinSprite;
    private static Sprite heartSprite;
    private static Sprite scoreSprite;
    private static int treeModulesPoolIndex;
    private static TreeBehaviour lastTreeModuleBehaviour;
    private static int moduleToLoadId;


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
        treeModulesPool = new List<GameObject>(15);
        treeBehavioresPool = new List<TreeBehaviour>(15);
        treeSpritesPool = new List<SpriteRenderer>(15);
        treeModulesPoolIndex = 0;
        moduleToLoadId = 0;
        for(int i = 0; i < treeModulesPool.Capacity; i++)
        {
            var module = Instantiate(treeModulePrefab, INITIALIZE_POSITION, Quaternion.identity);
            module.SetActive(false);
            treeModulesPool.Add(module);
            treeBehavioresPool.Add(module.GetComponent<TreeBehaviour>());
            treeSpritesPool.Add(module.GetComponent<SpriteRenderer>());
        }
    }

    private void Update()
    {
        if(!GameManager.IsGameStarted) return;

        if (isInfinityMode)
            LoadNextRandomLevel();
        else
            LoadNextLevel();

        if (lastTreeModuleBehaviour == null)
        {
            InstantiateNewTreeModule(LevelsManager.currentLevel.treeModules.First(), NEW_TREE_MODULE_INIT_POSITION);
            return;
        }

        if (LevelsManager.currentLevel.treeModules.Count < moduleToLoadId) return;
        if (lastTreeModuleBehaviour.gameObject.transform.position.y <= NEW_TREE_MODULE_INIT_POSITION.y) return;

        InstantiateNewTreeModule(LevelsManager.currentLevel.treeModules[moduleToLoadId], new Vector2(0, lastTreeModuleBehaviour.transform.position.y - 2.8F));
    }

    public static void StartMovingTree()
    {
        if (GameManager.IsGameStarted)
        {
            var initialTreeBehaviour = initialTree.GetComponent<InitialTreeBehaviour>();
            if (initialTreeBehaviour != null)
            {
                initialTreeBehaviour.StartMoving();
            }
        }
    }

    public static void UpdateModulesSpeed()
    {
        //TODO: don't use GetComponent in Update
        var treeBehaviours = treeModulesPool.Select(x => x.GetComponent<TreeBehaviour>()).ToList();
        foreach (var item in treeBehaviours)
        {
            item.ChangeSpeed(LevelsManager.currentLevel.endSpeed);
        }
    }

    private static void InstantiateNewTreeModule(TreeModuleModel treeModuleModel, Vector2 positionToInstantiate)
    {
        var treeModule = treeModulesPool[treeModulesPoolIndex];
        var treeBehaviour = treeBehavioresPool[treeModulesPoolIndex];
        var treeSprite = treeSpritesPool[treeModulesPoolIndex];
        CleanModuleToUseAgain(treeModule);
        treeBehaviour.CleanBehaviour();
        treeModule.transform.position = positionToInstantiate;
        treeModule.SetActive(true);
        if (GameManager.IsGameStarted)
        {
            treeBehaviour.StartMoving();
        }

        treeBehaviour.ChangeSpeed(LevelsManager.currentLevel.endSpeed);

        //adding level number to the first module
        if (treeModuleModel.moduleID == 0
            && (GameManager.IsGameRandom || LevelsManager.currentLevel.ID <= Helper.LEVELS_COUNT))
        {
            var canvas = treeModule.transform.GetChild((int)TreeModuleChildren.levelCanvas).gameObject;
            canvas.SetActive(true);
            canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text
                = LevelsManager.currentLevel.ID.ToString();
        }

        //loading tree module sprite
        treeSprite.sprite = LoadSprite(
            LevelsManager.currentLevel.treeModulesPath,
            LevelsManager.currentLevel.treeModules[treeModuleModel.moduleID].spriteName
        );
        treeSprite.flipX = LevelsManager.currentLevel.treeModules[treeModuleModel.moduleID].flipX;

        treeBehaviour.moduleId = treeModuleModel.moduleID;

        SetupBonusesForTreeModule(treeModule, LevelsManager.currentLevel.treeModules, treeModuleModel.moduleID);
        SetupBranchForTreeModule(treeModule, treeModuleModel.moduleID);
        treeModulesPool[treeModulesPoolIndex] = treeModule;
        if (treeModulesPoolIndex < treeModulesPool.Capacity - 1)
        {
            treeModulesPoolIndex++;
        }
        else
        {
            treeModulesPoolIndex = 0;
        }
        moduleToLoadId++;
        lastTreeModuleBehaviour = treeBehaviour;
    }

    private static void CleanModuleToUseAgain(GameObject treeModule)
    {
        var branch = treeModule.transform.GetChild((int)TreeModuleChildren.branch).gameObject;
        var brokenBranch = treeModule.transform.GetChild((int)TreeModuleChildren.brokenBranch).gameObject;

        branch.SetActive(false);
        branch.GetComponent<SpriteRenderer>().flipX = false;
        branch.transform.localPosition = BRANCH_DEFAULT_POSITION;

        brokenBranch.SetActive(false);
        brokenBranch.GetComponent<SpriteRenderer>().flipX = false;
        brokenBranch.transform.localPosition = BROKEN_BRANCH_DEFAULT_POSITION;

        treeModule.transform.GetChild((int)TreeModuleChildren.coin).gameObject.SetActive(false);
        treeModule.transform.GetChild((int)TreeModuleChildren.peanut).gameObject.SetActive(false);
        treeModule.transform.GetChild((int)TreeModuleChildren.carrot).gameObject.SetActive(false);
        treeModule.transform.GetChild((int)TreeModuleChildren.heart).gameObject.SetActive(false);
        treeModule.transform.GetChild((int)TreeModuleChildren.levelCanvas).gameObject.SetActive(false);
        treeModule.transform.GetChild((int)TreeModuleChildren.bonusCanvas).gameObject.SetActive(false);
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

    private static void LoadNextRandomLevel()
    {
        if (lastTreeModuleBehaviour == null) return;

        bool shouldLoad = LevelsManager.currentLevel.treeModules.Last().moduleID - lastTreeModuleBehaviour.moduleId < 15
            && !LevelsManager.isNextLevelReady;

        // Load next random level
        if (shouldLoad)
        {
            LevelsManager.LoadNextRandomLevel();
        }

        bool shouldSwitch = LevelsManager.currentLevel.treeModules.Last().moduleID - lastTreeModuleBehaviour.moduleId < 10
            && LevelsManager.isNextLevelReady;
        // Switch to next random level
        if (shouldSwitch)
        {
            LevelsManager.SwitchToNextRandomLevel();
            UpdateModulesSpeed();
        }
    }

    private static void LoadNextLevel()
    {
        if(lastTreeModuleBehaviour == null) return;

        bool shouldLoad = LevelsManager.currentLevel.treeModules.Last().moduleID - lastTreeModuleBehaviour.moduleId < 15
            && !LevelsManager.isNextLevelReady;
        // Load next level
        if (shouldLoad)
        {
            LevelsManager.LoadNextLevel();
        }

        bool shouldSwitch = LevelsManager.currentLevel.treeModules.Last().moduleID - lastTreeModuleBehaviour.moduleId < 1
            && LevelsManager.isNextLevelReady;
        // Switch to next level
        if (shouldSwitch)
        {
            moduleToLoadId = 0;
            LevelsManager.SwitchToNextLevel();
            UpdateModulesSpeed();
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
        treeModulesPool = new List<GameObject>(15);
        treeBehavioresPool = new List<TreeBehaviour>(15);
        treeSpritesPool = new List<SpriteRenderer>(15);
        treeModulesPoolIndex = 0;
        moduleToLoadId = 0;
        treeModulePrefab = null;
        isInfinityMode = false;
    }
}
