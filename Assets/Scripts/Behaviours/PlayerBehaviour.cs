using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;


    public void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerRigidbody.gravityScale = PlayerHelper.GRAVITY_SCALE_IDLE;
    }

    private void Update()
    {
        //SET PLAYER POSITION VALUE
        PlayerManager.PositionSide = playerRigidbody.position.x >= 0 ? Helper.SIDE_RIGHT : Helper.SIDE_LEFT;

        //JUMP ON BUTTON CLICK
        if (Input.GetMouseButtonDown(0) && GameManager.UI.IsSecurityPanelClicked())
        {
            JumpToSide(GetOppositeSide(PlayerManager.PositionSide));
        }

        //STOP PLAYER AFTER JUMP
        if (gameObject.transform.position.x > PlayerHelper.RIGHT_X_POSITION)
        {
            if (PlayerManager.IsDead)
                HidePlayerAfterDeath();
            else
                StopAtSide(Helper.SIDE_RIGHT);
        }
        else if (gameObject.transform.position.x < PlayerHelper.LEFT_X_POSITION)
        {
            if (PlayerManager.IsDead)
                HidePlayerAfterDeath();
            else
                StopAtSide(Helper.SIDE_LEFT);
        }
    }

    private char GetOppositeSide(char side) => side == Helper.SIDE_LEFT ? Helper.SIDE_RIGHT : Helper.SIDE_LEFT;

    public void SetBodyDirection(char side)
    {
        if (side == Helper.SIDE_LEFT)
        {
            gameObject.transform.rotation = Quaternion.Euler(0F, -180F, PlayerHelper.PLAYER_Z_ROTATION);
            transform.GetChild((int)PlayerChildren.shield).transform.rotation = Quaternion.Euler(0F, 0F, -25F);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0F, 0F, PlayerHelper.PLAYER_Z_ROTATION);
            transform.GetChild((int)PlayerChildren.shield).transform.rotation = Quaternion.Euler(0F, 0F, -25F);
        }
    }

    public void StopAtSide(char side)
    {
        if (PlayerManager.IsDead) return;

        PlayerManager.IsJumping = false;
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerRigidbody.gravityScale = PlayerHelper.GRAVITY_SCALE_IDLE;

        if (side == Helper.SIDE_LEFT)
        {
            gameObject.transform.position = new Vector2(-PlayerHelper.INITIAL_POSITION.x, PlayerHelper.INITIAL_POSITION.y);

        }
        else if (side == Helper.SIDE_RIGHT)
        {
            gameObject.transform.position = PlayerHelper.INITIAL_POSITION;
        }
    }

    private void HidePlayerAfterDeath()
    {
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        gameObject.transform.position = PlayerHelper.PLAYER_DEATH_HIDE_POSITION;
    }

    public void JumpToSide(char side)
    {
        if (PlayerManager.IsDead || PlayerManager.IsJumping) return;

        PlayerManager.IsJumping = true;
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody.gravityScale = PlayerHelper.GRAVITY_SCALE_JUMP;
        PlayerAnimations.PlayJump();
        if (side == Helper.SIDE_RIGHT)
        {
            playerRigidbody.AddForce(new Vector2(-PlayerHelper.JUMP_FORCE.x, PlayerHelper.JUMP_FORCE.y));
            SetBodyDirection(Helper.SIDE_RIGHT);
        }
        else
        {
            playerRigidbody.AddForce(PlayerHelper.JUMP_FORCE);
            SetBodyDirection(Helper.SIDE_LEFT);
        }
    }

    private void HandlePlayerHit()
    {
        PlayerManager.SubstractLives(1);
        PlayerAnimations.PlayHit(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.IsDead) return;

        switch (collision.tag)
        {
            case TagsDictionary.BRANCH:
                if (!PlayerManager.IsShieldUp)
                {
                    TreeManager.BreakModuleBranch(collision.gameObject);
                    CameraShake.Instance.ShakeCamera(4f, .5f, .07f);
                    HandlePlayerHit();
                }
                break;
            case TagsDictionary.COIN:
                CoinsManager.AddCoins(1);
                collision.gameObject.GetComponent<BonusBehaviour>().RemoveBonusGameObject();
                collision.gameObject.GetComponent<BonusAnimations>().PlayCatch();
                collision.GetComponentInParent<TreeBehaviour>().ActivateBonusLabel();
                break;
            case TagsDictionary.PEANUT:
                ScoreManager.AddScoreBonus(100);
                collision.gameObject.GetComponent<BonusBehaviour>().RemoveBonusGameObject();
                collision.gameObject.GetComponent<BonusAnimations>().PlayCatch();
                PlayerManager.TurnShieldOn();
                collision.GetComponentInParent<TreeBehaviour>().ActivateBonusLabel();
                break;
            case TagsDictionary.CARROT:
                ScoreManager.AddScoreBonus(50);
                CoinsManager.AddCoins(20);
                collision.gameObject.GetComponent<BonusBehaviour>().RemoveBonusGameObject();
                collision.gameObject.GetComponent<BonusAnimations>().PlayCatch();
                collision.GetComponentInParent<TreeBehaviour>().ActivateBonusLabel();
                break;
            case TagsDictionary.HEART:
                PlayerManager.AddLives(1);
                collision.gameObject.GetComponent<BonusBehaviour>().RemoveBonusGameObject();
                collision.gameObject.GetComponent<BonusAnimations>().PlayCatch();
                collision.GetComponentInParent<TreeBehaviour>().ActivateBonusLabel();
                break;
            default:
                break;
        }
    }
}
