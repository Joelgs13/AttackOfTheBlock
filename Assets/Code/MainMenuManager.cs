using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Llamado por el botón "Iniciar Juego"
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Asegúrate de que el nombre coincide
    }

    // Llamado por el botón "Salir"
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Cierra en editor
#else
        Application.Quit(); // Cierra el ejecutable
#endif
    }
}