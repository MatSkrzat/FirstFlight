using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    private float speed;
    private bool createdNewBackground = false;
    public bool shouldMove = false;
    public void ChangeSpeed(float newSpeed)
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
            transform.Translate(Vector2.up * Time.fixedDeltaTime * speed);
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
