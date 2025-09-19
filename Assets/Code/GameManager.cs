using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Has perdido el juego!");

        FindFirstObjectByType<ScoreManager>().GameOver();
        FindFirstObjectByType<GameOverUI>().ShowGameOver();
    }
}
