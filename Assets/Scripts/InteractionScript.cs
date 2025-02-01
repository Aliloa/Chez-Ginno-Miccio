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
                    grabableObject = grabableSauce;  // On attribue le script de sauce à la variable pour qu'on puisse la lâcher plus tard
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

        // Effectuer le raycast pour vérifier la présence d'un client
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, clientLayer))
        {
            // Vérifier si l'objet touché est un client
            if (raycastHit.transform.TryGetComponent(out client))
            {
                if (!client.hasOrder)
                {
                    // Le client n'a pas encore commandé
                    client.CreateOrder();
                }
                else if (grabableObject != null && grabableObject.CompareTag("CookedDough"))
                {
                    // Vérifie si on a un objet "CookedDough" dans les mains
                    client.CheckOrder(grabableObject.gameObject);
                }
                else
                {
                    // Le client répète sa commande
                    client.CreateOrder();
                }
            }
        }
    }

    private void Awake()
    {
        // Récupérer le PlayerInput attaché au GameObject
        playerInput = GetComponent<PlayerInput>();

        // Récupérer l'action "Interact"
        grabAction = playerInput.actions["Grab"];
        talkAction = playerInput.actions["Talk"];
    }
}
