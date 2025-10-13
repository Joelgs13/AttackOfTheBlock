using System.Collections.Generic;
using UnityEngine;

// Clase para guardar y cargar el ranking
[System.Serializable]
public class ScoreData
{
    public List<float> scores = new List<float>();

    private const string Key = "TopScores";

    public static void SaveScore(float score)
    {
        ScoreData data = Load();

        // Añadir nueva puntuación
        data.scores.Add(score);

        // Ordenar de mayor a menor (más tiempo = mejor)
        data.scores.Sort((a, b) => b.CompareTo(a));

        // Limitar a 5 mejores
        if (data.scores.Count > 5)
            data.scores = data.scores.GetRange(0, 5);

        // Guardar como JSON
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }

    public static ScoreData Load()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            string json = PlayerPrefs.GetString(Key);
            return JsonUtility.FromJson<ScoreData>(json);
        }
        return new ScoreData();
    }
}