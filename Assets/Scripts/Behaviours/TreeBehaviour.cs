using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    private float speed;
    private bool createdNewTree = false;
    public bool shouldMove = false;
    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        ManageTrees();
    }

    private void Move()
    {
        if (shouldMove)
        {
            rigidbody2d.position += speed * Time.fixedDeltaTime * Vector2.up;
        }
    }

    private void ManageTrees()
    {
        if (transform.position.y >= TreeModulesManager.NEW_TREE_MODULE_INIT_POSITION.y
            && !createdNewTree)
        {
            TreeModulesManager.ManageTreeModules();
            createdNewTree = true;
        }
    }
}
