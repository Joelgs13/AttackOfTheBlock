using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float minSpawnTime = 10f; // Min time between spawns
    [SerializeField] private float maxSpawnTime = 30f; // Max time between spawns
    [SerializeField] private float powerUpLifetime = 2f; // Max visibility lifetime

    private bool powerUpActive = false; // evade duplicated spawn

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Random time between spawns
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (!powerUpActive)
            {
                // Create power-up
                Vector3 randomPos = GetRandomPositionInCamera();
                GameObject powerUp = Instantiate(powerUpPrefab, randomPos, Quaternion.identity);

                powerUpActive = true;

                // Destroy after X seconds
                Destroy(powerUp, powerUpLifetime);

                // Wait until actual Power-up dissapears before spawning other one
                yield return new WaitForSeconds(powerUpLifetime);

                powerUpActive = false;
            }
        }
    }

    private Vector3 GetRandomPositionInCamera()
    {
        Camera cam = Camera.main;
        float x = Random.Range(cam.ViewportToWorldPoint(new Vector3(0.1f, 0, 0)).x,
            cam.ViewportToWorldPoint(new Vector3(0.9f, 0, 0)).x);
        float y = Random.Range(cam.ViewportToWorldPoint(new Vector3(0, 0.1f, 0)).y,
            cam.ViewportToWorldPoint(new Vector3(0, 0.9f, 0)).y);

        return new Vector3(x, y, 0f);
    }
    
    public void SetPowerUpActive(bool active)
    {
        powerUpActive = active;
    }

}