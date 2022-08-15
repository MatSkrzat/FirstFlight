using UnityEngine;

public class InitialTreeBehaviour : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
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

    private void Move(float speed)
    {
        rigidbody2d.position += speed * Time.fixedDeltaTime * Vector2.up;
    }

    private void DestroyIfOutOfCamera() 
    { 
        if(rigidbody2d.position.y > 10f)
        {
            Destroy(gameObject);
        }
    }
}
