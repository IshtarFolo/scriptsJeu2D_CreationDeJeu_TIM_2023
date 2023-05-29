using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class projectile : MonoBehaviour
{
    /*============================
     *          Variables
     =============================*/
    public GameObject perso;//La variable contenant le joueur
    public float distance;//La variable de la distance entre le joueur et le projectile 
    private float vitesse;//Variable de la vitesse de deplacement du projectile 

    public static int dommage = 15;//Pour le dommage lors de la collision avec le projectile

    // Start is called before the first frame update
    void Start()
    {
        //la vitesse de deplacement du projectile est mise a 3f
        vitesse = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //Un nouveau Vecteur 2 est cree pour calculer la distance entre le personnage et le projectile
        distance = Vector2.Distance(transform.position, perso.transform.position);
        //On invoque la fonction ou le projectile suit le joueur apres 1 seconde
        Invoke("SuivrePersonnage", 1f);

        /*-------Si le projectile est trop proche du terrain...--------*/
        if (transform.position.y <= -3.4f)
        {
            //On bloque son mouvement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //L'animation d'explosion joue
            GetComponent<Animator>().SetTrigger("explosion");
            //On detruit le projectile
            Destroy(gameObject, 0.6f);
        }

        /*------Si le Demon n'a plus de vie...-------*/
        if (Demon.pointsVie <= 0)
        {
            //On bloque son mouvement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //L'animation d'explosion joue
            GetComponent<Animator>().SetTrigger("explosion");
            //On desactive le collider
            GetComponent<CircleCollider2D>().enabled = false;
            //On detruit les projectiles restants
            Destroy(gameObject, 0.6f);
        }

        if (deplacementPerso.Vie <= 0)
        {
            //On bloque son mouvement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //L'animation d'explosion joue
            GetComponent<Animator>().SetTrigger("explosion");
            //On desactive le collider
            GetComponent<CircleCollider2D>().enabled = false;
            //On detruit les projectiles restants
            Destroy(gameObject, 0.6f);
        }
    }

    /*================================
     *      Detection des Trigger
     =================================*/
    /*-------------Si le projectile entre en contact avec le joueur...---------------*/
    void OnTriggerEnter2D(Collider2D infoTrigger)
    {
        if (infoTrigger.gameObject.tag == "Player")
        {
            //L'animation d'explosion joue
            GetComponent<Animator>().SetTrigger("explosion");
            //On desactive le collider
            GetComponent<CircleCollider2D>().enabled = false;
            //On bloque son mouvement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //Celui-ci est detruit
            Destroy(gameObject, 0.6f);
        }
    }

    /*=====================================
     *      Fonctions Supplementaires
     ======================================*/
    //Fonction pour poursuivre le personnage
    void SuivrePersonnage()
    {
        //Si la position du personnage est plus petite que 20 et plus grande que 0... 
        if (distance < 20 && distance > 0)
        {
            //Pour que le projectile suive le personnage joueur
            transform.position = Vector2.MoveTowards(this.transform.position, perso.transform.position, vitesse * Time.deltaTime);

            //Pour que le projectile fasse face a la direction du personnage
            if (perso.transform.position.x >= transform.position.x)
            {
                //Le sprite du projectile se retourne
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                //Sinon il reprend sa position
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
