using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private Vector2 direction;
    private float leftBound, rightBound;
    [SerializeField] private float speed;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public void SetMovementArea(float left, float right)
    {
        leftBound = left;
        rightBound = right;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.x < leftBound - 1 || transform.position.x > rightBound + 1)
        {
            gameObject.SetActive(false);
        }
    }
}
