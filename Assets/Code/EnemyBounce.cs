using System.Collections;
using UnityEngine;

/*
 * it's controlling the enemy's rotation on animation and the initial direction and the collision detection
 */
public class EnemyBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 5f; // initial speed
    [SerializeField] private float maxSpeed = 100f; // Velocidad máxima

    private float originalSpeed;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("bump");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.bounceClip);
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Random direction at start
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * speed;
    }
    
    void Update()
    {
        // Flip sprite depending on horizontal movement
        if (rb.linearVelocity.x > 0.01f)
        {
            spriteRenderer.flipX = false; // facing right
        }
        else if (rb.linearVelocity.x < -0.01f)
        {
            spriteRenderer.flipX = true;  // facing left
        }
        //vel controller
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public IEnumerator ReduceSpeedTemporarily(float multiplier, float duration)
    {
        // Guardamos la dirección actual antes de reducir
        Vector2 currentDir = rb.linearVelocity.normalized;

        // Reducir velocidad actual (manteniendo dirección)
        rb.linearVelocity = currentDir * (speed * multiplier);

        yield return new WaitForSeconds(duration);

        // Restaurar con el doble de la velocidad original (no la reducida)
        currentDir = rb.linearVelocity.normalized;
        rb.linearVelocity = currentDir * (speed * 2f);
    }
    void LateUpdate()
    {
        // Obtener límites de cámara en coordenadas del mundo
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector3 pos = transform.position;

        // Limitar X
        if (pos.x > screenBounds.x)
        {
            pos.x = screenBounds.x;
            rb.linearVelocity = new Vector2(-Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);
        }
        else if (pos.x < -screenBounds.x)
        {
            pos.x = -screenBounds.x;
            rb.linearVelocity = new Vector2(Mathf.Abs(rb.linearVelocity.x), rb.linearVelocity.y);
        }

        // Limitar Y
        if (pos.y > screenBounds.y)
        {
            pos.y = screenBounds.y;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -Mathf.Abs(rb.linearVelocity.y));
        }
        else if (pos.y < -screenBounds.y)
        {
            pos.y = -screenBounds.y;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Abs(rb.linearVelocity.y));
        }

        transform.position = pos;
    }

    


    // NOTE: PhysicsMaterial2D handles the bounce automatically
}