using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region STATIC
    public static PlayerManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    public static int NumberOfLives { get; private set; } = PlayerHelper.INITIAL_NUMBER_OF_LIVES;
    public static bool IsDead { get; private set; } = false;
    public static char PositionSide { get; set; } = Helper.SIDE_RIGHT;
    public static bool IsJumping { get; set; } = false;
    public static GameObject SelectedCharacterGameObject { get; private set; }

    private void Start()
    {
        SelectedCharacterGameObject = GameManager.UI.player.transform.GetChild(GameStateManager.CurrentGameState.selectedCharacterId).gameObject;
    }

    public static void UpdateSelectedCharacter(GameObject characterGameObject = default)
    {
        if (characterGameObject == default)
        {
            SelectedCharacterGameObject
                = GameManager.UI.player.transform.GetChild(GameStateManager.CurrentGameState.selectedCharacterId).gameObject;
            return;
        }
        SelectedCharacterGameObject = characterGameObject;
    }

    public static void SubstractLives(int livesToSubstract)
    {
        if (IsDead) return;

        NumberOfLives -= livesToSubstract;
        GameManager.UI.UpdateDisplayedHealth(NumberOfLives);

        if (NumberOfLives == 1)
        {
            var character = PlayerHelper.CHARACTERS.Find(x => x.ID == GameStateManager.CurrentGameState.selectedCharacterId);
            SelectedCharacterGameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>(PathsDictionary.GetPlayerPath(character.Name) + FilenameDictionary.BODY_DAMAGED);
        }

        if (NumberOfLives <= 0)
        {
            PlayerAnimations.PlayDeath();
            IsDead = true;
            GameManager.EndGame();
        }
    }

    public static void SetValuesToDefault()
    {
        NumberOfLives = PlayerHelper.INITIAL_NUMBER_OF_LIVES;
        IsDead = false;
        PositionSide = Helper.SIDE_RIGHT;
        IsJumping = false;
    }
}
