using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private float currentTime = 0f;
    private bool isGameOver = false;

    private List<float> topScores = new List<float>(); // ranking
    private int nextTargetIndex = 0; // índice del TOP actual a superar

    private readonly string[] topNames = { "TOP 5", "TOP 4", "TOP 3", "TOP 2", "BEST" };

    void Start()
    {
        ResetTopScores();
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime += Time.deltaTime;
            UpdateScoreText();
        }
    }

    void ResetTopScores()
    {
        // Cargar ranking y ordenar de menor a mayor para mostrar TOP 5 primero
        topScores = new List<float>(ScoreData.Load().scores);
        topScores.Sort(); // menor tiempo = TOP más bajo
        while (topScores.Count < 5) topScores.Insert(0, float.MaxValue); // rellena hasta 5 para evitar errores
        nextTargetIndex = 0; // comenzamos con TOP 5
    }

    void UpdateScoreText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000);

        string currentStr = $"Tiempo: {minutes:00}:{seconds:00}:{milliseconds:000}\t";

        // Si ya superaste todos los TOP, solo mostramos tu tiempo
        if (nextTargetIndex >= topScores.Count)
        {
            scoreText.text = currentStr;
            return;
        }

        float targetScore = topScores[nextTargetIndex];

        // Si superaste la marca actual, pasamos a la siguiente
        if (currentTime > targetScore)
        {
            nextTargetIndex++;
            if (nextTargetIndex >= topScores.Count)
            {
                scoreText.text = currentStr;
                return;
            }
            targetScore = topScores[nextTargetIndex];
        }

        // Mostrar el TOP correspondiente
        int tMin = Mathf.FloorToInt(targetScore / 60);
        int tSec = Mathf.FloorToInt(targetScore % 60);
        int tMs = Mathf.FloorToInt((targetScore * 1000) % 1000);

        string topName = nextTargetIndex < topNames.Length ? topNames[nextTargetIndex] : "TOP";

        scoreText.text = currentStr + $"\t{topName}: {tMin:00}:{tSec:00}:{tMs:000}";
    }

    public void GameOver()
    {
        isGameOver = true;
        ScoreData.SaveScore(currentTime);
        ResetTopScores(); // recarga ranking
        UpdateScoreText();
    }

    public void ResetScore()
    {
        currentTime = 0f;
        isGameOver = false;
        ResetTopScores();
    }
}
