using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderScript : MonoBehaviour
{
    public bool hasOrder { get; private set; }

    //private DialogueManagerScript dialogueManager;

    [SerializeField] private GameObject dialogue; //object with the DialogueManager script (do not put the prefab otherwise it will not work)
    [SerializeField] private GameObject clientSpawner; //Object with the script clientSpawner
    private OrderManagerScript orderManager;

    void Start()
    {
        // Récupérer OrderManagerScript sur le même GameObject
        orderManager = GetComponent<OrderManagerScript>();
    }


    public void StartOrder()
    {
        if (!hasOrder)
        {
            hasOrder = true;

            orderManager.order = orderManager.CreateOrder(); //Generate the order

            string[] commande = { "Can I get a uhhhh " + orderManager.order + " pizza" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(commande);
            return;
        }
        else
        {
            string[] repetCommande = { "I said I want a " + orderManager.order + " pizza" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(repetCommande);
            return;
        }
    }
    public void RespondToOrder(GameObject pizza)
    {
        bool orderIsCorrect = orderManager.CheckOrder(pizza);

        if (orderIsCorrect)
        {
            string[] bonneCommande = { "Thank you so much!" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
            hasOrder = false; // Restart order
        }
        else
        {
            string[] mauvaiseCommande = { "This is not what I ordered!" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
        }
        // Wait util the dialogue is done to contunue
        StartCoroutine(WaitForDialogueToComplete());
    }

    IEnumerator WaitForDialogueToComplete()
        {
            // Wait until the dialogue is finished
            yield return new WaitUntil(() => !dialogue.GetComponent<DialogueManagerScript>().isDialogueActive);

            // Once the dialog is complete, call OnOrderCompleted()
            clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
        }

}
