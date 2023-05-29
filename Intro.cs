using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    /*===============================
     *          Variables
     ================================*/
    /*----Objets----*/
    public GameObject nuages;//Pour controller les nuages (1 et 2)
    public GameObject nuages2;
    public GameObject fadeOut;//Variable pour le solide 2d du fade lors du debut du jeu
    public Canvas leCanvas;//Variable pour le canvas et tout le texte

    /*----Musique----*/
    AudioSource Audio;//Pour la musique du debut

    private bool commencerJeu = false;//Variable qui determine si on commence le jeu ou non
    private float decompteDebut;//Variable pour creer la transition entre le debut et la scene principale de jeu


    // Start is called before the first frame update
    void Start()
    {
        /*----Musique----*/
        //On associe la variable Audio au GetComponent
        Audio = GetComponent<AudioSource>();
        //On fait jouer la musique
        Audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Pour bouger les nuages, on leur donne une vitesse de translation sur l'axe des X de 0.01f
        nuages.transform.Translate(0.005f, 0, 0);

        //Si les nuages depassent le cadre de la camera...
        if (nuages.transform.position.x >= 19f)
        {
            //Leur position est ramenee de l'autre cote du cadre et leur position sur l'axe des Y est determinee au hasard entre 1f et -1f
            nuages.transform.position = new Vector2(-9f, Random.Range(1f, -1f));
        }

        //Meme modification au mouvement pour les autres nuages de l'ecran d'acceuil du jeu
        nuages2.transform.Translate(0.005f, 0, 0);

        if (nuages2.transform.position.x >= 19f)
        {
            //Leur position est ramenee de l'autre cote du cadre et leur position sur l'axe des Y est determinee au hasard entre 1f et -1f
            nuages2.transform.position = new Vector2(-9f, Random.Range(1f, -1f));
        }


        //Pour commencer le jeu...
        //Le joueur doit apppuyer sur la touche "espace"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //On commence le jeu
            commencerJeu = true;
        }
        //Si on commence le jeu...
        if (commencerJeu == true)
        {
            //Le decompte du debut demarre
            decompteDebut += Time.deltaTime;
            //On fait disparaitre l'interface d'intro progressivement
            fadeOut.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, decompteDebut);
            //Le canvas est desactive
            leCanvas.enabled = false;
            //Le son de la musique descend progressivement
            GetComponent<AudioSource>().volume -= 0.001f;
        }

        //Le jeu commence et on charge la scene principale
        if (commencerJeu == true && decompteDebut >= 1f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
