using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingScript : MonoBehaviour
{
    [SerializeField] private GameObject dough;
    [SerializeField] private Material cookedDoughMaterial;
    [SerializeField] private Material burntDoughMaterial;

    private bool isCooking = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dough"))
        {
            isCooking = true;
            Debug.Log("Pizza en train de cuir");
            StartCoroutine(CookPizza(collision.gameObject));
        } 
        else if (collision.gameObject.CompareTag("CookedDough"))
        {
            isCooking = true;
            Debug.Log("Pizza en train de bruler");
            StartCoroutine(BurnPizza(collision.gameObject));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dough") || collision.gameObject.CompareTag("CookedDough") || collision.gameObject.CompareTag("BurntDough"))
        {
            isCooking = false;
            Debug.Log("Pizza sortie du four !");
        }
    }
    private IEnumerator CookPizza(GameObject doughInstance) //IEnumerator function that takes time to execute without blocking others
    {
        Rigidbody rb = doughInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        yield return new WaitForSeconds(3); // Wait 3 seconds
        Debug.Log("Pizza cuite !");
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation; //Remove duplication
        }
        Renderer renderer = doughInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = cookedDoughMaterial; // Change material to "cooked"
        }
        doughInstance.tag = "CookedDough";

        StartCoroutine(BurnPizza(doughInstance));
    }

    private IEnumerator BurnPizza(GameObject doughInstance)
    {
        yield return new WaitForSeconds(5); // Wait 5 seconds

        if (isCooking)
        {
            Debug.Log("Pizza brûlée !");
            Renderer renderer = doughInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = burntDoughMaterial;
            }
            doughInstance.tag = "BurntDough";
        }
    }
}
