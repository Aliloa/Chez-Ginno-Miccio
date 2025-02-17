using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingScript : MonoBehaviour
{
    [SerializeField] private GameObject dough;
    [SerializeField] private Material CookedDoughMaterial;
    [SerializeField] private Material BurntDoughMaterial;

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
    private IEnumerator CookPizza(GameObject doughInstance) //IEnumerator fonction qui prend du temps � s'�xecuter sans bloquer les autres
    {
        //isCooking = true; // Emp�che de relancer la cuisson plusieurs fois
        Rigidbody rb = doughInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        yield return new WaitForSeconds(3); // Attend 3 secondes
        Debug.Log("Pizza cuite !");
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation; //enlever la duplication
        }
        Renderer renderer = doughInstance.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = CookedDoughMaterial; // Change material to "cooked"
        }
        doughInstance.tag = "CookedDough";

        StartCoroutine(BurnPizza(doughInstance));
    }

    private IEnumerator BurnPizza(GameObject doughInstance)
    {
        yield return new WaitForSeconds(5); // Attend 5 secondes après la cuisson

        if (isCooking)
        {
            Debug.Log("Pizza brûlée !");
            Renderer renderer = doughInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = BurntDoughMaterial;
            }
            doughInstance.tag = "BurntDough";
        }
    }
}
