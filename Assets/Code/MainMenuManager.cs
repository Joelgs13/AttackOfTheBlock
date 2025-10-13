using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Handles the Main Menu functionality: starting the game or quitting.
 * Also ensures the mouse cursor is visible when in the menu.
 */
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        // Mostrar el cursor al estar en el menú
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Asignar eventos a los botones
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        // Ocultar el cursor al iniciar el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("GameScene");
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Solo en editor
#else
        Application.Quit(); // En el ejecutable
#endif
    }
}