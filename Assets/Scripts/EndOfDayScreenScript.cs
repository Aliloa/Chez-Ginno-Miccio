using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDayScreenScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowEndOfDayScreen(int score)
    {
        scoreText.text = "Score du jour : " + score;
        gameObject.SetActive(true); // Afficher l'écran de fin de journée
    }
}
