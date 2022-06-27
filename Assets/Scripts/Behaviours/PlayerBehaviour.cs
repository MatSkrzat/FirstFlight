using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    private static readonly float RIGHT_X_POSITION = 2;
    private static readonly float LEFT_X_POSITION = -2;
    private static readonly float GRAVITY_SCALE_IDLE = 0;
    private static readonly float GRAVITY_SCALE_JUMP = 55;
    private static readonly Vector2 INITIAL_POSITION = new Vector2(2f, 0f);
    private static readonly float PLAYER_Z_ROTATION = -25F;
    private static readonly Vector2 JUMP_FORCE = new Vector2(-80f, 40f);
    private static char positionSide = Helper.SIDE_RIGHT;

    public void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerRigidbody.gravityScale = GRAVITY_SCALE_IDLE;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            JumpToSide(GetOppositeSide(positionSide));
        }
        if (gameObject.transform.position.x > RIGHT_X_POSITION)
        {
            StopAtSide(Helper.SIDE_RIGHT);
        }
        else if (gameObject.transform.position.x < LEFT_X_POSITION)
        {
            StopAtSide(Helper.SIDE_LEFT);
        }
    }

    private char GetOppositeSide(char side) => side == Helper.SIDE_LEFT ? Helper.SIDE_RIGHT : Helper.SIDE_LEFT;

    public void SetBodyDirection(char side)
    {
        if (side == Helper.SIDE_LEFT)
        {
            gameObject.transform.rotation = Quaternion.Euler(0F, -180F, PLAYER_Z_ROTATION);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0F, 0F, PLAYER_Z_ROTATION);
        }
    }

    public void StopAtSide(char side)
    {
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerRigidbody.gravityScale = GRAVITY_SCALE_IDLE;

        if (side == Helper.SIDE_LEFT)
        {
            gameObject.transform.position = new Vector2(INITIAL_POSITION.x * -1, INITIAL_POSITION.y);
            positionSide = Helper.SIDE_LEFT;

        }
        else if (side == Helper.SIDE_RIGHT)
        {
            gameObject.transform.position = INITIAL_POSITION;
            positionSide = Helper.SIDE_RIGHT;
        }
    }

    public void JumpToSide(char side)
    {
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody.gravityScale = GRAVITY_SCALE_JUMP;
        if (side == Helper.SIDE_RIGHT)
        {
            playerRigidbody.AddForce(new Vector2(JUMP_FORCE.x * -1, JUMP_FORCE.y));
            SetBodyDirection(Helper.SIDE_RIGHT);
        }
        else
        {
            playerRigidbody.AddForce(JUMP_FORCE);
            SetBodyDirection(Helper.SIDE_LEFT);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagsDictionary.BRANCH)
        {
            TreeModulesManager.BreakModuleBranch(collision.gameObject);
        }
    }
}
