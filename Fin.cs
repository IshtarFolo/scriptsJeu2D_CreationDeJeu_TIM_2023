using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fin : MonoBehaviour
{
    /*============================
     *      LES VARIABLES        *
     =============================*/
    //Pour controller le defilement du paysage
    public GameObject foret;

    /*----- Pour controller l'apparition de la scene avec un effet de "fade in" -----*/
    public GameObject carreNoirFade;//Le carre noir servant a reveler la scene
    private float chronoFade = 0f;//Le chronometre permettant de controller le fade in
    private float fadeIn = 2f;//La valeur de base du fade in

    /*----- Controle des credits -----*/
    public GameObject credits;//Le texte transforme en objet 
    private float positionY = -596f;//La position Y de base du texte 
    public GameObject controleRetour;//Le texte disant d'appuyer sur espace pour recommencer
    private float apparition = 0f;//La variable faisant apparaitre le texte pour montrer les derniers controles au joueur

    /*----- Le personnage -----*/
    public GameObject perso;//Le sprite du personnage
    private float positionX = -0.09f;//La position X du personnage

    /*=================================
     *        LES FONCTIONS           *
     ==================================*/
    // Update is called once per frame
    void Update()
    {
        /*----- Pour l'effet de "fade in" -----*/
        //On part le chronometre
        chronoFade += Time.deltaTime;
        //Si le chronometre est plus grand ou egal a 0...
        if (chronoFade >= 0) {
            //La scene se revele progressivement avec la reduction des valeurs alpha de la couleur du carre noir devant la camera 
            fadeIn -= Time.deltaTime;
            //On passe les valeur de couleur au bon composant
            carreNoirFade.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, fadeIn);
        }

        /*----- Pour deplacer le decor, le personnage et faire apparaitre les controles pour le retour au debut -----*/
        //Le decor se deplace lentement par modification de la propriete translate
        foret.GetComponent<SpriteRenderer>().transform.Translate(0.01f, 0f, 0f);

        //Si le decor atteint la position X de 54f...
        if (foret.GetComponent<SpriteRenderer>().transform.position.x >= 54f)
        {
            //Le decor arrete de se deplacer sur l'axe des X
            foret.GetComponent<SpriteRenderer>().transform.position = new Vector3(54f, 0f, 0f);
            //La position X descend graduellement de 0.05f
            positionX -= 0.02f;
            //Le personnage commence a se deplacer jusqu'a quitter le cadre 
            perso.GetComponent<SpriteRenderer>().transform.position = new Vector3(positionX, -5.08f, 0f);

            //Si le personnage depasse le cadre de la camera...
            if (perso.GetComponent<SpriteRenderer>().transform.position.x <= -15f)
            {
                //Il s'arrete
                perso.GetComponent<SpriteRenderer>().transform.position = new Vector3(-15f, -5.08f, 0f);
                //L'alpha du texte change peu a peu avec le temps reel
                apparition += Time.deltaTime;
                //On fait apparaitre le texte indiquant au joueur comment revenir a l'ecran principal
                controleRetour.GetComponent<TextMeshProUGUI>().color = new Color(255f, 255f, 255f, apparition);
                //Le son des pas du personnage diminuent tranquillement pour donner l'mpression qu'il s'eloigne
                perso.GetComponent<AudioSource>().volume -= Time.deltaTime;
            }
            //Si le joueur appuie sur espace... 
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                //On recharge la scene du debut/ intro
                SceneManager.LoadScene(0);
            }
        }

        /*----- Pour le mouvement des credits -----*/
        //La position avance avec les secondes
        positionY += Time.deltaTime;
        //On ajuste la position des credits 
        credits.GetComponent<RectTransform>().anchoredPosition = new Vector3(-163f, positionY,0f);
    }
}
