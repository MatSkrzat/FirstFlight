using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    private float speed;
    private bool createdNewBackground = false;
    public bool shouldMove = false;

    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        ManageBackgrounds();
    }

    private void Move()
    {
        if (shouldMove)
        {
            rigidbody2d.position += speed * Time.fixedDeltaTime * Vector2.up;
        }
    }

    private void ManageBackgrounds()
    {
        if (transform.position.y >= BackgroundsManager.NEW_BACKGROUND_INIT_POSITION.y
            && !createdNewBackground)
        {
            BackgroundsManager.ManageBackgrounds();
            createdNewBackground = true;
        }
    }
}
