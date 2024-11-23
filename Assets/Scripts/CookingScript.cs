using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingScript : MonoBehaviour
{
    [SerializeField] private GameObject dough;
    [SerializeField] private Material CookedDoughMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == dough)
        {
            //isTouchingDough = true;
            Debug.Log("Pizza en train de cuir");
            StartCoroutine(CookPizza());
        }
    }
    private IEnumerator CookPizza() //IEnumerator fonction qui prend du temps � s'�xecuter sans bloquer les autres
    {
        //isCooking = true; // Emp�che de relancer la cuisson plusieurs fois
        dough.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(3); // Attend 3 secondes
        Debug.Log("Pizza cuite !");
        dough.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        dough.GetComponent<Renderer>().material = CookedDoughMaterial;
        //isCooking = false; // Permet de relancer la cuisson si n�cessaire
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
