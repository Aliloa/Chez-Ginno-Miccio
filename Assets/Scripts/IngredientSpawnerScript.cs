using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject ingredient;
    [SerializeField] private int maxIngredients = 5;
    private HashSet<GameObject> ingredientsInSpawner = new HashSet<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnIngredientsOverTime());
    }

    void Update()
    {
        if (ingredientsInSpawner.Count < maxIngredients)
        {
            StartCoroutine(SpawnIngredientsOverTime());
        }
    }

    private IEnumerator SpawnIngredientsOverTime() // Mettre une intervale entre chaque spawn pour pas que ça explose au début
    {
        float spawnInterval = 0.05f;
        while (ingredientsInSpawner.Count < maxIngredients)
        {
            SpawnIngredient();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnIngredient()
    {// Mettre aussi une distance entre chaque spawn pour pas que ça explose
        Vector3 offset = new Vector3(
    Random.Range(-0.05f, 0.05f),
    0,
    Random.Range(-0.05f, 0.05f)
    );
        Vector3 spawnPosition = transform.position + offset;
        GameObject newingredient = Instantiate(ingredient, spawnPosition, Quaternion.identity);
    }

    // --------------------------Faire respawn les ingredients quand ils sortent du bol
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith(ingredient.name))
        {
            ingredientsInSpawner.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith(ingredient.name))
        {
            ingredientsInSpawner.Remove(other.gameObject);
        }
    }
}