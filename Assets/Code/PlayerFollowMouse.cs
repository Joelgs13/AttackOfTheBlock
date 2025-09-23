using UnityEngine;
using UnityEngine.InputSystem; // Required for Mouse.current

/*
* This script controls the player movement. 
* The player follows the mouse position on the screen, 
* while respecting collisions with invisible walls and enemies. 
*/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFollowMouse : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Speed at which the player moves toward the mouse

    private Rigidbody2D rb;            // Reference to the Rigidbody2D for physics-based movement
    private Vector3 targetPosition;    // Target position in world space based on the mouse

    void Awake()
    {
        // Get a reference to the player's Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Basic Rigidbody2D setup for smooth 2D movement
        rb.gravityScale = 0;                 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; 
        rb.freezeRotation = true;            
    }

    /*
    * Update is called once per frame.
    * Reads the current mouse position and converts it to world coordinates
    * so the player knows where to move.
    */
    void Update()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePos = Mouse.current.position.ReadValue();

        // Convert mouse position to world space
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.z = 0f; 
    }

    /*
    * FixedUpdate is used for physics-based movement.
    * Moves the Rigidbody2D smoothly towards the target position,
    * respecting collisions with walls or enemies.
    */
    void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    /*
    * Called when the player collides with another object.
    * If the object has the "Enemy" tag, the game ends.
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("GAME OVER!");
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }
}
