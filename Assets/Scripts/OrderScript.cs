using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{
    private string order;
    public bool hasOrder { get; private set; }

    //private DialogueManagerScript dialogueManager;

    [SerializeField] private GameObject dialogue; //objet sur lequel se trouve le script DialogueManager (ne pas mettre le prefab sinon ca marche pas
    [SerializeField] private GameObject clientSpawner; //objet sur lequel se trouve le script clientSpawner

    private void Start()
    {
        //dialogueManager = FindObjectOfType<DialogueManagerScript>();
    }
    public void CreateOrder() {
        if (!hasOrder)
        {
            // Generate a random order
            string[] possibleOrders = { "Mushrooms Pizza", "Pepperoni Pizza" };
            order = possibleOrders[Random.Range(0, possibleOrders.Length)];

            // Mark that the client has an order
            hasOrder = true;

                string[] commande = { "Can I get a uhhhh " + order };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(commande);
                return;
            //Debug.Log("Can I get a uhhhh " + order);
        }
        else
        {
                string[] repetCommande = { "I said I want a " + order };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(repetCommande);
                return;
            //Debug.Log("I said I want a " + order);
        }
    }

    public void CheckOrder(GameObject pizza)
    {
        if (pizza != null && pizza.CompareTag("CookedDough"))
        {
            // Récupérer tous les enfants de l'objet Dough
            Transform doughTransform = pizza.transform;
            List<string> ingredientsOnPizza = new List<string>();

            foreach (Transform child in doughTransform)
            {
                ingredientsOnPizza.Add(child.tag); // Utilise les tags pour identifier les ingrédients
            }

            // Vérification de la commande
            if (order == "Mushrooms Pizza" && ingredientsOnPizza.Contains("Mushroom") && !ingredientsOnPizza.Contains("Pepperoni"))
            {
                string[] bonneCommande = { "Merci mon reuf" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
                hasOrder = false; // Réinitialiser la commande
                Destroy(pizza.gameObject); // Détruire la pizza une fois livrée
                //clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
                StartCoroutine(WaitForDialogueToComplete());
            }
            else if (order == "Pepperoni Pizza" && ingredientsOnPizza.Contains("Pepperoni") && !ingredientsOnPizza.Contains("Mushroom"))
            {
                //Debug.Log("Merci mon reuf");
                string[] bonneCommande = { "Merci mon reuf" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
                hasOrder = false; // Réinitialiser la commande
                Destroy(pizza.gameObject); // Détruire la pizza une fois livrée
                //clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
                StartCoroutine(WaitForDialogueToComplete());
            }
            else
            {
                //Debug.Log("This is not what I ordered!");
                string[] mauvaiseCommande = { "This is not what I ordered!" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
                return;
            }
        }
        else //-- Quand on donne juste un ingrédient seul ou une pizza pas cuite
        {
            //Debug.Log("Euh tu joues à quoi"); // -- ca va jamais s'executer mais à voir peut être ça va servir
            string[] mauvaiseCommande = { "Euh tu joues à quoi" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
            return;
        }
    }

    IEnumerator WaitForDialogueToComplete()
    {
        // Attendre que le dialogue soit terminé (tu devras adapter selon ton système de dialogue)
        yield return new WaitUntil(() => !dialogue.GetComponent<DialogueManagerScript>().isDialogueActive);

        // Une fois le dialogue terminé, appeler OnOrderCompleted()
        clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
    }

}
