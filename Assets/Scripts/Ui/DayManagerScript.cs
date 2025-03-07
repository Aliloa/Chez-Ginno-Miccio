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

    public void EndDay()
    {
        Debug.Log("Fin du jour " + currentDay);

        // Incrémenter le jour
        currentDay++;

        // Charger la scène du jour suivant
        string nextSceneName = "Day " + currentDay; // Nom de la prochaine scène, ex. : "Day 2"
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Aucune scène trouvée pour le jour suivant !");
        }
    }
}
