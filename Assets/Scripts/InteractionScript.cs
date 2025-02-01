using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private GrabableObjectScript grabableObject;
    private OrderScript client;
    private DialogueManagerScript dialogueManager;
    private DoughSpawnerScript doughSpawner;
    private GrabableSauceScript grabableSauce;

    private PlayerInput playerInput;
    private InputAction grabAction;
    private InputAction talkAction;

    public void OnGrab(InputValue value)
    {
            RaycastHit raycastHit;
            float interactDistance = 2f;
            LayerMask ingredientLayer = LayerMask.GetMask("Ingredients", "Default");

            // Attrapper et lacher les objets
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, ingredientLayer))
            {
                if (grabableObject == null)
                {
                    if (raycastHit.transform.TryGetComponent(out grabableObject))
                    {
                        grabableObject.Grab(objectGrabPointTransform);
                    }
                if (raycastHit.transform.TryGetComponent(out grabableSauce))
                {
                    grabableSauce.Grab(objectGrabPointTransform);
                    grabableObject = grabableSauce;  // On attribue le script de sauce � la variable pour qu'on puisse la l�cher plus tard
                }
            }
                else
                {
                    grabableObject.Drop();
                    grabableObject = null;
                }
                if (raycastHit.transform.TryGetComponent(out doughSpawner))
            {
                doughSpawner.SpawnDough();
            }
             }
    }
    //    //------------------------------------------------------------- Interaction client et dialogue

    private void OnTalk(InputValue value)
    {
        float interactDistance = 3f;
        LayerMask clientLayer = LayerMask.GetMask("Client");
        RaycastHit raycastHit;

        dialogueManager = FindObjectOfType<DialogueManagerScript>();
        if (dialogueManager != null && dialogueManager.isDialogueActive)
        {
            dialogueManager.NextLine();
            return;
        }

        // Effectuer le raycast pour v�rifier la pr�sence d'un client
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, clientLayer))
        {
            // V�rifier si l'objet touch� est un client
            if (raycastHit.transform.TryGetComponent(out client))
            {
                if (!client.hasOrder)
                {
                    // Le client n'a pas encore command�
                    client.CreateOrder();
                }
                else if (grabableObject != null && grabableObject.CompareTag("CookedDough"))
                {
                    // V�rifie si on a un objet "CookedDough" dans les mains
                    client.CheckOrder(grabableObject.gameObject);
                }
                else
                {
                    // Le client r�p�te sa commande
                    client.CreateOrder();
                }
            }
        }
    }

    private void Awake()
    {
        // R�cup�rer le PlayerInput attach� au GameObject
        playerInput = GetComponent<PlayerInput>();

        // R�cup�rer l'action "Interact"
        grabAction = playerInput.actions["Grab"];
        talkAction = playerInput.actions["Talk"];
    }
}
