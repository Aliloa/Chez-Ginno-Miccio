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
        //Modifier le rigid body de tous les enfants (de la pate) comme ça je peux bouger les ingrédients avec
        foreach (Transform child in this.transform)
        {
            Rigidbody childRigidbody = child.GetComponent<Rigidbody>();
            if (childRigidbody != null)
            {
                childRigidbody.WakeUp();
                childRigidbody.isKinematic = true; // Disable physics
                childRigidbody.useGravity = false;
            }
            Collider childCollider = child.GetComponent<Collider>(); //enlever le collider aussi sinon ça beug
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
        //Remettre le Rigidbody
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
            //Vector3 targetPosition = objectGrabPointTransform.position;
            //objectRigidbody.MovePosition(Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed));
        }
    }

    //private Transform originalParent;

    //public void Grab(Transform grabPoint)
    //{
    //    originalParent = transform.parent;
    //    if (GetComponent<Rigidbody>() != null)
    //    {
    //        GetComponent<Rigidbody>().useGravity = false;
    //        GetComponent<Rigidbody>().isKinematic = true;
    //    }
    //    transform.SetParent(grabPoint);
    //    transform.localPosition = Vector3.zero;
    //    transform.localRotation = Quaternion.identity;
    //}

    //public void Drop()
    //{
    //    transform.SetParent(originalParent);
    //    if (GetComponent<Rigidbody>() != null)
    //    {
    //        GetComponent<Rigidbody>().useGravity = true;
    //        GetComponent<Rigidbody>().isKinematic = false;
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Dough"))
    //    {
    //        this.transform.SetParent(collision.gameObject.transform);
    //        //Vector3 globalScale = this.transform.lossyScale;
    //        //this.transform.localScale = globalScale;
    //        //this.transform.rotation = Quaternion.identity;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Dough"))
    //    {
    //        this.transform.SetParent(null);
    //    }
    //}

}
