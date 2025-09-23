using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f; // start speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        // aleatory direction at start
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * speed;
    }

    //NOTE: the PhisicMaterial2D makes the bounce automatically
}
