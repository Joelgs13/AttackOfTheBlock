using UnityEngine;
using System.Collections;

public class PowerUpKillSpawner : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float spawnInterval = 30f; // tiempo máximo entre powerups

    private bool isPowerUpActive = false;
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
            // espera un tiempo aleatorio entre 10 y 30 segundos
            float waitTime = Random.Range(10f, spawnInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isPowerUpActive)
            {
                SpawnPowerUp();
            }
        }
    }

    void SpawnPowerUp()
    {
        isPowerUpActive = true;
        Vector3 pos = GetRandomPositionOnScreen();
        GameObject p = Instantiate(powerUpPrefab, pos, Quaternion.identity);
        PowerUpKill kill = p.GetComponent<PowerUpKill>();
        if (kill != null) kill.OnDestroyed += () => isPowerUpActive = false;
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