using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrabableSauceScript : GrabableObjectScript
{
    [SerializeField] private ParticleSystem sauceParticles;

    public override void Grab(Transform grabPoint)
    {
        base.Grab(grabPoint);  // Call the parent class method to pick up the object
        sauceParticles.Play();
    }
    public override void Drop()
    {
        base.Drop(); // Call the parent class method to drop the object
        sauceParticles.Stop();
    }
}