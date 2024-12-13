using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeperoniSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject pepperoni;
    [SerializeField] private int maxPepperonis = 5;
    private HashSet<GameObject> pepperonisInSpawner = new HashSet<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnPepperonisOverTime());
    }

    void Update()
    {
        if (pepperonisInSpawner.Count < maxPepperonis)
        {
            StartCoroutine(SpawnPepperonisOverTime());
        }
    }

    private IEnumerator SpawnPepperonisOverTime() // Mettre une intervale entre chaque spawn pour pas que ça explose au début
    {
        float spawnInterval = 0.05f;
        while (pepperonisInSpawner.Count < maxPepperonis)
        {
            SpawnPepperoni();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPepperoni()
    {// Mettre aussi une distance entre chaque spawn pour pas que ça explose
        Vector3 offset = new Vector3(
    Random.Range(-0.05f, 0.05f),
    0,
    Random.Range(-0.05f, 0.05f)
    );
        Vector3 spawnPosition = transform.position + offset;
        GameObject newPepperoni = Instantiate(pepperoni, spawnPosition, Quaternion.identity);
    }

    // --------------------------Faire respawn les Pepperonis quand ils sortent du bol
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pepperoni"))
        {
            pepperonisInSpawner.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ( other.CompareTag("Pepperoni"))
        {
            pepperonisInSpawner.Remove(other.gameObject);
        }
    }
}
