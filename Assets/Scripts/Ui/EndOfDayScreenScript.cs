using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDayScreenScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject player;

    void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowEndOfDayScreen(int score)
    {
        scoreText.text = "Score du jour : " + score;
        gameObject.SetActive(true); // Show end of day screen

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.GetComponent<FirstPersonController>().enabled = false;
    }

    public void ContinueToNextDay()
    {
        DayManagerScript.Instance.EndDay();

        // Reactivate the locked cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(false);

        player.GetComponent<FirstPersonController>().enabled = true;
    }
}
