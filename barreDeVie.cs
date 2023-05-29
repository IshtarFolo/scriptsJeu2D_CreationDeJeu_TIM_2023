using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Realise a l'aide du tutoriel de "Brackeys" sur Youtube:
//https://www.youtube.com/watch?v=BLfNP4Sc_iA&t=602s&ab_channel=Brackeys

public class barreDeVie : MonoBehaviour
{
    public Slider ajusteur;//On fait du "slider" dans unity un objet publique qui sera associe a l'objet vide barre de vie

     public void AjusterVieMax()
    {
        /*
          ici on cree une fonction pour recevoir l'information du script deplacementPerso qui ajustera la vie maximale 
          via la variable du script mentionne ci-dessus
         */ 
        ajusteur.maxValue = deplacementPerso.VieMax;
        ajusteur.value = deplacementPerso.VieMax;
    }

    public void AjusterVie()
    {
        //Et on fait la meme chose pour la vie du personnage lors des fonctions d'Update
        ajusteur.value = deplacementPerso.Vie;
    }

    //Les deux fonctions sont ensuite appelees dans le script deplacementPerso
}
