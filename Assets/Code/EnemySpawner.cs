using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 15f; // Tiempo entre spawns
    [SerializeField] private int maxEnemies = 20;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        TrySpawnEnemy();
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        if (FindObjectsOfType<EnemyBounce>().Length >= maxEnemies)
            return;

        Vector2 spawnPos = GetRandomSpawnPosition();
        EnemyBounce enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<EnemyBounce>();

        // Evitar daño al jugador durante 2 segundos
        if (enemy != null)
            enemy.DisableDamageTemporarily(0.5f);
    }


    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        return new Vector2(
            Random.Range(-screenBounds.x * 0.8f, screenBounds.x * 0.8f),
            Random.Range(-screenBounds.y * 0.8f, screenBounds.y * 0.8f)
        );
    }
}