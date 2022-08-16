using UnityEngine;

public class InitialTreeBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    private readonly float DESTROY_POSITION_Y = 15F;
    private bool shouldMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move(LevelsManager.currentLevel.endSpeed);
        DestroyIfOutOfCamera();
    }

    public void StartMoving()
    {
        shouldMove = true;
    }

    private void Move(float speed)
    {
        if(shouldMove)
            rigidbody2d.position += speed * Time.fixedDeltaTime * Vector2.up;
    }

    private void DestroyIfOutOfCamera() 
    { 
        if(rigidbody2d.position.y > DESTROY_POSITION_Y)
        {
            Destroy(gameObject);
        }
    }
}
