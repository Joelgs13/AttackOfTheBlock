using UnityEngine;
using System;

public class PowerUpKill : MonoBehaviour
{
    [SerializeField] private float lifeTime = 6f; // cuánto dura en escena
    public Action OnDestroyed;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPowerup player = other.GetComponent<PlayerPowerup>();
            if (player != null)
            {
                player.GiveKillPowerUp();
            }

            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}