using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableCheeseScript : GrabableObjectScript
{
    public ParticleSystem cheeseParticles;  // Système de particules pour la sauce

    public override void Grab(Transform grabPoint)
    {
        base.Grab(grabPoint);  // Appeler la méthode de la classe parente pour ramasser l'objet
        cheeseParticles.Play();
    }
    public override void Drop()
    {
        base.Drop();  // Appeler la méthode de la classe parente pour lâcher l'objet
        cheeseParticles.Stop(); 
    }
}
