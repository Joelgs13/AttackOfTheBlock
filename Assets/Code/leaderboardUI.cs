using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardText;

    void Start()
    {
        ShowLeaderboard();
    }

    void ShowLeaderboard()
    {
        ScoreData data = ScoreData.Load();

        if (data.scores.Count == 0)
        {
            leaderboardText.text = "No scores yet!";
            return;
        }

        leaderboardText.text = "🏆 Top 5 Scores 🏆\n\n";

        for (int i = 0; i < data.scores.Count; i++)
        {
            float score = data.scores[i];
            int minutes = Mathf.FloorToInt(score / 60);
            int seconds = Mathf.FloorToInt(score % 60);
            int milliseconds = Mathf.FloorToInt((score * 1000) % 1000);

            leaderboardText.text += $"{i + 1}. {minutes:00}:{seconds:00}:{milliseconds:000}\n";
        }
    }
}