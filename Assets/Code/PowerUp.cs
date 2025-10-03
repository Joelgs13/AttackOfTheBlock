using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reducir velocidad de todos los enemigos
            EnemyBounce[] enemies = FindObjectsOfType<EnemyBounce>();
            foreach (EnemyBounce enemy in enemies)
            {
                enemy.StartCoroutine(enemy.ReduceSpeedTemporarily(0.5f, 5f));
            }

            // Avisar al spawner de que el efecto sigue activo
            FindFirstObjectByType<PowerUpSpawner>().StartCoroutine(EffectCooldown(5f));

            // Destruir power-up recogido
            Destroy(gameObject);
        }
    }

    private IEnumerator EffectCooldown(float duration)
    {
        FindFirstObjectByType<PowerUpSpawner>().SetPowerUpActive(true);
        yield return new WaitForSeconds(duration);
        FindFirstObjectByType<PowerUpSpawner>().SetPowerUpActive(false);
    }
}