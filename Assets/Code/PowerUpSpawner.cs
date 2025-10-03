using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float minSpawnTime = 10f; // tiempo mínimo entre spawns
    [SerializeField] private float maxSpawnTime = 30f; // tiempo máximo entre spawns
    [SerializeField] private float powerUpLifetime = 2f; // duración visible del objeto

    private bool powerUpActive = false; // evita spawn duplicado

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // esperar tiempo aleatorio antes de intentar spawn
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (!powerUpActive)
            {
                // Crear power-up
                Vector3 randomPos = GetRandomPositionInCamera();
                GameObject powerUp = Instantiate(powerUpPrefab, randomPos, Quaternion.identity);

                powerUpActive = true;

                // Destruir tras X segundos si no se recoge
                Destroy(powerUp, powerUpLifetime);

                // esperar a que el power-up desaparezca antes de permitir otro
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