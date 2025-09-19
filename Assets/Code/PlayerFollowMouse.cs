using UnityEngine;
using UnityEngine.InputSystem; // Necesario para Mouse.current

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFollowMouse : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private Vector3 targetPosition;

    void Awake()
    {
        // Obtenemos la referencia al Rigidbody2D del jugador
        rb = GetComponent<Rigidbody2D>();

        // Configuración básica del Rigidbody2D para movimiento 2D libre
        rb.gravityScale = 0;                 // Sin gravedad
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Mejor detección
        rb.freezeRotation = true;            // No queremos que rote al chocar
    }

    void Update()
    {
        // Leer la posición del ratón con el nuevo Input System
        Vector3 mousePos = Mouse.current.position.ReadValue();
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.z = 0f;
    }

    void FixedUpdate()
    {
        // Mover al jugador usando Rigidbody2D (respetará colisiones con muros)
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    // Detecta colisión con un enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("GAME OVER!");
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }
}

