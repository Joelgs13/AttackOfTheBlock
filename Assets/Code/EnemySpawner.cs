using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private int maxEnemies = 12;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int count = FindObjectsByType<EnemyBounce>(FindObjectsSortMode.None).Length;
            if (count >= maxEnemies) continue;

            Vector3 pos = GetRandomPositionOnScreen();
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnScreen()
    {
        float x = Random.Range(0.1f, 0.9f);
        float y = Random.Range(0.1f, 0.9f);
        Vector3 world = cam.ViewportToWorldPoint(new Vector3(x, y, 10));
        world.z = 0f;
        return world;
    }
}