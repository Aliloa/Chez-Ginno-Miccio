using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderScript : MonoBehaviour
{
    private string order;
    List<string> orderIngredients = new List<string>();
    public bool hasOrder { get; private set; }

    //private DialogueManagerScript dialogueManager;

    [SerializeField] private GameObject dialogue; //object with the DialogueManager script (do not put the prefab otherwise it will not work)
    [SerializeField] private GameObject clientSpawner; //Object with the script clientSpawner

    private void Start()
    {
        //dialogueManager = FindObjectOfType<DialogueManagerScript>();
    }
    public void CreateOrder() {
        if (!hasOrder)
        {
            // Generate random order
            string[] possibleIngredients = { "Mushroom", "Pepperoni" };
            //order = possibleOrders[Random.Range(0, possibleOrders.Length)];

            // Randomly choose whether the pizza has 1 or 2 ingredients
            int numberOfIngredients = Random.Range(1, 3);

            while (orderIngredients.Count < numberOfIngredients)
            {
                string ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Length)];

                // Making sure to not select the same ingredient twice
                if (!orderIngredients.Contains(ingredient))
                {
                    orderIngredients.Add(ingredient);
                }
            }

            order = string.Join(" and ", orderIngredients);

            hasOrder = true;

                string[] commande = { "Can I get a uhhhh " + order + " pizza" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(commande);
                return;
        }
        else
        {
                string[] repetCommande = { "I said I want a " + order + "pizza" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(repetCommande);
                return;
        }
    }

    public void CheckOrder(GameObject pizza)
    {
        if (pizza != null && pizza.CompareTag("CookedDough"))
        {
            // Retrieve all children of the Dough object
            Transform doughTransform = pizza.transform;
            List<string> ingredientsOnPizza = new List<string>();

            foreach (Transform child in doughTransform)
            {
                ingredientsOnPizza.Add(child.tag); // Use tags to identify ingredients
            }

            bool orderIsCorrect = true;
            foreach (string ingredient in orderIngredients)
            {
                if (!ingredientsOnPizza.Contains(ingredient))
                {
                    orderIsCorrect = false;
                    break; // If an ingredient is missing, the check is stopped
                }
            }

            // Check the order
            if (orderIsCorrect)
            {
                string[] bonneCommande = { "Merci mon reuf" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
                hasOrder = false; // Reset order
                Destroy(pizza.gameObject); // Destroy the pizza once delivered
                StartCoroutine(WaitForDialogueToComplete());

                //Calculating the score: the more ingredients there are, the higher the score
                int scoreForPizza = ingredientsOnPizza.Count * 10; // + 10 points per ingredient
                ScoreManagerScript.Instance.AddPoints(scoreForPizza); // Add points to today's score
            }
            else
            {
                string[] mauvaiseCommande = { "This is not what I ordered!" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
                return;
            }
        }
        else //--When there is a single ingredient or an uncooked pizza
        {
            // -- It will never happen, but we'll see, maybe it will be useful...
            string[] mauvaiseCommande = { "Euh tu joues à quoi" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
            return;
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
