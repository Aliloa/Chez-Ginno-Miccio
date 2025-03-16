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

    public void EndDay()
    {
        currentDay++;

        // Change scenes for the next day
        string nextSceneName = "Day " + currentDay; // Name of the scene
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
