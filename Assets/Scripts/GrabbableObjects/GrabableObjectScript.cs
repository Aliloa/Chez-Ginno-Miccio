using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableObjectScript : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    //private bool isTouchingDough = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dough") && collision.gameObject.layer == gameObject.layer)
        {
            //isTouchingDough = true;
            Debug.Log("The object is touching the dough!");
            this.transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dough") && collision.gameObject.layer == gameObject.layer)
        {
            //isTouchingDough = false;
            this.transform.SetParent(null);
        }
    }

    void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = false;
        //Modify the rigid body of all the children (of the dough) so that I can move the ingredients with it
        foreach (Transform child in this.transform)
        {
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();
            if (childRigidbody != null)
            {
                childRigidbody.WakeUp();
                childRigidbody.isKinematic = true; // Disable physics
                childRigidbody.useGravity = false;
            }
            Collider childCollider = child.GetComponent<Collider>(); //remove the collider too otherwise it crashes
            if (childCollider != null)
            {
                childCollider.enabled = false;
            }
        }
    }

    public virtual void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        //Put RigidBody back
        foreach (Transform child in this.transform)
        {
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();
            if (childRigidbody != null)
            {
                childRigidbody.WakeUp();
                childRigidbody.isKinematic = false;
                childRigidbody.useGravity = true;
            }
            Collider childCollider = child.GetComponent<Collider>();
            if (childCollider != null)
            {
                childCollider.enabled = true;
            }
        }
    }

    [SerializeField] private float lerpSpeed = 5f;
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }
    }

}
