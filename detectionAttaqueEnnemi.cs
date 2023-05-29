using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class detectionAttaqueEnnemi : MonoBehaviour
{
    /*===================
          Variables
     ====================*/
    private int dommage = 10;//Nombre de dommage subit lorsque l'attaque du squelette touche le joueur
    private int Vie;//Variable qui va prendre les parametres de vie du personnage
    private bool coup;//Booleen pour activer l'animation de coup recut par le joueur
    private bool partieTerminee;

    /*------Sons--------*/
    public AudioClip ouchJoueur;//Son du joueur blesse

    void Update()
    {
        partieTerminee = deplacementPerso.partieTerminee;
    }
    /*=============================
        Detection de collisions
     ==============================*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'attaque de l'ennemi entre en contact avec le personnage joueur
        if (infoCollision.gameObject.name == "persoStatique_0" && partieTerminee == false)
        {
            //Le joueur perds le nombre de dommage en vie, ici 10  
            Vie = deplacementPerso.Vie -= dommage;
            //
            GetComponent<AudioSource>().PlayOneShot(ouchJoueur, 0.5f);
            //On active l'animation de dommage pour le joueur
           coup = deplacementPerso.frappe = true;
           //L'animation de dommage est desactivee
           Invoke("PasFrappe", 0.2f);
        } 
        else if (partieTerminee == true)
        {
            GetComponent<AudioSource>().enabled = false;
        }
    }

    /*=============================
     *  Fonction Supplementaire
     ==============================*/
    //Fonction pour desactiver l'animation de dommage
    void PasFrappe()
    {
        //Le bolleen frappe est false
        coup = deplacementPerso.frappe = false;
    }
}

