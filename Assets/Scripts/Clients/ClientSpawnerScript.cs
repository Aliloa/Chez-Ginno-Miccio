using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientSpawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> clientsInScene;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform stopPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 2f;

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
            //DayManagerScript.Instance.EndDay();
            ScoreManagerScript.Instance.EndDayScore();
        }
    }

    private IEnumerator MoveClientToShop(GameObject client)
    {
        currentClient = client;
        Vector3 targetPosition = stopPoint.position;

        while (Vector3.Distance(client.transform.position, targetPosition) > 0.1f)
        {
            // Calculate the direction of movement
            Vector3 direction = (targetPosition - client.transform.position).normalized;
            // Adjust the client's rotation so that they are looking in the direction of movement
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                client.transform.rotation = Quaternion.Slerp(client.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            client.transform.position = Vector3.MoveTowards(client.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        // Turn to look towards the player
        Quaternion finalRotation = Quaternion.Euler(0, 180, 0); // Rotation around the Y axis
        while (Quaternion.Angle(client.transform.rotation, finalRotation) > 0.1f)
        {
            client.transform.rotation = Quaternion.Slerp(client.transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
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

        // Destroy the client once he has left the scene
        Destroy(client);

        // Next client
        clientIndex++;
        SpawnNextClient();
    }
}
