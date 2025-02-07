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

    [SerializeField] private GameObject dialogue; //objet sur lequel se trouve le script DialogueManager (ne pas mettre le prefab sinon ca marche pas
    [SerializeField] private GameObject clientSpawner; //objet sur lequel se trouve le script clientSpawner

    private void Start()
    {
        //dialogueManager = FindObjectOfType<DialogueManagerScript>();
    }
    public void CreateOrder() {
        if (!hasOrder)
        {
            // generer commande random
            string[] possibleIngredients = { "Mushroom", "Pepperoni" };
            //order = possibleOrders[Random.Range(0, possibleOrders.Length)];

            // Choisir aléatoirement si la pizza a 1 ou 2 ingrédients
            int numberOfIngredients = Random.Range(1, 3); // 1 ou 2 ingrédients

            while (orderIngredients.Count < numberOfIngredients)
            {
                string ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Length)];

                // S'assurer qu'on ne sélectionne pas le même ingrédient deux fois
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
            // Récupérer tous les enfants de l'objet Dough
            Transform doughTransform = pizza.transform;
            List<string> ingredientsOnPizza = new List<string>();

            foreach (Transform child in doughTransform)
            {
                ingredientsOnPizza.Add(child.tag); // Utilise les tags pour identifier les ingrédients
            }

            bool orderIsCorrect = true;
            foreach (string ingredient in orderIngredients)
            {
                if (!ingredientsOnPizza.Contains(ingredient))
                {
                    orderIsCorrect = false;
                    break; // Si un ingrédient est manquant, on arrête la vérification
                }
            }

            // Vérification de la commande
            if (orderIsCorrect)
            {
                string[] bonneCommande = { "Merci mon reuf" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(bonneCommande);
                hasOrder = false; // Réinitialiser la commande
                Destroy(pizza.gameObject); // Détruire la pizza une fois livrée
                StartCoroutine(WaitForDialogueToComplete());

                // Calcul du score : plus il y a d'ingrédients, plus le score est élevé
                int scoreForPizza = ingredientsOnPizza.Count * 10; // Exemple : 10 points par ingrédient
                ScoreManagerScript.Instance.AddPoints(scoreForPizza); // Ajouter des points au score du jour
            }
            else
            {
                string[] mauvaiseCommande = { "This is not what I ordered!" };
                dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
                return;
            }
        }
        else //-- Quand on donne juste un ingrédient seul ou une pizza pas cuite
        {
            // -- ca va jamais s'executer mais à voir peut être ça va servir
            string[] mauvaiseCommande = { "Euh tu joues à quoi" };
            dialogue.GetComponent<DialogueManagerScript>().StartDialogue(mauvaiseCommande);
            return;
        }
    }

    IEnumerator WaitForDialogueToComplete()
    {
        // Attendre que le dialogue soit terminé
        yield return new WaitUntil(() => !dialogue.GetComponent<DialogueManagerScript>().isDialogueActive);

        // Une fois le dialogue terminé, appeler OnOrderCompleted()
        clientSpawner.GetComponent<ClientSpawnerScript>().OnOrderCompleted();
    }

}
