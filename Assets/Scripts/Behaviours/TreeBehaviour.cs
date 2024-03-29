﻿using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public int moduleId = 0;
    private float speed;
    private bool createdNewTree = false;
    public bool shouldMove = false;
    private Rigidbody2D rigidbody2d;

    public float Speed { get { return speed; } }

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void StartMoving()
    {
        shouldMove = true;
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ActivateBonusLabel()
    {
        transform.GetChild((int)TreeModuleChildren.bonusCanvas).gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        ManageTrees();
    }

    private void Move()
    {
        if (shouldMove)
        {
            rigidbody2d.MovePosition((Vector2)transform.position + speed * Time.fixedDeltaTime * Vector2.up);
        }
    }

    private void ManageTrees()
    {
        if (transform.position.y >= TreeManager.NEW_TREE_MODULE_INIT_POSITION.y
            && !createdNewTree)
        {
            TreeManager.ManageTreeModules();
            createdNewTree = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagsDictionary.BOTTOM_COLLIDER)
        {
            if (PlayerManager.IsDead)
            {
                return;
            }
            if (LevelsManager.currentLevel.ID != GameManager.FirstSelectedLevel
                && !GameManager.IsGameRandom)
            {
                if (transform.GetChild((int)TreeModuleChildren.levelCanvas).gameObject.activeSelf == true)
                {
                    GameManager.SM.PlaySingleSound(GameManager.SM.LevelUp);
                    GameManager.UI.PlayConfettiParticles();
                }
            }

            if (GameManager.IsGameRandom)
            {
                ScoreManager.AddOneScorePoint();
            }
        }
    }
}
