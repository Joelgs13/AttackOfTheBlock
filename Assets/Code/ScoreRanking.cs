using UnityEngine;
using TMPro;
using System.Linq;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankingText;

    void Start()
    {
        ShowRanking();
    }

    public static void SaveScore(float score)
    {
        float[] scores = new float[5];
        for (int i = 0; i < 5; i++)
            scores[i] = PlayerPrefs.GetFloat("BestScore" + i, 0f);

        scores = scores.Concat(new float[] { score })
            .OrderByDescending(s => s)
            .Take(5)
            .ToArray();

        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetFloat("BestScore" + i, scores[i]);

        PlayerPrefs.Save();
    }

    public void ShowRanking()
    {
        string text = "🏆 Top 5 Scores:\n";
        for (int i = 0; i < 5; i++)
        {
            float s = PlayerPrefs.GetFloat("BestScore" + i, 0f);
            if (s > 0)
                text += $"{i + 1}. {s:F2} sec\n";
        }
        rankingText.text = text;
    }
}