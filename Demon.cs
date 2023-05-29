using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Demon : MonoBehaviour
{
    /*=======================
     *      Variables
     ========================*/
    private float compteur;//variable pour creer un delai entre les attaques du demon

    /*----------Variables pour la vie---------------*/
    public static int pointsVie;//Vie du demon
    public static int vieMax = 100;//Vie maximale du demon
    public barreVieBoss barreVieBoss;//Variable pour contenir la barre de vie du boss

    /*--------Variables pour la fin de la partie---------*/
    public static bool finDePartie = false;

    /*------------Variables par rapport au joueur----------------*/
    public GameObject joueur;//variable du personnage joueur
    public float distance;//variable qui contient la distance entre le joueur et le boss

    /*-------------------Variables des projectiles-------------------------*/
    public GameObject projectiles;//Variable contenant le projectile
    private float compteurSpawn;//Compteur pour gerer l'intervalle entre le clonage de projectiles
    private float projectilesMax = 5f;//La variable limite a laquelle s'arrete le compteur
    private bool peutTirer;//La variable qui dit si le boss peut tirer un projectile ou non

    /*-----------Variables pour la deuxieme phase: Le boss invoque des squelettes-------------*/
    public GameObject Squelette;//L'objet squelette
    private float compteurSpawnSquelettes;//Le compteur qui permet d'invoquer des squelettes
    private float squelettesMax = 5f;//Le maximum que peut atteindre le compteur
    private bool peutInvoquer;//Variable qui permet au boss d'invoquer des squelettes ou non

    // Start is called before the first frame update
    void Start()
    {
        //La variable vie au depart est a 100
        pointsVie = vieMax;
        //La partie recommence
        finDePartie = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*Si la partie n'est pas terminée...*/
        if (finDePartie == false)
        {
            barreVieBoss.AjusterMaxVie();
        }

        //Un nouveau Vecteur 2 est cree pour calculer la distance entre le joueur et le boss
        distance = Vector2.Distance(transform.position, joueur.transform.position);

        //Pour que le demon se retourne si le joueur est derriere lui
        if (joueur.transform.position.x >= transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Sinon il reprend sa position
            transform.eulerAngles = new Vector3(0, 0, 0);
        }


        /*===============================
         *     Pour cracher du feu
         ================================*/
        //Si la partie n'est pas finie et que la distance entre le joueur et le boss est plus petite que 20 mais plus grande que 1...
        if (finDePartie == false)
        {
            if (distance < 5 && distance > 0)
            {
                //Le compteur augmente avec le temps qui passe
                compteur += Time.deltaTime;

                /*Si le compteur atteint 4 secondes...*/
                if (compteur >= 3f)
                {
                    //On invoque la fonction pour faire cracher du feu au demon
                    Invoke("cracherFeu", 0.2f);
                    //Fonction qui arrete l'animation pour faire cracher du feu au demon
                    Invoke("ArretFeu", 0.4f);
                    //Le compteur repart a 0 
                    compteur -= 3f;
                }
            }

            /*======================================
             *      Pour tirer des projectiles
             =======================================*/
            //Si la distance entre le joueur et le boss est plus petite que 20 et plus grande que 0
            if (distance < 24 && distance > 0)
            {
                //Le boss peut tirer des projectiles
                peutTirer = true;

                //Si le boss peut tirer et que le compteur des projectiles est a 0
                if (peutTirer == true && compteurSpawn == 0)
                {
                    //On invoque la fonction de clonage de projectiles apres 1 seconde
                    Invoke("TirerProjectile", 1f);
                }
                //Si le compteur atteint la variable maximale des projectiles, soit 5f
                if (compteurSpawn >= projectilesMax)
                {
                    //La variable peutTirer devient false
                    peutTirer = false;
                    //Le compteur repart a 0
                    compteurSpawn = 0;
                }
            }
            //Si la variable peutTirer est true...
            if (peutTirer == true)
            {
                //Le compteur demarre et augmente avec le temps reel
                compteurSpawn += Time.deltaTime;
            }

            /*==============================================
             *        Pour Invoquer des squelettes
             ===============================================*/
            //Si la vie du boss est plus petite ou egale a 50...
            if (pointsVie <= 50)
            {
                //Le boss peut invoquer des squelettes
                peutInvoquer = true;

                //Si le boss peut invoquer et que le compteur est a 0...
                if (peutInvoquer == true && compteurSpawnSquelettes == 0)
                {
                    //La fonction d'invocation des squelettes est activee apres 1 seconde 
                    Invoke("SpawnSquelettes", 1f);
                }
                //Si le compteur atteint sa limite maximale...
                if (compteurSpawnSquelettes >= squelettesMax)
                {
                    //Le boss ne peut plus invoquer des squelettes
                    peutInvoquer = false;
                    //Le compteur repart a 0
                    compteurSpawnSquelettes = 0;
                }
                //Si la variable peutInvoquer est true...
                if (peutInvoquer == true)
                {
                    compteurSpawnSquelettes += Time.deltaTime;
                }
            }

            //Si le personnage joueur a 0 de vie...
            if (deplacementPerso.Vie <= 0)
            {
                //La partie prends fin
                finDePartie = true;
                //Le compteur s'arrete
                compteur = 0;
            }

        barreVieBoss.AjusterVie();//Pour ajuster la taille de la barre de vie pendant la partie
        }

        else if (finDePartie == true)
        {
            //On empeche le demon d'activer son attaque de feu
            GetComponent<Animator>().SetBool("Feu", false);
        }

        /*---------La mort du boss-----------*/

        //si la vie du demon atteint 0 ou plus bas...
        if (pointsVie <= 0)
        {
            //La partie est finie
            finDePartie = true;
            //Le demon ne peut plus suivre le joueur
            transform.eulerAngles = new Vector3(0, 0, 0);
            //Le collider est desactive
            GetComponent<Collider2D>().enabled = false;
            //L'animation de mort est activee
            GetComponent<Animator>().SetBool("mort", true);

            GetComponent<Animator>().SetBool("Feu", false);
            //On detruit le personnage a la fin de l'animation
            Invoke("MortFinale", 0.5f);
        }
    }

            /*=======================================
             *      Fonctions des supplementaires
             ========================================*/

        /*---fonction pour faire cracher du feu au demon---*/
        void cracherFeu()
        {
            //Activation de l'animation ou le demon crache du feu
            GetComponent<Animator>().SetBool("Feu", true);
        }
        /*---Fonction d'arret de l'animation pour faire cracher du feu---*/
        void ArretFeu()
        {
            GetComponent<Animator>().SetBool("Feu", false);
        }

        /*---Fonction pour la mort definitive du boss---*/
        void MortFinale()
        {
        //Destruction du boss apres son anmation de mort
        Destroy(gameObject);
        }

        /*---Fonction qui clone les projectiles---*/
        void TirerProjectile()
        {
            //Clonage du projectile
            GameObject cloneProjectile = Instantiate(projectiles);

            //Position du projectile clone, il apparait entre les coordonnees 172f et 162f sur l'axe des X et 6f sur l'axe des Y
            cloneProjectile.transform.position = new Vector3(Random.Range(172f, 162f), 6f, 0f);

            //Activation du nouvel objet cree
            cloneProjectile.SetActive(true);

            //Le projectile est detruit apres 15 secondes
            Destroy(cloneProjectile, 15f);
        }
        
        /*----Fonction pour cloner des squelettes-----*/
        void SpawnSquelettes()
        {
        //Clonage du projectile
        GameObject cloneSquelette = Instantiate(Squelette);

        //Position du projectile clone, il apparait entre les coordonnees 172f et 162f sur l'axe des X et 6f sur l'axe des Y
        cloneSquelette.transform.position = new Vector3(Random.Range(166f, 159f), -3.18f, 0f);

        //Activation du nouvel objet cree
        cloneSquelette.SetActive(true);

        //Le projectile est detruit apres 15 secondes
        Destroy(cloneSquelette, 30f);
        }
    }