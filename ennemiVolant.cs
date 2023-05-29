using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemiVolant : MonoBehaviour
{
    /*=====================
     *      Variables
     ======================*/
    private bool finPartie;//Variable de fin de partie
    public static int vie;//Vie du joueur
    public static int vieEnnemi;//vie de l'ennemi volant
    private int dommage = 10;//dommage de l'ennemi volant
    private float vitesseY;//Vitesse Y lors de sa chute a la mort
    private bool coup;//Pour activer ou desactiver l'animation de dommage

    // Start is called before the first frame update
    void Start()
    {
        //Si la fin de la partie est false au commencement du jeu...
        if (finPartie == false)
        {
            //Si la partie commence la vie de l'ennemi volant est a 10
            vieEnnemi = 10;
        }
    }

    void Update()
    {
        //Si la vie de l'ennemi tombe a 0 ou plus bas
        if (vieEnnemi <= 0)
        {
            //On active l'animation de mort de l'ennemi
            GetComponent<Animator>().SetTrigger("mort");
            //On met la gravite a 1 pour faire tomber la creature volante
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            //On desactive le collider sur l'ennemi
            GetComponent<Collider2D>().enabled = false;
            //L'ennemi est detruit
            Destroy(gameObject, 0.9f);
            //On desactive son parent
            transform.parent = null;
        }
    }

    /*========================================
     *      Detection des colllisions
     =========================================*/
    void OnTriggerEnter2D(Collider2D infoTrigger)
    {
        /*Si le monstre touche le joueur...*/
        if (infoTrigger.gameObject.name == "persoStatique_0")
        {
            //Le joueur perds de la vie equivalente au dommage de la creature, soit 10
            vie = deplacementPerso.Vie -= dommage;

            //On active l'animation de dommage pour le joueur
            coup = deplacementPerso.frappe = true;
            //L'animation de dommage est desactivee
            Invoke("PasFrappe", 0.2f);
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

