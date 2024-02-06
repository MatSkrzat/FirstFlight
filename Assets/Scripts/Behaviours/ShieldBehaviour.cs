using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    ShieldAnimations shieldAnimations;

    private void Start()
    {
        shieldAnimations = gameObject.GetComponent<ShieldAnimations>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.IsDead) return;

        switch (collision.tag)
        {
            case TagsDictionary.BRANCH:
                shieldAnimations.PlayHit();
                TreeManager.BreakModuleBranch(collision.gameObject);
                break;
        }
    }
}
