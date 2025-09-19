using UnityEngine;
using UnityEngine.InputSystem; // Necesario para Mouse.current

public class PlayerFollowMouse : MonoBehaviour
{
    private Vector3 targetPosition;

    [SerializeField] private float moveSpeed = 10f;

    void Update()
    {
        // Leer la posición del ratón con el nuevo Input System
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.z = 0f;

        // Mover al jugador hacia la posición del ratón
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Detecta colisión con un enemigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("GAME OVER!");
            // Llama al GameManager para finalizar el juego
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }
}
