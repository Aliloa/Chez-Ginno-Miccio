using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrabableSauceScript : GrabableObjectScript
{
    public ParticleSystem sauceParticles;  // Système de particules pour la sauce

    public override void Grab(Transform grabPoint)
    {
        base.Grab(grabPoint);  // Appeler la méthode de la classe parente pour ramasser l'objet
        sauceParticles.Play();
    }
    public override void Drop()
    {
        base.Drop();  // Appeler la méthode de la classe parente pour lâcher l'objet
        sauceParticles.Stop();  // Arrêter l'écoulement de la sauce
    }
}