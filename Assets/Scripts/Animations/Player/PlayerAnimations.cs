using Assets.Scripts.Animations;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    const string JUMP_ANIM = "jump";
    const string HIT_ANIM = "hit";

    private static Rigidbody2D playerRigidbody2D;
    private static AnimationController animationController;
    private static ParticleSystem particle;

    private void Start()
    {
        var selectedCharacterId = GameStateManager.CurrentGameState.selectedCharacterId;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animationController = new AnimationController(gameObject.transform.GetChild(selectedCharacterId).GetComponent<Animator>());
        particle = GetComponent<ParticleSystem>();
    }

    public static void PlayJump()
    {
        animationController.Play(JUMP_ANIM);
    }
    
    public static void PlayHit()
    {
        animationController.Play(HIT_ANIM);
        if(particle != null)
            particle.Play();
    }

    public static void PlayDeath()
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
