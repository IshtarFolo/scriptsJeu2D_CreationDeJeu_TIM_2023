using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonPorte : MonoBehaviour
{
    /*============================
     *        Variables
     =============================*/
    public GameObject perso;//Pour connaitre la position du joueur
    private bool joueUneFois;//Pour que le son ne joue qu'une fois


    // Start is called before the first frame update
    void Start()
    {
        joueUneFois = false;//La variable pour determiner l'etat du son est a false
    }

    // Update is called once per frame
    void Update()
    {
        //Si le joueur est a 91f ou plus grand et si le jeu n'a pas encore joue...
        if (perso.transform.position.x >= 91f && joueUneFois == false)
        {
            //Le son joue
            GetComponent<AudioSource>().Play();
            //La variable determinant si le son a deja joue ou non passe a vrai
            joueUneFois = true;
            //On invoque l'arret du son apres 3 secondes (soit le nombre de temps que dure le son)
            Invoke("ArreterLeSon", 3f);
        }
    }

    /*==========================================
     *         FONCTION SUPPLEMENTAIRE
     ===========================================*/
    /*Fonction pour l'arret du son*/
    void ArreterLeSon()
    {
        GetComponent<AudioSource>().Stop();
    }
}
