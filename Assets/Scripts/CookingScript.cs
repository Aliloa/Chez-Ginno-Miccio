using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingScript : MonoBehaviour
{
    [SerializeField] private GameObject dough;
    [SerializeField] private Material CookedDoughMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dough"))
        {
            //isTouchingDough = true;
            Debug.Log("Pizza en train de cuir");
            StartCoroutine(CookPizza(collision.gameObject));
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
        //isCooking = false; // Permet de relancer la cuisson si n�cessaire
    }
}
