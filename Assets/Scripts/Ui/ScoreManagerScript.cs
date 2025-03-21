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
        // Create a single instance that persists between scenes
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
        // Add today's score to the score list
        dailyScores.Add(dailyScore);
        for (int i = 0; i < dailyScores.Count; i++)
        {
            Debug.Log("Jour " + (i + 1) + " : " + dailyScores[i] + " points");
        }
        endOfDayScreen.GetComponent<EndOfDayScreenScript>().ShowEndOfDayScreen(dailyScore);

        dailyScore = 0; // Reset today's score for the next day
    }

    public int GetScoreForDay(int day)
    {
       return dailyScores[day - 1]; // Returns the score of the requested day
    }
}
