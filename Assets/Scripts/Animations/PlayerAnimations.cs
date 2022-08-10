using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private static Rigidbody2D playerRigidbody2D;

    private void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    public static void PlayDeathAnimation()
    {
        if (PlayerManager.IsDead) return;

        playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        playerRigidbody2D.gravityScale = PlayerHelper.GRAVITY_SCALE_FALL;
        playerRigidbody2D.velocity = Vector2.zero;

        //RIGHT
        if (playerRigidbody2D.position.x >= 0)
        {
            playerRigidbody2D.AddForce(PlayerHelper.DEATH_FORCE);
            playerRigidbody2D.AddTorque(PlayerHelper.DEATH_TORQUE);
        }

        //LEFT
        else
        {
            playerRigidbody2D.AddForce(new Vector2(-PlayerHelper.DEATH_FORCE.x, PlayerHelper.DEATH_FORCE.y));
            playerRigidbody2D.AddTorque(-PlayerHelper.DEATH_TORQUE);
        }
    }
}
