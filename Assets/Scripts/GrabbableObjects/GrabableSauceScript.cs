using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrabableSauceScript : GrabableObjectScript
{
    [SerializeField] private ParticleSystem sauceParticles;
    private Animator animator;
    [SerializeField] private AudioSource sauceSound;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Grab(Transform grabPoint)
    {
        base.Grab(grabPoint);  // Call the parent class method to pick up the object
        animator.SetBool("isGrabbing", true);
    }

    public void PlaySauceParticles()
    {
        if (sauceParticles != null)
        {
            sauceParticles.Play();  // Joue les particules
            sauceSound.Play();
        }
    }

    public override void Drop()
    {
        base.Drop(); // Call the parent class method to drop the object
        animator.SetBool("isGrabbing", false);
        sauceParticles.Stop();
        sauceSound.Stop();
    }
}