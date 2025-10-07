using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reduce all enemies Vel
            EnemyBounce[] enemies = FindObjectsByType<EnemyBounce>(FindObjectsSortMode.None);
            foreach (EnemyBounce enemy in enemies)
            {
                enemy.StartCoroutine(enemy.ReduceSpeedTemporarily(0.5f, 5f));
            }

            // Call spawner so it knows the effect is still lasting
            FindFirstObjectByType<PowerUpSpawner>().StartCoroutine(EffectCooldown(5f));

            // Destroy Power-up
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