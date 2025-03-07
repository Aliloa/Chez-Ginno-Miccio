using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManagerScript : MonoBehaviour
{
    public static DayManagerScript Instance;
    public int currentDay = 0;

    private void Awake()
    {
        // Cr�er une instance unique qui persiste entre les sc�nes
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

    public void EndDay()
    {
        Debug.Log("Fin du jour " + currentDay);

        // Incr�menter le jour
        currentDay++;

        // Charger la sc�ne du jour suivant
        string nextSceneName = "Day " + currentDay; // Nom de la prochaine sc�ne, ex. : "Day 2"
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Aucune sc�ne trouv�e pour le jour suivant !");
        }
    }
}
