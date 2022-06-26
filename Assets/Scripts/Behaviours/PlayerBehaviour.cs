using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    private static readonly float MAX_X_POSITION = 2;
    private static readonly float MIN_X_POSITION = -2;
    private static readonly float GRAVITY_SCALE_IDLE = 0;
    private static readonly float GRAVITY_SCALE_JUMP = 55;
    private static readonly Vector2 INITIAL_POSITION = new Vector2(-2f, 0f);

    private static Vector2 jumpDirection = new Vector2(100f, 50f);
    public void Awake()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerRigidbody.gravityScale = GRAVITY_SCALE_IDLE;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Jump();
        }
        if (gameObject.transform.position.x > MAX_X_POSITION)
        {
            Debug.Log("if" + gameObject.transform.position.x + "|" + MAX_X_POSITION);
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            playerRigidbody.gravityScale = GRAVITY_SCALE_IDLE;
            gameObject.transform.position = new Vector2(INITIAL_POSITION.x * -1, INITIAL_POSITION.y);
        }
        else if (gameObject.transform.position.x < MIN_X_POSITION)
        {
            Debug.Log("elseif" + gameObject.transform.position.x + "|" + MIN_X_POSITION);
            playerRigidbody.bodyType = RigidbodyType2D.Static;
            playerRigidbody.gravityScale = GRAVITY_SCALE_IDLE;
            gameObject.transform.position = INITIAL_POSITION;
        }
    }

    public void Jump()
    {
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody.gravityScale = GRAVITY_SCALE_JUMP;
        playerRigidbody.AddForce(jumpDirection);
        jumpDirection = new Vector2(jumpDirection.x * -1, jumpDirection.y);
        Debug.Log(jumpDirection.x + "," + jumpDirection.y);
    }
}
