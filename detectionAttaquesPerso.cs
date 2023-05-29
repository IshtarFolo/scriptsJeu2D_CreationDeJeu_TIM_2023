using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionAttaquesPerso : MonoBehaviour
{
    /*===========================
              Variables
     ============================*/
    private static int dommage = 10;//Le dommage d'une attaque du personnage joueur
    private int VieEnnemi;//La vie de l'ennemi declaree dans le script et a quoi on passe les donnees de la meme variable du script "ennemi"
    private int vieVolantTrou;//Vie de l'ennemi volant au dessus du trou avec les pointes
    private int vieVolant;//La variable pour la vie des ennemis volants
    private int vieBoss;//Vie du demon
    private int VieEnnemiPorte;//Pour le squelette a la porte
    private int vieEnnemiBoss;//La vie des squelettes qui sont invoques par le boss

    /*===========================
        Detection Collisions
     ============================*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si l'attaque touche un ennemi (Ici le squelette) 
        if (infoCollision.gameObject.tag == "Ennemi")
        {
            //On recupere les donnees de la variable "Vie" dans le script "ennemi" et on lui enleve le dommage
            VieEnnemi = Mathf.Clamp(ennemi.Vie -= dommage, 0, 20);
        } 
        //Si le joueur attaque le squelette devant la porte du chateau
        else if (infoCollision.gameObject.tag == "ennemiPorte")
        {
            //On recupere les donnees de la variable "Vie" dans le script "squelettePorte" et on lui enleve le dommage
            VieEnnemiPorte = Mathf.Clamp(squelettePorte.Vie -= dommage, 0, 20);
        }
        //Si le joueur attaque un squelette invoque par le boss...
        else if (infoCollision.gameObject.tag == "ennemiBoss")
        {
            //Il prends du dommage egal a l'attaque du joueur
            vieEnnemiBoss = Mathf.Clamp(ennemiBoss.Vie -= dommage, 0, 20);
        }
    }

    /*==========================
     *  Detection des Triggers
     ===========================*/
    void OnTriggerEnter2D(Collider2D infoTrigger) {
        //Si l'attaque touche un ennemi volant
        if (infoTrigger.gameObject.tag == "EnnemiVolant")
        {
            //Il perd de la vie egale au dommage de la boite de collision
            vieVolant = ennemiVolant.vieEnnemi -= dommage;
        }
        //Si le joueur attaque l'ennemi volant au dessus du trou avec les pointes
        else if (infoTrigger.gameObject.tag == "volantTrou")
        {
            //Il perd de la vie egale au dommage de la boite de collision
            vieVolantTrou = volantTrou.vieEnnemi -= dommage;
        }


        //Si l'attaque touche le boss...
        if (infoTrigger.gameObject.tag == "boss")
        {
            //Il perd de la vie pour l'equivalent de dommage de la boite de collision
            vieBoss = Demon.pointsVie -= dommage;
        }
    }
  
}
