using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Central game controller.
* Handles game over state and notifies other managers (Score & UI).
*/
public class GameManager : MonoBehaviour
{
    private bool isGameOver = false; // Prevents multiple game over calls

    void Start()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.startClip);
    }
    
    // Called when the player loses
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Game Over!");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);

        // Notify other systems
        FindFirstObjectByType<ScoreManager>().GameOver();
        FindFirstObjectByType<GameOverUI>().ShowGameOver();
    }
}
