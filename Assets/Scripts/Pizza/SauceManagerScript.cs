using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceManagerScript : MonoBehaviour
{

    public float maxSize = 0.1f; // Taille max de la sauce
    public float growSpeed = 0.1f; // Vitesse d'expansion de la sauce

    private Vector3 initialScale;
    private bool isActive = false;

    [SerializeField] private GameObject sauce;

    void OnParticleCollision(GameObject other)
    {
        if (sauce == null) return;

        if (!isActive)
        {
            isActive = true;
            sauce.transform.localScale = Vector3.one * 0.01f; // Rend la sauce visible avec une petite taille au début
            sauce.SetActive(true);
        }

        // Faire grandir la sauce progressivement
        if (sauce.transform.localScale.x < maxSize)
        {
            sauce.transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
        }
    }
}