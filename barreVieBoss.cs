using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barreVieBoss : MonoBehaviour
{
    public Slider ajusteurVie;//Pour associer le slider a l'objet vide barre de vie

    /*Fonction qui ajuste la vie maximale du boss*/
    public void AjusterMaxVie()
    {
        ajusteurVie.maxValue = Demon.vieMax;
        ajusteurVie.value = Demon.vieMax;
    }

    /*fonction qui ajuste la vie du demon pendant la partie*/
    public void AjusterVie()
    {
        ajusteurVie.value = Demon.pointsVie;
    }
}
