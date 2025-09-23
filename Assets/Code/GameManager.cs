using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Central game controller.
* Handles game over state and notifies other managers (Score & UI).
*/
public class GameManager : MonoBehaviour
{
    private bool isGameOver = false; // Prevents multiple game over calls

    // Called when the player loses
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Game Over!");

        // Notify other systems
        FindFirstObjectByType<ScoreManager>().GameOver();
        FindFirstObjectByType<GameOverUI>().ShowGameOver();
    }
}
