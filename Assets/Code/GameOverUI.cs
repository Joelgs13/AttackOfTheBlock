using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        // Asegurarse que el panel está desactivado al inicio
        gameOverPanel.SetActive(false);

        // Asignar eventos a los botones
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Mostrar pantalla Game Over
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Reiniciar escena
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Salir del juego
    void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Para salir en editor
        #else
        Application.Quit();
        #endif
    }
}
