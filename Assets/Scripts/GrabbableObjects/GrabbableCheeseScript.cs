using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableCheeseScript : GrabableObjectScript
{
    public ParticleSystem cheeseParticles;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void PlayCheeseParticles()
    {
        cheeseParticles.Play();
    }

    public override void Grab(Transform grabPoint)
    {
        base.Grab(grabPoint);
        animator.SetBool("isGrabbing", true);
        Invoke(nameof(PlayCheeseParticles), 1f); //Start pouring cheese after 1 second
    }
    public override void Drop()
    {
        base.Drop();
        cheeseParticles.Stop();
        animator.SetBool("isGrabbing", false);
    }
}
