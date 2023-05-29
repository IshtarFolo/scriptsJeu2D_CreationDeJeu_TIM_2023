using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class controleTexte : MonoBehaviour
{
    /*================================
     *           Variables
     =================================*/
    public TextMeshProUGUI controles;//Pour le texte au bas de l'ecran

    Color controleAlpha;//Pour controler la couleur

    private float compteur;//La minuterie
    private float tempsMax;//La limite de temps pour faire apparaitre et disparaitre le texte
    private bool compteurRepart;//La variable booleene pour dire si le compteur repart ou non

    // Start is called before the first frame update
    void Start()
    {
        //On associe la variable controleAlpha a la couleur du TextMeshProUGUI
        controleAlpha = controles.GetComponent<TextMeshProUGUI>().color;
        //Le alpha de la couleur est a 0 en partant
        controleAlpha.a = 0f;
        //La limite de temps maximale est mise a 0.5 au depart
        tempsMax = 0.5f;
        //Le compteur n'est pas reparti en debutant
        compteurRepart = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Le compteur suit le temps reel
        compteur += Time.deltaTime;

        //Si le compteur est plus grand ou egal au temps maximum
        if (compteur >= tempsMax)
        {
            //L'opacitee du texte est a 0
            controleAlpha.a = 0f;
            //controleAlpha est associe au GetComponent de la couleur du texte
            controles.GetComponent<TextMeshProUGUI>().color = controleAlpha;
            //On invoque la fonction pour repartir le compteur
            Invoke("RelancerCompteur", 0.3f);
        }
        //Si le compteur est reparti...
        else if (compteurRepart == true)
        {
            //L'opacitee est a 1
            controleAlpha.a = 1f;
            //On associe a nouveau la nouvelle valeur de controleAlpha au GetComponent de la couleur du texte
            controles.GetComponent<TextMeshProUGUI>().color = controleAlpha;
            //Le compteur n'est pas reparti
            compteurRepart = false;
        }
    }

    /*=======================================
     *      Fonctions Supplementaires
     ========================================*/
    void RelancerCompteur()
    {
        //Le compteur revient a 0
        compteur = 0;
        //Le compteur est reparti
        compteurRepart = true;
    }

}
