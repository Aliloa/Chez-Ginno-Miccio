using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript Instance;
    [SerializeField] private GameObject endOfDayScreen;

    public int dailyScore = 0;

    public List<int> dailyScores = new List<int>();

    private void Awake()
    {
        // Créer une instance unique qui persiste entre les scènes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        dailyScore += points;
    }
    public void EndDayScore()
    {
        // Ajouter le score du jour à la liste des scores
        dailyScores.Add(dailyScore);
        for (int i = 0; i < dailyScores.Count; i++)
        {
            Debug.Log("Jour " + (i + 1) + " : " + dailyScores[i] + " points");
        }
        endOfDayScreen.GetComponent<EndOfDayScreenScript>().ShowEndOfDayScreen(dailyScore);

        dailyScore = 0; // Réinitialiser le score du jour pour le prochain jour
    }

    public int GetScoreForDay(int day)
    {
       return dailyScores[day - 1]; // Retourne le score du jour demandé (1 = Jour 1)
    }
}
