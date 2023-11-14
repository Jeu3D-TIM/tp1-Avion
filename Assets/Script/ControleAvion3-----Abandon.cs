using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class ControleAvion3 : MonoBehaviour
{
    [Header("Vitesse de l'avion")]
    public float vitesseAvant;
    public float vitesseAvantMax;
    public float vitesseAvantMin;
    public float vitesseMonterDescendre;
    public float vitesseTourner;


    [Header("Composants de l'avion")]
    public Rigidbody rigidAvion;

    [Header("Composant relier a l'avion")]
    public static float chronometre;
    public float milisecondes;
    public float secondes;
    public float minutes;
    public GameObject checkPoint;
    public GameObject ligneDepart;

    [Header("Référence au objets")]
    public GameObject TMPchronometre;

    // Start is called before the first frame update
    void Start()
    {
        rigidAvion.GetComponent<Rigidbody>();
        TMPchronometre.GetComponent<TextMeshProUGUI>();
        chronometre = 0;
        checkPoint.transform.position = ligneDepart.transform.position;
        
    }

    private void Update()
    {
        //---------------------------------------------- Reset au dernier check point et au depart --------------------------------//
        //remettre l'Avion au dernier checkpoint qu'elle a franchit
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = Vector3.MoveTowards(transform.position, checkPoint.transform.position, 1f);
        }

        //------------------------------------------------- Controle Avion pour Clavier ---------------------------------------------//
        vitesseTourner = Input.GetAxis("Horizontal")*vitesseAvant;
        vitesseMonterDescendre = Input.GetAxis("Vertical")*vitesseAvant;

        if (Input.GetKey(KeyCode.W))
        {
            vitesseAvant += 0.1f;
        }
        else if(vitesseAvant > 0)
        {
            vitesseAvant -= 0.01f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidAvion.AddRelativeTorque(0f, 0f, vitesseMonterDescendre);

        transform.Translate(vitesseAvant, 0f, 0f);

    }

    //--------------------------------------------------- Colision -----------------------------------------------------------------//
    public void OnTriggerEnter(Collider infoCollision)
    {
        //une fois que l'avion traverse la ligne de départ, on part un chronometre pour suivre le temps du joueur
        if(infoCollision.gameObject.name == "ligneDepart")
        {
            InvokeRepeating("MilliSeconde", 0f, 0.001f);
        }

        //afficher le temps lorsque l'avion passe dans un checkpoint
        if(infoCollision.gameObject.tag == "checkPoint")
        {
            TMPchronometre.GetComponent<TextMeshProUGUI>().text = minutes.ToString() + " : " + secondes.ToString() + " : " + milisecondes.ToString();
            Invoke("AffichageTemps", 1.5f);
            checkPoint.transform.position = infoCollision.transform.position;
        }

        //detection de la fin du parcour ----- afficher le temps, proposer d'autres niveaux ou de rejouer.
        if(infoCollision.gameObject.name == "ligneArrivee")
        {
            CancelInvoke("MilliSecondex#");
            TMPchronometre.GetComponent<TextMeshProUGUI>().text = minutes.ToString() + " : " + secondes.ToString() + " : " + milisecondes.ToString();
        }

        //dection de la position par dessus une surface d'eau (tres proche de l'eau) ---- faire des éclaboussures
        if(infoCollision.gameObject.name == "eau")
        {
            print("test");
        }
    }


    //------------------------------ Gestion du chronometre ------------------------//
    public void MilliSeconde()
    {
        milisecondes += 0.001f;
        if(milisecondes >= 1)
        {
            milisecondes = 0;
            secondes += 1;
        }
        if(secondes >= 60)
        {
            secondes = 0;
            minutes += 1;
        }
    }

    //--------------------------- Reset affichage temps -----------------------------//
    public void AffichageTemps()
    {
        TMPchronometre.GetComponent<TextMeshProUGUI>().text = "";
    }
}
