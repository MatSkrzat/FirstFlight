using UnityEngine;

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
        if (transform.position.y >= TreeManager.NEW_TREE_MODULE_INIT_POSITION.y
            && !createdNewTree)
        {
            TreeManager.ManageTreeModules();
            createdNewTree = true;
        }
    }
}
