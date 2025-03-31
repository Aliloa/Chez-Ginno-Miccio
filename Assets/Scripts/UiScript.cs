using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    private GrabableObjectScript grabbableObject;
    private OrderScript client;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private LayerMask clientLayer;
    [SerializeField] private TextMeshProUGUI pressE;
    [SerializeField] private TextMeshProUGUI pressF;

    void Start()
    {
        pressE.enabled = false;
        pressF.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player did all the interactions disable the script
        CheckForClient();
        CheckForGrabbableObject();
    }

    private void CheckForGrabbableObject()
    {
        RaycastHit raycastHit;
        float interactDistance = 2f;

        // Cast a ray from the camera's position in the direction the camera is facing
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, pickupLayerMask))
        {
            // Check if the object hit has a GrabbableObjectScript component
            if (raycastHit.transform.TryGetComponent(out grabbableObject))
            {
                // Show the "Press E" UI element if the player is looking at a grabbable object
                pressE.enabled = true;
            }
            else
            {
                // Hide the "Press E" UI element if the player is not looking at a grabbable object
                pressE.enabled = false;
            }
        }
        else
        {
            // Hide the "Press E" UI element if nothing is hit by the raycast
            pressE.enabled = false;
        }
    }

    private void CheckForClient()
    {
        RaycastHit raycastHit;
        float interactDistance = 3f;

        // Cast a ray from the camera's position in the direction the camera is facing
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance, clientLayer))
        {
            // Check if the object hit has a GrabbableObjectScript component
            if (raycastHit.transform.TryGetComponent(out client))
            {
                // Show the "Press E" UI element if the player is looking at a grabbable object
                pressF.enabled = true;
            }
            else
            {
                // Hide the "Press E" UI element if the player is not looking at a grabbable object
                pressF.enabled = false;
            }
        }
        else
        {
            // Hide the "Press E" UI element if nothing is hit by the raycast
            pressF.enabled = false;
        }
    }
}
