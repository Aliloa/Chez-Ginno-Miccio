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

    private PlayerInput playerInput;
    private InputAction grabAction;
    private InputAction talkAction;

    private void OnGrab(InputAction.CallbackContext context)
    {
            RaycastHit raycastHit;
            float interactDistance = 2f;
            LayerMask ingredientLayer = LayerMask.GetMask("Ingredient");

            // Attrapper et lacher les objets
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, ingredientLayer))
            {
                if (grabableObject == null)
                {
                    if (raycastHit.transform.TryGetComponent(out grabableObject))
                    {
                        grabableObject.Grab(objectGrabPointTransform);
                    }
                }
                else
                {
                    grabableObject.Drop();
                    grabableObject = null;
                }
        }
    }
    //    //------------------------------------------------------------- Interaction client
    private void OnTalk(InputAction.CallbackContext context)
    {
        float interactDistance = 3f;
        LayerMask clientLayer = LayerMask.GetMask("Client");
        RaycastHit raycastHit;

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

    private void OnEnable()
    {
        // Activer l'�coute de l'action
        grabAction.started += OnGrab;
        talkAction.started += OnTalk;
    }

    private void OnDisable()
    {
        // D�sactiver l'�coute de l'action
        grabAction.started -= OnGrab;
        talkAction.started -= OnTalk;
    }
}
