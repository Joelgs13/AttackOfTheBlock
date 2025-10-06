using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/*
* This script manages the Game Over screen.
* It shows the Game Over panel and handles the Restart and Quit buttons.
*/
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Panel that contains the Game Over UI
    [SerializeField] private Button restartButton;     // Restart button reference
    [SerializeField] private Button quitButton;        // Quit button reference

    void Awake()
    {
        
        // Ensure the panel is hidden at the start
        gameOverPanel.SetActive(false);

        // Assign button actions
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Show the Game Over screen
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Reload the current scene
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit the game (works in Editor and build)
    void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
