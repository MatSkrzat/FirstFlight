using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    public void ExplodeAtPosition(Vector2 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.IsDead) return;

        switch (collision.tag)
        {
            case TagsDictionary.BRANCH:
                TreeManager.BreakModuleBranch(collision.gameObject);
                break;
        }
    }
}
