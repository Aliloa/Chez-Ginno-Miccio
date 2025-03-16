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
            LayerMask ingredientLayer = LayerMask.GetMask("Ingredients");

        // Catching and dropping objects
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
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
                    grabableObject = grabableSauce;  // We assign the sauce script to the variable so that we can drop it later
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
    //Customer interaction and dialogue

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

        // Raycast to check for client presence
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, clientLayer))
        {
            // Check if the touched object is a customer
            if (raycastHit.transform.TryGetComponent(out client))
            {
                if (!client.hasOrder)
                {
                    // The customer has not yet ordered
                    client.CreateOrder();
                }
                else if (grabableObject != null && grabableObject.CompareTag("CookedDough"))
                {
                    // Check if you have a "CookedDough" object in your hands
                    client.CheckOrder(grabableObject.gameObject);
                }
                else
                {
                    // The customer repeats his order
                    client.CreateOrder();
                }
            }
        }
    }

    private void Awake()
    {
        //Get the PlayerInput attached to the GameObject
        playerInput = GetComponent<PlayerInput>();

        // Get "Interact" action
        grabAction = playerInput.actions["Grab"];
        talkAction = playerInput.actions["Talk"];
    }
}
