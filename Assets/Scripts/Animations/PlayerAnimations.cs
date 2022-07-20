using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static Rigidbody2D playerRigidbody2D;
    private static GameObject playerGameObject;

    private void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerGameObject = gameObject;
    }
    public static void PlayDeathAnimation()
    {
        playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody2D.gravityScale = PlayerHelper.GRAVITY_SCALE_FALL;

        if (PlayerManager.PositionSide == Helper.SIDE_RIGHT)
        {
            playerRigidbody2D.AddForce(PlayerHelper.DEATH_FORCE);
            playerRigidbody2D.AddTorque(PlayerHelper.DEATH_TORQUE);
        }
        else
        {
            playerRigidbody2D.AddForce(new Vector2(-PlayerHelper.DEATH_FORCE.x, PlayerHelper.DEATH_FORCE.y));
            playerRigidbody2D.AddTorque(-PlayerHelper.DEATH_TORQUE);
        }
    }
}
