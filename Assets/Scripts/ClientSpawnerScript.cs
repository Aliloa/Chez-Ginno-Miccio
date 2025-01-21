using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> clientsInScene;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform stopPoint;
    [SerializeField] private Transform exitPoint;
    public float moveSpeed = 2f;
    public float rotationSpeed = 2f;

    private int clientIndex = 0;
    private GameObject currentClient;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var client in clientsInScene)
        {
            client.SetActive(false);
        }
        SpawnNextClient();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnNextClient()
    {
        if (clientIndex < clientsInScene.Count)
        {
            GameObject existingClient = clientsInScene[clientIndex];
            existingClient.SetActive(true);
            existingClient.transform.position = spawnPoint.position;
            existingClient.transform.rotation = Quaternion.identity;
            currentClient = existingClient;
            StartCoroutine(MoveClientToShop(existingClient));
        }
        else
        {
            Debug.Log("Fin de la journée, tous les clients ont été servis !");
            EndDay();
        }
    }

    private IEnumerator MoveClientToShop(GameObject client)
    {
        currentClient = client;
        Vector3 targetPosition = stopPoint.position;

        while (Vector3.Distance(client.transform.position, targetPosition) > 0.1f)
        {
            // Calculer la direction du mouvement
            Vector3 direction = (targetPosition - client.transform.position).normalized;
            // Ajuster la rotation du client pour qu'il regarde dans la direction du mouvement
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                client.transform.rotation = Quaternion.Slerp(client.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            client.transform.position = Vector3.MoveTowards(client.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Client arrivé au magasin !");
        // Se tourner pour regarder vers le joueur
        Quaternion finalRotation = Quaternion.Euler(0, 180, 0); // Rotation autour de l'axe Y
        while (Quaternion.Angle(client.transform.rotation, finalRotation) > 0.1f)
        {
            client.transform.rotation = Quaternion.Slerp(client.transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        // Le client est maintenant en place, prêt pour l'interaction
    }

    public void OnOrderCompleted()
    {
        StartCoroutine(MoveClientToExit(currentClient));
    }

    private IEnumerator MoveClientToExit(GameObject client)
    {
        Vector3 targetPosition = exitPoint.position;

        while (Vector3.Distance(client.transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - client.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                client.transform.rotation = Quaternion.Slerp(client.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            client.transform.position = Vector3.MoveTowards(client.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Détruire le client une fois qu'il a quitté la scène
        Destroy(client);

        // Passer au client suivant
        clientIndex++;
        SpawnNextClient();
    }

    private void EndDay()
    {
        Debug.Log("Tous les clients ont été servis pour aujourd'hui !");
        // Transition vers la journée suivante
    }
}
