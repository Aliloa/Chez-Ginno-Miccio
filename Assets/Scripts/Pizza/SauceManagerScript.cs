using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceManagerScript : MonoBehaviour
{

    [SerializeField] private float maxSize = 0.1f;
    [SerializeField] private float growSpeed = 0.1f; // Sauce expansion speed

    private Vector3 initialScale;
    GameObject sauce = null;

    void OnParticleCollision(GameObject other)
    {
        string particleTag = other.tag; // Particles system tag

        foreach (Transform child in transform) // Get the child sauce from dough
        {
            if (child.CompareTag(particleTag))
            {
                sauce = child.gameObject;
                break;
            }
        }

        if (sauce != null && sauce.CompareTag(particleTag))
        {
            if (sauce.transform.localScale.x < maxSize)  // Make sauce grow
            {
                sauce.transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("Aucun enfant trouvé avec le tag : " + particleTag);
        }

    }
}