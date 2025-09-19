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

        // Opciones de fin de juego:
        // 1. Cerrar aplicación
        // Application.Quit();

        // 2. Recargar escena (reiniciar juego)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 3. O mostrar una UI de "Game Over" (Canvas con un panel)
    }
}
