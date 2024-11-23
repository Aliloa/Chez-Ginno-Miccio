using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectScript : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    private GrabableObjectScript grabableObject;
    private OrderScript client;

    // Update is called once per frame
    void Update()
    {
        float interactDistance = 2f;

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit raycastHit;

            // First raycast: Check for grabbing or interacting
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out raycastHit, interactDistance))
            {
                // Check if the raycast hit a grabable object
                if (grabableObject == null) // No object in hand, so try to grab one
                {
                    if (raycastHit.transform.TryGetComponent(out grabableObject))
                    {
                        grabableObject.Grab(objectGrabPointTransform);
                    }
                }
                else // We have an object in hand, so drop it
                {
                    grabableObject.Drop();
                    grabableObject = null;
                }

                // Check if the raycast hit a client
                if (raycastHit.transform.TryGetComponent(out client))
                {
                    Debug.Log("Ok je suis pret à commander");
                }
            }
        }
    }
}
