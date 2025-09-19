using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private float currentTime = 0f;
    private float bestTime = 0f;
    private bool isGameOver = false;

    void Start()
{
    // Cargar mejor tiempo guardado al iniciar
    bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
    UpdateScoreText(); // Para mostrar desde el inicio
}

public void GameOver()
{
    isGameOver = true;

    // Guardar mejor tiempo solo si el actual es mayor
    if (currentTime > bestTime)
    {
        bestTime = currentTime;
        PlayerPrefs.SetFloat("BestTime", bestTime);
        PlayerPrefs.Save(); // OBLIGATORIO para persistencia
    }

    UpdateScoreText(); // Actualizar texto final
}


    void Update()
    {
        if (!isGameOver)
        {
            currentTime += Time.deltaTime;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        // Convertir tiempo actual a min:seg:miliseg
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000);

        int bestMinutes = Mathf.FloorToInt(bestTime / 60);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60);
        int bestMilliseconds = Mathf.FloorToInt((bestTime * 1000) % 1000);

        scoreText.text = $"Tiempo: {minutes:00}:{seconds:00}:{milliseconds:000}   Mejor: {bestMinutes:00}:{bestSeconds:00}:{bestMilliseconds:000}";
    }

    

    public void ResetScore()
    {
        currentTime = 0f;
        isGameOver = false;
    }
}
