using UnityEngine;
using TMPro;

/*
* Este script basa su funcionalidad en contabilizar la puntuación del jugador. 
* sirve para controlar como se actualiza la puntuacion en los diferentes momentos del juego
*/
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private float currentTime = 0f; //current points
    private float bestTime = 0f;//the best mark
    private bool isGameOver = false;//checks if game finished

    void Start()
    {
        // Chargest the best time at the start of the game
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        UpdateScoreText();
    }

    /*
    * method that is called at the end of the game, so it can 
    * SAVE the time, check if its better that the current best mark and save it. also updates the current mark
    */
    public void GameOver()
    {
        isGameOver = true;

        // saves only if current mark bypasses the current best
        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save(); // this saves on current player mark
        }

        UpdateScoreText(); // Updates final text
    }


    /*
    * Update is called once per frame.
    * If the game is not over, the timer (score) increases 
    * and the UI is updated to reflect the current time.
    */
    void Update()
    {
        if (!isGameOver)
        {
            currentTime += Time.deltaTime;
            UpdateScoreText();
        }
    }

    /*
    * Converts both the current time and the best time into
    * minutes:seconds:milliseconds format, then updates the UI text.
    */
    void UpdateScoreText()
    {
        // Convert current time
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000);

        // Convert best time
        int bestMinutes = Mathf.FloorToInt(bestTime / 60);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60);
        int bestMilliseconds = Mathf.FloorToInt((bestTime * 1000) % 1000);

        // Update the UI text with formatted values
        scoreText.text = $"Tiempo: {minutes:00}:{seconds:00}:{milliseconds:000}   Mejor: {bestMinutes:00}:{bestSeconds:00}:{bestMilliseconds:000}";
    }

    /*
    * Resets the score when starting a new game.
    * Sets the current time back to zero and reactivates score counting.
    */
    public void ResetScore()
    {
        currentTime = 0f;
        isGameOver = false;
    }
}