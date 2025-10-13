using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private bool gameOverActive = false;

    void Awake()
    {
        gameOverPanel.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverActive = true;

        Time.timeScale = 0f;

        // Mostrar cursor al morir
        Cursor.visible = true;
    }

    public bool IsGameOverActive()
    {
        return gameOverActive;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ReturnToMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}