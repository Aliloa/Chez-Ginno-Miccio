using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceManagerScript : MonoBehaviour
{

    public float maxSize = 0.1f; // Taille max de la sauce
    public float growSpeed = 0.1f; // Vitesse d'expansion de la sauce

    private Vector3 initialScale;
    GameObject sauce = null;

    void OnParticleCollision(GameObject other)
    {
        string particleTag = other.tag; // R�cup�re le tag du syst�me de particules

        foreach (Transform child in transform) // R�cup�rer le child sauce dans dough
        {
            if (child.CompareTag(particleTag))
            {
                sauce = child.gameObject;
                break;
            }
        }

        if (sauce != null && sauce.CompareTag(particleTag))
        {
            if (sauce.transform.localScale.x < maxSize)  // Faire grossir la sauce
            {
                sauce.transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("Aucun enfant trouv� avec le tag : " + particleTag);
        }

    }
}