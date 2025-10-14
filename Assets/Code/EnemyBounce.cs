using System.Collections;
using UnityEngine;

/*
 * EnemyBounce:
 *  - Rebota en los bordes de la cámara.
 *  - Puede fusionarse con otros enemigos (crece y reduce su velocidad).
 *  - No puede dañar al jugador inmediatamente tras spawnear.
 *  - Puede ser destruido por un power-up de "kill".
 *  - Compatible con el power-up de ralentización.
 */
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeed = 100f;

    [Header("Fusión")]
    [SerializeField] private float mergeScaleMultiplier = 1.5f; // +50% tamaño
    [SerializeField] private float mergeSpeedMultiplier = 0.7f; // -30% velocidad
    [SerializeField] private float mergeCooldown = 0.1f;
    [SerializeField] private float maxScale = 3.5f;

    private float currentBaseSpeed;
    private bool isMerging = false;

    // Evitar daño inmediato tras spawn
    private bool canDamagePlayer = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentBaseSpeed = speed;

        // Dirección aleatoria inicial
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * currentBaseSpeed;
    }

    void Update()
    {
        // Flip horizontal según dirección X
        if (rb.linearVelocity.x > 0.01f) spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.01f) spriteRenderer.flipX = true;

        // Clamp velocidad máxima
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Evitar que se quede completamente quieto
        if (rb.linearVelocity.sqrMagnitude < 0.001f)
            rb.linearVelocity = Random.insideUnitCircle.normalized * currentBaseSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.bounceClip);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBounce other = collision.gameObject.GetComponent<EnemyBounce>();
            if (other != null && !isMerging && !other.isMerging)
            {
                StartCoroutine(AbsorbCoroutine(other));
            }
        }
    }

    private IEnumerator AbsorbCoroutine(EnemyBounce victim)
    {
        if (victim == null) yield break;

        isMerging = true;
        victim.isMerging = true;

        yield return new WaitForSeconds(0.02f); // pequeña pausa

        // Aumentar escala con límite
        float newScale = Mathf.Min(maxScale, transform.localScale.x * mergeScaleMultiplier);
        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);

        // Reducir velocidad base
        currentBaseSpeed = Mathf.Max(0.05f, currentBaseSpeed * mergeSpeedMultiplier);

        // Mantener dirección actual
        Vector2 dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = dir * currentBaseSpeed;

        Destroy(victim.gameObject);

        yield return new WaitForSeconds(mergeCooldown);
        isMerging = false;
    }

    // 🔹 Power-up de reducción temporal de velocidad
    public IEnumerator ReduceSpeedTemporarily(float multiplier, float duration)
    {
        Vector2 dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.0001f)
            dir = Random.insideUnitCircle.normalized;

        // Reducir velocidad actual
        rb.linearVelocity = dir * (currentBaseSpeed * multiplier);

        yield return new WaitForSeconds(duration);

        // Restaurar a velocidad original (x2 como tenías antes)
        dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.0001f)
            dir = Random.insideUnitCircle.normalized;

        rb.linearVelocity = dir * (currentBaseSpeed * 2f);
    }

    // Método para matar al enemigo (por power-up)
    public void Kill()
    {
        Destroy(gameObject);
    }

    // Método para hacer que el enemigo no pueda dañar al jugador temporalmente
    public void DisableDamageTemporarily(float duration)
    {
        StartCoroutine(DisableDamageRoutine(duration));
    }

    private IEnumerator DisableDamageRoutine(float duration)
    {
        canDamagePlayer = false;
        yield return new WaitForSeconds(duration);
        canDamagePlayer = true;
    }

    public bool CanDamagePlayer()
    {
        return canDamagePlayer;
    }

    // Mantener dentro de los límites de la pantalla
    void LateUpdate()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 pos = transform.position;

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
}
