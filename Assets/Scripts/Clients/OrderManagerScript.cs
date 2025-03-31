using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManagerScript : MonoBehaviour
{
    public string order;
    // List of all the ingredients to drag and drop in the inspector
    [SerializeField] private List<GameObject> possibleIngredients; 

    // List of ingredients in the order
    private List<GameObject> orderIngredients = new List<GameObject>();
    public string CreateOrder()
    {
        // Clear previous order
        orderIngredients.Clear();

        List<string> orderIngredientNames = new List<string>();
        foreach (var ingredient in possibleIngredients)
        {
            Debug.Log("Possible ingredient: " + ingredient.name);
        }
        // Randomly choose whether the pizza has 1 or 2 ingredients
        int numberOfIngredients = Random.Range(1, 3);

        while (orderIngredients.Count < numberOfIngredients)
            {
            GameObject ingredient = possibleIngredients[Random.Range(0, possibleIngredients.Count)];

            // Making sure to not select the same ingredient twice
            if (!orderIngredients.Contains(ingredient))
                {
                    orderIngredients.Add(ingredient);
                orderIngredientNames.Add(ingredient.name);
                Debug.Log("Added ingredient: " + ingredient.name);
            }
            }
            order = string.Join(" and ", orderIngredientNames);
            return order;
    }

    public bool CheckOrder(GameObject pizza)
    {
        if (pizza == null || !pizza.CompareTag("CookedDough"))
            return false;

        // Get ingredients on pizza
        Transform doughTransform = pizza.transform;
        List<string> ingredientsOnPizza = new List<string>();

        foreach (Transform child in doughTransform)
        {
            ingredientsOnPizza.Add(child.tag);
        }

        // Check if all the ingredients are present
        foreach (GameObject ingredient in orderIngredients)
        {
            if (!ingredientsOnPizza.Contains(ingredient.tag))
            {
                return false; // Ingredient missing
            }
        }

        // If all ingredients are present, complete the order
        CompleteOrder(pizza);
        return true;
    }

    public void CompleteOrder(GameObject pizza)
    {
            Destroy(pizza.gameObject); // Destroy the pizza

            // Calculating score : +10 points per ingredient
            int scoreForPizza = orderIngredients.Count * 10;
            ScoreManagerScript.Instance.AddPoints(scoreForPizza);
    }
}
