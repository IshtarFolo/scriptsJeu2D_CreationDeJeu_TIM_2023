using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class deplacementPerso : MonoBehaviour
{
    /*==============================
        DECLARATION DE VARIABLES
    ================================*/
    /*---- Fade in du debut ----*/
    public GameObject fadeInIntro;//Variable pour contenir le rectangle de transition avec fade in
    private float transition = 2f;//La variable de transition qui servira a reduire progressivement les alpha de la couleur du rectangle

    /*--------------Varables pour le deplacement et les sauts--------------*/
    public static float vitesseX;//Variable deplacement axe des X
    public float vitesseY;//Variable pour saut
    public float vitesseXMax = 5f;//Variable pour stabiliser la vitesse X après un saut (le personnage n'est pas ralentis)
    public bool dansAir;//Variable qui detecte si le joueur est en l'air ou pas

    /*---------------Variables pour les vies-----------------*/
    public int nbPotionsVie = 0;//Compteur pour les potions de vie
    public static int VieMax = 100;//Maximum de vie que le personnage peut avoir
    public static int Vie;//Vie du personnage
    public GameObject effetPotion;//Variable pour l'animation de guerison
    public barreDeVie barreVie;//Pour contenir la barre de vie
    public GameObject barreVieBoss;//Pour gerer l'apparition de la barre de vie du boss
    public Image imageVieBoss;//Pour gerer l'apparition de la barre de vie du boss
    public static bool fadeIn;//Variable qui fait apparaitre la barre de vie du boss au contact du trigger de la salle

    /*--------------Variable attaque--------------*/
    public bool attaque = false;//Pour attaquer
    public static bool frappe = false;//Pour detecter si le joueur est frappe par un ennemi
    public float tempsAttaque = 0f;//Pour le temps de recuperation d'attaque

    /*----------------variables pour les objets reactifs dans le monde-----------------------*/
    public GameObject porteChateau;//Pour aller chercher l'objet porteChateau et faire jouer son animation
    public GameObject murInvisible;//Pour empecher le joueur de quitter la salle du boss

    /*-------------Variables cameras--------------*/
    public GameObject camMain;//Camera principale
    public GameObject camBoss;//Camera du boss

    /*--------------Variable pour la fin de la partie---------------*/
    public static bool partieTerminee;//pour gerer la fin de la partie

    /*------------Sons-----------------*/
    public AudioClip coupEpee;//Le son de l'attaque
    public AudioClip potion;//Le son de la potion lorsque le joueur la ramasse
    public AudioClip guerir;//Le son qui joue quand le joueur utilise une potion
    public AudioClip ouch;//Son qui joue quand le personnage se fait blesser
    public AudioClip sonMort;//Le son qui joue a la mort du personnage

    private bool sonJoue;//Variable qui determine si le son joue

    /*-------------Textes UI---------------*/
    public TextMeshProUGUI compteurPotions;//Pour recevoir la quantitee de potions comme parametre et la faire apparaitre en texte
    public TextMeshProUGUI nomBoss;//Pour le nom du boss
    public TextMeshProUGUI mort;//Pour le texte de fin de partie lorsque le joueur meurt
    public TextMeshProUGUI recommencer;//Texte qui dit comment recommencer le jeu
    public GameObject canvas;//Variable qui contient l'objet canvas

    Color controleAlpha;//Pour le controle de la couleur (surtout la transparence)
    Color controleAlphaMort;//Pour controler la couleur du texte de fin de partie

    private bool fadeInTexte;//Pour faire apparaitre le texte 
    private bool fadeInFin;//Pour faire apparaitre le texte de fin de parties

    /*-------------Variables pour transition fade to black avec le solide noir sur la zone de combat avec le boss--------------*/
    public GameObject noir;//Pour contenir l'objet fadeToBlack
    private float fadeOut = 0;//Pour modifier les valeurs des alpha du SpriteRenderer
    private float chronoFade;//Le compteur qui represente les valeurs a passer a la variable fadeOut
    private float decompteFin;//Le compteur pour compter le temps avant de faire passer l'ecran au noir
    public AudioSource musiqueBatailleFinale;//La musique pour le combat de la fin

    /*=======================================================
         Start is called before the first frame update
     ========================================================*/
    void Start()
    {
        Vie = VieMax;//En commencant la vie est a 100 points
        barreVie.AjusterVieMax();//On fait s'ajuster la valeur de la barre de vie au début et donc de mettre la vie du joueur a 100 points
        partieTerminee = false;//La partie commence et donc n'est pas finie

        /*-------Etat des cameras au depart-----*/
        //La camera qui suit le joueur est active et celle de la zone de bataille contre le Demon est desactivee
        camMain.SetActive(true);
        camBoss.SetActive(false);

        /*-----------Etat de la barre de vie du boss-----------*/
        //Au depart, le fade in est a false
        fadeIn = false;
        //Si le booleen fade in est false, l'animation d'apparition de la barre de vie du boss est desactivee 
        if (fadeIn == false)
        {
            //L'image de vie du boss n'apparait pas
            imageVieBoss.GetComponent<Animator>().enabled = false;
        }

        /*-------Le texte----------*/
        fadeInTexte = false;//Pour que le texte apparaisse
        fadeInFin = false;//Le texte n'apparait pas tant que la partie n'est pas finie (soit que la vie du joueur n'est pas a 0)

        controleAlpha = nomBoss.GetComponent<TextMeshProUGUI>().color;//La variable controleAlpha est associee a la couleur du texte
        controleAlphaMort = mort.GetComponent<TextMeshProUGUI>().color;//La variable controleAlphaMort gere la couleur du texte
        controleAlphaMort = recommencer.GetComponent<TextMeshProUGUI>().color;

        controleAlphaMort.a = 0f;//On donne 0 a l'alpha du texte de fin
        controleAlpha.a = 0f;//L'alpha est mis a 0

        nomBoss.GetComponent<TextMeshProUGUI>().color = controleAlpha;//On passe la nouvelle valeur au texte
        mort.GetComponent<TextMeshProUGUI>().color = controleAlphaMort;//On passe la nouvelle valeur au texte de fin
        recommencer.GetComponent<TextMeshProUGUI>().color = controleAlphaMort;//On passe les meme valeur pour l'opacite au texte pour recommencer

        sonJoue = false;//Le son de blessure ne joue pas au debut
    }

    /*=====================================
       Update is called once per frame
     =====================================*/
    void Update()
    {
        //On change la valeur de transition progressivement
        transition -= 0.01f;
        //On passe ses valeurs dans la couleur alpha du solide noir servant a la transition entre la scene d'intro et de debut du jeu
        fadeInIntro.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, transition);

        /*==============================
            MOUVEMENTS DU PERSONNAGE
        ================================*/

        //***********Pour courir de gauche à droite************

        //------------------Ecoute si on appuie sur "A" ou la fleche de gauche...-------------------
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //La vitesse change a -1 pour permettre les deplacement a gauche
            vitesseX = -2f * vitesseXMax;
            transform.eulerAngles = new Vector3(0, 180, 0);/*
                                                            pour la rotation du personnage sur l'axe des X sans avoir a replacer 
                                                            l'image de base, source pour l'information sur les "eulerAngles":
                                                            https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html.
                                                            */
        }

        //-------------------Ecoute si on appuie sur "D" ou la fleche de droite...--------------------
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //La vitesse change a 2 pour permettre le deplacement a droite et on la multiplie avec vitesseXMax pour qu'elle reste la meme apres un saut
            vitesseX = 2f * vitesseXMax;
            transform.eulerAngles = new Vector3(0, 0, 0);//la rotation est a 0 ici car on bouge vers la droite (comme le sprite de base)
        }
        else
        {
            //Retour a la valeur initiale
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        //********Pour sauter********
        //------------------Ecoute si on appuie sur "W" ou la fleche du haut...-------------------

        if (Input.GetKeyDown(KeyCode.W) && Physics2D.OverlapCircle(transform.position, 0.1f) || Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.OverlapCircle(transform.position, 0.1f))
        {
            //La vitesse change a 1 pour permettre le deplacement a droite
            vitesseY = 30f;
            //L'animation de saut est activee 
            GetComponent<Animator>().SetBool("saut", true);
            //L'animation de course est desactivee
            GetComponent<Animator>().SetBool("course", false);
        }

        //Pour l'animation ou le personnage tombe
        else if (GetComponent<Rigidbody2D>().velocity.y < -4 && Physics2D.OverlapCircle(transform.position, 0.1f) == false)
        {
            //L'animation du personnage qui tombe est activee
            GetComponent<Animator>().SetBool("tombe", true);
            //Les animations de course et de saut passent a false
            GetComponent<Animator>().SetBool("saut", false);
            GetComponent<Animator>().SetBool("course", false);

            //On fait retomber le personnage 
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }
        //Si le personnage touche le sol
        else if (Physics2D.OverlapCircle(transform.position, 0.1f)) 
        {
            GetComponent<Animator>().SetBool("tombe", false);
        }
        else
        {
            //Retour a la valeur initiale
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }

        //On passe les variables et leur valeurs a l'objet a l'aide d'un nouveau Vecteur
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

        /*===================
            POUR ATTAQUER
        =====================*/
        //Si la variable d'attaque est false...
        if (attaque == false)
        {
            //Le temps augmente
            tempsAttaque += Time.deltaTime;

            //--------------------Si on appuie sur la barre espace...--------------
            if (Input.GetKeyDown(KeyCode.Space) && tempsAttaque <= 0.7f && partieTerminee == false)
            {
                //L'animation est activee
                GetComponent<Animator>().SetTrigger("attaque");
                //Permet de sauter en attaquant MAIS perso revient en mode idle
                GetComponent<Animator>().SetBool("saut", false);
                //On donne un temps de recuperation pour l'attaque
                attaque = true;
                //Le son de l'attaque joue
                GetComponent<AudioSource>().PlayOneShot(coupEpee, 1.5f);
            }
        }

        //Si la variable d'attaque est true...
        if (attaque == true)
        {
            //Le temps d'attaque augmente
            tempsAttaque += Time.deltaTime;
            //Si le temps d'attaque est plus grand ou egal a 1.5f...
            if (tempsAttaque >= 1f)
            {
                //Si on relache la touche espace...
                attaque = false;
                //Le chrono est reparti
                tempsAttaque = 0f;
            }
        }
        //Sinon...
        else
        {
            //Le temps d'attaque est a 0
            tempsAttaque = 0f;
        }
        
        /*==================================================
            POUR UTILISER UNE POTION DE VIE ET SE GUERIR
        ====================================================*/
        //--------------Le joueur appuie sur "Q"----------------

        //Pour faire apparaitre la quantitee de potions en possession du joueur a l'ecran
        compteurPotions.text = ": " + nbPotionsVie.ToString();//On passe l'information retenue sous nbPotionsVie en int
                                                              //sous la forme d'une "String" dans le champs de texte

        /*Si le nombre de potions de vie est plus grand que 0 et que la vie du joueur n'est pas egale au nombre maximal de vie et que le joueur appuie sur "Q"...*/
        if (nbPotionsVie > 0 && Vie != VieMax && Input.GetKeyDown(KeyCode.Q) && partieTerminee == false)
        {
            //On gagne 10 points de vie
            Vie = Mathf.Clamp(Vie + 30, 0, 100);
            //l'animation de guerison est activee
            effetPotion.SetActive(true);
            //L'effet de guerison prends fin avec l'invocation de la fonction 
            Invoke("Guerison", 0.75f);
            //Le nombre de potions est diminu� de 1
            nbPotionsVie--;
            //Le son de guerison joue
            GetComponent<AudioSource>().PlayOneShot(guerir, 0.5f);
        }
        //Si le joueur meurt il n'a plus de potions
        else if (Vie == 0 && partieTerminee == true)
        {
            nbPotionsVie = 0;
            //La vie du demon revient au maximum
            Demon.pointsVie = Demon.vieMax;
        }

        /*----------------------Animations de la course----------------------*/
        if (vitesseX > 0.1f || vitesseX < -0.1f)
        {
            //L'animation est activee
            GetComponent<Animator>().SetBool("course", true);
        }
        else
        {
            //L'animation est desactivee et on revient a "Idle"
            GetComponent<Animator>().SetBool("course", false);
        }

        /*-------------------------Mort du personnage------------------------*/
        //Si la vie du joueur tombe a 0...
        if (Vie <= 0)
        {
            //Son animation de mort joue
            GetComponent<Animator>().SetBool("mort", true);
            //Arrete le mouvement du personnage
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //Empeche le mouvement lateral du personnage
            transform.eulerAngles = new Vector3(0, 0, 0);
            //On arrete les animations de saut qui pourraient jouer lorsqu'on appuie sur W ou la fleche du haut 
            GetComponent<Animator>().SetBool("saut", false);
            GetComponent<Animator>().SetBool("tombe", false);
            fadeInFin = true;

            if (partieTerminee == false)
            {
                //Le son de mort joue
                GetComponent<AudioSource>().PlayOneShot(sonMort, 0.4f);
            }

            //La partie prends fin
            partieTerminee = true;

            //Si on appuie sur R...
            if (Input.GetKey(KeyCode.R))
            {
                //On invoque la fonction qui fait recommencer le jeu
                Invoke("RelancerLeJeu", 0.5f);
            }
        }

        //Si le personnage tombe et que sa position devient trop basse...
        if (GetComponent<Rigidbody2D>().transform.position.y <= -14f)
        {
            //Pour eviter que la vie du joueur ne continue a descendre a la mort
            Vie = Mathf.Clamp(Vie - 100, 0, 100);
            //Si le joueur appuie sur R...
            if (Input.GetKey(KeyCode.R))
            {
                //On invoque la fonction qui fait recommencer le jeu
                Invoke("RelancerLeJeu", 0.5f);
            }
        }

        /*----------------------------Ouverture de la porte du chateau--------------------------------*/
        //Lorsque le personnage est assez proche la porte s'ouvre
        if (GetComponent<Rigidbody2D>().transform.position.x >= 91f)
        {
            //On fait jouer l'animation
            porteChateau.GetComponent<Animator>().enabled = true;
        }
        else
        {
            //Sinon elle ne joue pas
            porteChateau.GetComponent<Animator>().enabled = false;
        }

        /*----------------------------Si le joueur est frappe par un adversaire-------------------------------*/
        //Si le booleen frappe est vrai....
        if (frappe == true)
        {
            //On active l'animation de dommage
            GetComponent<Animator>().SetBool("blesse", true);
        }
        //Sinon, on la desactive
        else
        {
            GetComponent<Animator>().SetBool("blesse", false);
        }

        barreVie.AjusterVie();//On dit au script de la barre de vie de s'ajuster pendant l'"update" et donc a chaque fois que le joueur se gueri ou prends du dommage

        //Le texte apparait si le booleen fadeInTexte est true
        if (fadeInTexte == true)
        {
            //Sa transparence est augmentee progressievement
            controleAlpha.a += 0.007f;
            //On passe les valeurs de controleAlpha a la couleur du texte
            nomBoss.GetComponent<TextMeshProUGUI>().color = controleAlpha;
        }

        /*-------Pour faire apparaitre le texte de fin de partie lorsque le joueur perds-------*/
        //Si le fade in de la fin est false...
        if (fadeInFin == true)
        {
            controleAlphaMort.a += 0.001f;
            mort.GetComponent<TextMeshProUGUI>().color = controleAlphaMort;
            recommencer.GetComponent<TextMeshProUGUI>().color = controleAlphaMort;
        }

        /*--------------------La mort du boss et le changement de scene------------------------*/
        //Si la partie prends fin et que la vie du boss est plus basse ou egale a 0...
        if (Demon.finDePartie == true && Demon.pointsVie <= 0)
        {
            //On part le compteur qui determine quand partir l'effet de "fade to black"
            decompteFin += Time.deltaTime;

            //Si le compteur de la fin atteint 2 secondes après la mort du boss...
            if (decompteFin >= 2f)//A VOIR!!!!
            {
                //On rends les elements du canvas invisibles en le desactivant
                canvas.SetActive(false);
                //On augmente progressivement la valeur des alpha du solide noir 
                chronoFade += 0.001f;
                //On associe la variable fadeOut au chronometre servant a modifier la valeur de l'alpha du solide
                fadeOut += chronoFade;
                //Finalement, on passe la valeur de la variable fadeOut au SpriteRenderer
                noir.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, fadeOut);
                musiqueBatailleFinale.GetComponent<AudioSource>().volume -= 0.001f;
            }
            //Si l'écran passe au noir....
            if (noir.GetComponent<SpriteRenderer>().color.a >= 255f)
            {
                //On charge la scene de fin
                SceneManager.LoadScene(2);
            }
        }
    }

    /*=================================
     *      Detection des Collisions
     ==================================*/
    /*--------A la sortie de la collision---------*/
    void OnCollisionExit2D(Collision2D infoCollision)
    {
        //Si le personnage quitte la plateforme il n'est plus son enfant
        transform.parent = null;
        //La vitesse X maximale revient a la normale lorsque le joueur quitte une plateforme
        vitesseXMax = 5f;
    }

    /*----------------Fonction activee lors de l'entree en collisions----------------*/
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f) == false && Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) && Physics2D.OverlapCircle(transform.position, 0.1f) == false)
        {
            //L'animation de saut est false lorsqu'on touche un collider
            GetComponent<Animator>().SetBool("saut", false);
            vitesseY = -5f;
        }

        //Si le personnage entre en collision avec une potion de vie...
        if (infoCollision.gameObject.name == "potionVie")
        {
            //Il la ramasse et le systeme la garde en memoire pour une future utilisation
            nbPotionsVie += 1;
            //L'objet disparait
            infoCollision.gameObject.SetActive(false);
            //Le son de ramassage de la potion joue
            GetComponent<AudioSource>().PlayOneShot(potion, 0.6f);
        }

        //Si le personnage tombe dans le trou avec des pointes au fond
        if (infoCollision.gameObject.name == "trouAvecPointes")
        {
            //On enleve toute sa vie
            Vie = Mathf.Clamp(Vie - 100, 0, 100);
        }

        //Si le personnage entre en contact avec la plateforme...
        if (infoCollision.gameObject.tag == "plateforme")
        {
            //Il devient son enfant et suit son mouvemenent
            transform.parent = infoCollision.gameObject.transform;
            //La vitesse X sur les plateformes est modifiee pour permettre au joueur de se deplacer plus vite sur celles-ci
            vitesseXMax = 10f;
        }
    }

    /*==============================
          Detection des Trigger
     ===============================*/
    /*----------------Changement de camera par trigger pour la bataille contre le Boss---------------------*/
    void OnTriggerEnter2D(Collider2D infoTrigger)
    {
        //On detecte le collider de la deuxieme camera
        if (infoTrigger.gameObject.name == "triggerCamBoss")
        {
            //On desactive la camera sur le personnage et on active la camera sur la zone de la bataille contre le Demon
            camMain.SetActive(false);
            camBoss.SetActive(true);

            //Le booleen fade in devient true
            fadeIn = true;//Pour activer l'effet d'apparition de la barre de vie du boss
            //La variable pour faire apparaitre le texte passe a true
            fadeInTexte = true;
            //le mur invisible a l'entree de la salle du boss est active et empeche le joueur de revenir en arriere
            murInvisible.GetComponent<EdgeCollider2D>().enabled = true;
            
            //Si la camera de la salle du boss est active...
            if (camBoss.activeInHierarchy == true)
            {
                //On change la position x et y de l'objet vide pour l'animation de guerison pour eviter que l'objet ne se deplace au changement de camera  
                effetPotion.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

                //Si le fade in est true...
                if (fadeIn == true)
                {
                    //On active l'animation d'apparition de la barre de vie du demon
                    imageVieBoss.GetComponent<Animator>().enabled = true;
                }
            }
        }

        /*---------Si le joueur entre en contact avec l'attaque ou le boss crache du feu...--------*/
        if (infoTrigger.gameObject.name == "attaqueFeu")
        {
            //Il perds 20 points de vie
            Vie -= 20;
            //le booleen frappe devient true
            frappe = true;
            //On invoque la fin de l'animation de coup recut
            Invoke("RecoitCoup", 0.2f);
            //On fait jouer le son de blessure du joueur
            GetComponent<AudioSource>().PlayOneShot(ouch, 0.5f);
        }

        /*---------Si le joueur entre en contact avec un projectile du boss...----------*/
        if (infoTrigger.gameObject.tag == "projectile")
        {
            //Il perd de la vie equivalente au dommage du projectile
            Vie -= projectile.dommage;
            //La variable frappe est true
            frappe = true;
            //On invoque la fonction de recuperation du coup qui remet la variable frappe a false
            Invoke("RecoitCoup", 0.2f);
            //Le son peut jouer
            sonJoue = true;
            //Si le son peut jouer...
            if (sonJoue == true)
            {
                //Le son de dommage recut joue
                GetComponent<AudioSource>().PlayOneShot(ouch, 0.5f);
            }
            else
            {
                //Sinon on remet la variable a false 
                sonJoue = false;
            }
        }
    }

    /*====================================
           Fonctions Supplementaires
     =====================================*/
    //On relance le jeu si le personnage meurt
    void RelancerLeJeu()
    {
        SceneManager.LoadScene(1);
    }

    //Pour l'animation de guerison avec les potions
    void Guerison()
    {
        //L'animation de guerison joue
        effetPotion.SetActive(false);
    }

    //Pour l'animation ou le joueur recoit du dommage
    void RecoitCoup()
    {
        frappe = false;
    }
}
