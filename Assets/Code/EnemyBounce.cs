using System.Collections;
using UnityEngine;

/*
 * EnemyBounce: rebote, flip, límites de pantalla, reducción temporal de velocidad
 * y fusión al chocar con otros enemigos.
 */
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;               // base inicial
    [SerializeField] private float maxSpeed = 100f;          // tope de velocidad

    [Header("Merge")]
    [SerializeField] private float mergeScaleMultiplier = 1.5f;   // 50% más grande => x1.5
    [SerializeField] private float mergeSpeedMultiplier = 0.7f;   // velocidad al fusionar => 70%
    [SerializeField] private float mergeCooldown = 0.08f;         // breve cooldown para evitar fusiones dobles
    [SerializeField] private float maxScale = 3.5f;               // tope de escala para no desbordar

    private bool isMerging = false;
    private float currentBaseSpeed; // velocidad base actual (se reduce al fusionar)
    private bool canDamagePlayer = true; // por defecto true
    public bool CanDamagePlayer() => canDamagePlayer;

// Llama a esto al spawnear
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Rigidbody2D defaults
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
        // Flip horizontal según velocidad X
        if (rb.linearVelocity.x > 0.01f) spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.01f) spriteRenderer.flipX = true;

        // Clamp de velocidad para evitar túnel físico
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Protección: si por alguna razón la velocidad es 0, reasigna una pequeña dirección
        if (rb.linearVelocity.sqrMagnitude < 0.001f)
            rb.linearVelocity = Random.insideUnitCircle.normalized * currentBaseSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reproducir sonido de rebote
        AudioManager.Instance.PlaySFX(AudioManager.Instance.bounceClip);

        // Fusión con otro enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBounce other = collision.gameObject.GetComponent<EnemyBounce>();
            if (other != null && other != this)
            {
                // Fusiona tamaño y reduce velocidad
                transform.localScale *= 1.5f;
                rb.linearVelocity *= 0.7f;

                Destroy(other.gameObject);
            }
        }
    }


    private IEnumerator AbsorbCoroutine(EnemyBounce victim)
    {
        if (victim == null) yield break;

        // marcar ambos como en fusión para evitar reentradas
        isMerging = true;
        victim.isMerging = true;

        // breve pausa para estabilizar
        yield return new WaitForSeconds(0.02f);

        // Nueva escala (clamp)
        float newScale = Mathf.Min(maxScale, transform.localScale.x * mergeScaleMultiplier);
        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);

        // Reducir velocidad base y aplicar al rigidbody manteniendo dirección
        currentBaseSpeed = Mathf.Max(0.05f, currentBaseSpeed * mergeSpeedMultiplier);

        Vector2 currentDir = rb.linearVelocity.normalized;
        if (currentDir.sqrMagnitude < 0.0001f) currentDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = currentDir * currentBaseSpeed;

        // Opcional: efectos visuales aquí (partículas, pop, etc.)

        // destruir al absorbido
        Destroy(victim.gameObject);

        // cooldown para evitar nuevas fusiones inmediatas
        yield return new WaitForSeconds(mergeCooldown);

        isMerging = false;
    }

    // Mantengo tu método para reducir velocidad temporal
    public IEnumerator ReduceSpeedTemporarily(float multiplier, float duration)
    {
        Vector2 dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;

        rb.linearVelocity = dir * (speed * multiplier);

        yield return new WaitForSeconds(duration);

        dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = dir * (speed * 2f); // tal como tenías antes
    }

    // Método público para matar el enemigo (powerup kill)
    public void Kill()
    {
        // Aquí efectos/sonidos si quieres
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        // Limitar dentro de la cámara (tu código original)
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
