using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughSpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject dough;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentDough;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDough();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDough()
    {
        if (currentDough != null)
        {
            Destroy(currentDough);
        }
        currentDough = Instantiate(dough, spawnPoint.position, spawnPoint.rotation);
    }
}
