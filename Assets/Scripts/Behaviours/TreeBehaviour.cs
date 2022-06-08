using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private float speed;
    private bool createdNewTree = false;
    public bool shouldMove = false;
    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    void FixedUpdate()
    {
        Move();
        ManageTrees();
    }
    void Move()
    {
        if (shouldMove)
        {
            transform.Translate(Vector2.up * Time.fixedDeltaTime * speed);
        }
    }
    void ManageTrees()
    {
        if (transform.position.y >= TreeModulesManager.NEW_TREE_MODULE_INIT_POSITION.y
            && !createdNewTree)
        {
            TreeModulesManager.ManageTreeModules();
            createdNewTree = true;
        }
    }
}
