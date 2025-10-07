using UnityEngine;
using UnityEngine.InputSystem; 

/*
* Controls player movement, life system and temporary invulnerability.
* The player follows the mouse, has 3 lives, and becomes invulnerable (no collisions)
* for a short period after being hit by an enemy.
*/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFollowMouse : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;       // Movement speed
    [SerializeField] private int maxLives = 3;            // Total lives
    [SerializeField] private float invulnerabilityTime = 2f; // Duration of invulnerability

    private PlayerAnimation playerAnim;
    
    private int currentLives;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    private int playerLayer;
    private int enemyLayer;

    void Awake()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;

        currentLives = maxLives;

        // Store layer indices (make sure the layers exist in Unity)
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        // Read mouse position and convert to world space
        Vector3 mousePos = Mouse.current.position.ReadValue();
        targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.z = 0f;
    }

    void FixedUpdate()
    {
        // Move player smoothly toward the target
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isInvulnerable) return;
            

            currentLives--;
            Debug.Log($"Player hit! Lives remaining: {currentLives}");

            AudioManager.Instance.PlaySFX(AudioManager.Instance.hitClip);

            if (currentLives > 0)
            {
                playerAnim.PlayHurt();
                StartCoroutine(InvulnerabilityRoutine());
            }
            else
            {
                playerAnim.PlayDead();
                Debug.Log("GAME OVER!");
                FindFirstObjectByType<GameManager>().GameOver();
            }
        }
    }

    private System.Collections.IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        // Disable collisions between Player and Enemy layers
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        // Optional blink effect
        float blinkInterval = 0.2f;
        for (float t = 0; t < invulnerabilityTime; t += blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Restore everything
        spriteRenderer.enabled = true;
        isInvulnerable = false;

        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}
