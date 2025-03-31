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

    [Header("Customizable dialogs")]
    [TextArea] public List<string> introductionDialogues = new List<string> { "Ah, what a beatiful day!", "I hope you got fresh pizzas" };
    [TextArea] public string orderText = "Can I get a uhhhh {0} pizza";
    [TextArea] public string repeatOrderText = "I said I want a {0} pizza";
    [TextArea] public string goodOrderText = "Thank you so much!";
    [TextArea] public string badOrderText = "This is not what I ordered!";

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

            // Mix introduction dialogues and the order
            List<string> fullDialogue = new List<string>(introductionDialogues);
            fullDialogue.Add(string.Format(orderText, orderManager.order));

            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(fullDialogue.ToArray());


            //string[] commande = { "Can I get a uhhhh " + orderManager.order + " pizza" };
            //dialogue.GetComponent<DialogueManagerScript>().StartDialogue(commande);
            //return;
        }
        else
        {
            string[] repetCommande = { string.Format(repeatOrderText, orderManager.order) };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(repetCommande);

            //string[] repetCommande = { "I said I want a " + orderManager.order + " pizza" };
            //dialogue.GetComponent<DialogueManagerScript>().StartDialogue(repetCommande);
            //return;
        }
    }
    public void RespondToOrder(GameObject pizza)
    {
        bool orderIsCorrect = orderManager.CheckOrder(pizza);

        if (orderIsCorrect)
        {
            string[] bonneCommande = { goodOrderText };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
            hasOrder = false; // Restart order
            StartCoroutine(WaitForDialogueToComplete());// Wait util the dialogue is done to contunue
        }
        else
        {
            string[] mauvaiseCommande = { badOrderText };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
        }
    }

    IEnumerator WaitForDialogueToComplete()
        {
            // Wait until the dialogue is finished
            yield return new WaitUntil(() => !dialogue.GetComponent<DialogueManagerScript>().isDialogueActive);

            // Once the dialog is complete, call OnOrderCompleted()
            clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
        }

}
