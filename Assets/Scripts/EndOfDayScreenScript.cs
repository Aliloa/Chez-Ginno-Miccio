using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDayScreenScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public GameObject player;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowEndOfDayScreen(int score)
    {
        scoreText.text = "Score du jour : " + score;
        gameObject.SetActive(true); // Afficher l'écran de fin de journée

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.GetComponent<FirstPersonController>().enabled = false;
    }

    public void ContinueToNextDay()
    {
        DayManagerScript.Instance.EndDay();

        // Réactiver le curseur verrouillé
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Désactiver l'écran de fin de journée
        gameObject.SetActive(false);

        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
