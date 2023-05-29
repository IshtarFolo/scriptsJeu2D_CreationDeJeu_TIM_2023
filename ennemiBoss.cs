using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ennemiBoss : MonoBehaviour
{
    /*=======================
            Variables
    =========================*/
    public float vitesseX;//Pour le deplacement du squelette

    public static int Vie;//Vie du squelette
    private bool finPartie;//Variable de la fin de partie

    public GameObject joueur;//Variable pour le personnage joueur
    public float distance;//Variable qui calcule la distance entre le joueur et l'ennemi

    private float compteur;//compteur pour le son

    /*--------Sons--------*/
    public AudioClip marche;//Son du squelette lorsqu'il marche


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetTrigger("invocation");//animation d'invocation des squelettes qui joue au debut
        //Si la fin de la partie est false au commencement du jeu...
        if (finPartie == false)
        {
            //La vie du squelette est de 20 points
            Vie = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Le compteur augmente avec les secondes
        compteur += Time.deltaTime;
        //On cree un nouveau vecteur 2 pour calculer la distance entre le personnage joueur et le squelette pour lui permettre de poursuivre le joueur plus tard
        distance = Vector2.Distance(transform.position, joueur.transform.position);

        /*======================================
            Detection de la fin de la partie
        ========================================*/
        //La fin de partie depend de la variable partie terminee du script deplacementPerso
        finPartie = deplacementPerso.partieTerminee;

        /*=================================
            Gestion de l'AI du squelette
        ===================================*/
        /*Pour que le squelette suive le joueur...*/
        //Si la position x du joueur est plus grande que la position x du squelette...
        if (joueur.transform.position.x >= transform.position.x) {
            //celui-ci se retourne pour faire face au joueur
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Sinon il reprend sa position
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //Si la distance entre le joueur et le squelette est plus petite que 7 et plus grande que 1...
        if (distance < 7 && distance > 1)
        {
            //lorsque la distance est plus petite que 7 et plsu grande que 1, le squelette marche vers le personnage joueur
            GetComponent<Animator>().SetBool("marche", true);
            /*La position du squelette se met a jour par rapport a la position du personnage et la vitesseX permet un decalage entre la mise a jour de position du joueur 
            et de l'ennemi (ici j'utilise la fonction MoveTowards de Unity, ici se trouve le liens vers le tutoriel pour l'AI de l'ennemi: https://www.youtube.com/watch?v=2SXa10ILJms&t=165s&ab_channel=MoreBBlakeyyy)*/
            transform.position = Vector2.MoveTowards(this.transform.position, joueur.transform.position, vitesseX * Time.deltaTime);
            //Lorsque le squelette marche l'attaque est false 
            GetComponent<Animator>().SetBool("attaque", false);

            //Si le compteur atteint 1 seconde ou plus
            if (compteur >= 1f)
            {
                //Le son de la marche du squelette joue
                GetComponent<AudioSource>().PlayOneShot(marche, 2f);
                //Le compteur est remis a 0
                compteur = 0;
            }
        }
        //Si la distance est plus grand que 7...
        else if (distance > 7)
        {
            //l'animation de marche est interrompue
            GetComponent<Animator>().SetBool("marche", false);
        }

        /*=============================
                   Attaque
         ==============================*/
        //Si la distance est plus petite ou egale a 1 et que la vie du joueur est plus grande que 0...
        if (distance <= 1 && deplacementPerso.Vie > 0)
        {
            //L'animation de marche est interrompue
            GetComponent<Animator>().SetBool("marche", false);
            //On invoke l'attaque
            Invoke("Attaque", 0.2f);
            //On invoque l'annulation de l'attaque pour donner une impression de temps de recuperation 
            Invoke("AnnulerAttaque", 0.5f);
        }
        //Si le squelette est pres du personnage et que le joueur est mort, on empeche le squelette d'attaquer et de marcher
        else if (distance <= 1 && deplacementPerso.Vie <= 0)
        {
            GetComponent<Animator>().SetBool("marche", false);
            GetComponent<Animator>().SetBool("attaque", false);
        }

        /*====================
         *   Mort du Boss
         =====================*/
        //Si la vie du boss tombe a 0...
        if (Demon.pointsVie <= 0)
        {
            //L'animation de sa mort joue
            GetComponent<Animator>().SetTrigger("mort");
            //Les squelettes sont detruits
            Destroy(gameObject);
        }
    }

    /*================================================
        Detection des collisions pour le combat
    ==================================================*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //Si le squelette prends du dommage et n'a plus de vie...
        if (Vie <= 0)
        {
            //L'animation de sa mort joue
            GetComponent<Animator>().SetTrigger("mort");
            //L'attaque ne peut plus etre activee
            GetComponent<Animator>().SetBool("attaque", false);
            //L'animation de marche ne peut plus etre active
            GetComponent<Animator>().SetBool("marche", false);
            //On invoque la fonction qui le detruit
            Invoke("MortFinale", 0.4f);
        } 
        //Si le squelette prends du dommage sans mourir...
        else if (infoCollision.gameObject.name == "boiteDeCollision")
        {
            //On joue l'animation ou le squelette prends du dommage
            GetComponent<Animator>().SetTrigger("dommage");
        }
    }

    /*=====================================
     *      Fonctions Supplementaires
     ======================================*/
    //Fonction de destruction du squelette apres quelques secondes (voire centiemes de secondes)
    void MortFinale()
    {
        Destroy(gameObject);
    }

    //Fonction pour l'activation de l'attaque
    void Attaque()
    {
        GetComponent<Animator>().SetBool("attaque", true);
    }

    //Pour qu'il y ai un peu de temps de recuperation entre chaque attaque
    void AnnulerAttaque()
    {
        GetComponent<Animator>().SetBool("attaque", false);
    }
}

       