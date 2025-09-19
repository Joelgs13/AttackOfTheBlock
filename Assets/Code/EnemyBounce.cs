using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5f; // Velocidad inicial

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Dirección aleatoria inicial
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * speed;
    }

    // Rebotes automáticos gracias al PhysicsMaterial2D
}
