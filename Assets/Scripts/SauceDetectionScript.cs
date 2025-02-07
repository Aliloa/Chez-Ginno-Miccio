using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceDetectionScript : MonoBehaviour
{
    private Vector3 initialScale;
    private float scaleIncrease = 0.05f;

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("sauce touche qq chose?");
        if (other.CompareTag("Dough")) // Vérifie si la pâte est touchée
        {
            Debug.Log("sauce touche la pate");
            // Trouve l'enfant avec le nom "TomatoSauce"
            Transform sauceOnDough = other.transform.Find("TomatoSauce");
            if (sauceOnDough == null)
            {
                Debug.LogWarning("L'enfant 'TomatoSauce' est introuvable !");
                return;
            }

            // Agrandit le GameObject "sauce" pour simuler le liquide qui coule
            Vector3 newScale = sauceOnDough.localScale + new Vector3(scaleIncrease, scaleIncrease, 0);
            sauceOnDough.localScale = newScale; // Applique l'agrandissement
        }
    }
}
