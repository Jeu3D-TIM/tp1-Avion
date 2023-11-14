//Script des commandes de l'Avion ( SHIFT gauche pour avance, W pour descendre, S pour monter, A pour touner a gauche, D pour tourner a droite, E pour changer de caméra, ESPACE pour lancer les missiles).
//Gestion des collisions en lien avec l'avion. Gestion du chronometre générale du niveau. Gestion de la réussite du Niveau 1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;

public class ControleAvion2 : MonoBehaviour
{
    [Header("Vitesse de l'avion")]
    public float vitesseAvant;
    public float vitesseAvantRecommender;
    public float vitesseAvantMax;
    public float vitesseAvantMin;
    public float rotationYGauche;
    public float rotationYDroite;
    public float rotationZMonter;
    public float rotationZDescendre;
    public bool peutJouer;

    [Header("Composants de l'avion")]
    public Rigidbody rigidAvion;
    public bool sonAvion;

    [Header("Composant relier a l'avion")]
    public static float chronometre;
    public float milisecondes;
    public int secondes;
    public int minutes;
    //particule
    public ParticleSystem explosion;
    public ParticleSystem splash;
    public ParticleSystem explosionNuage;
    //en lien avec la course
    public GameObject ligneDepart;
    public GameObject checkPoint;
    public GameObject prochainCheckPoint1;//pour le niveau 1
    public GameObject prochainCheckPoint2;//pour le niveau 2
    public GameObject[] tousLesCheckPoints;
    public List<GameObject> checkpointsTraverses;
    public int indexCheckpoints;
    public GameObject resetCheckpoint;
    //camera
    public GameObject cameraPilot;
    public GameObject camera3emPersonne;

    [Header("Référence au objets")]
    public GameObject TMPchronometre;
    public GameObject AvionAnim;
    public GameObject medaileOr;
    public GameObject medaileArgent;
    public GameObject medaileBronze;
    public GameObject flecheCheckPoint;



    // Start is called before the first frame update
    void Start()
    {
        rigidAvion.GetComponent<Rigidbody>();
        TMPchronometre.GetComponent<TextMeshProUGUI>();
        chronometre = 0;
        checkPoint.transform.position = ligneDepart.transform.position;
        peutJouer = true;
        medaileOr.SetActive(false);
        medaileArgent.SetActive(false);
        medaileBronze.SetActive(false);

        explosion.GetComponent<ParticleSystem>();
        splash.GetComponent<ParticleSystem>();
        explosionNuage.GetComponent<ParticleSystem>();

        explosion.Pause();
        splash.Pause();
        explosionNuage.Pause();

        //faire en sorte que tout les checkpoints du niveau 1 rentre dans le tableau pour ensuite verifier si ils ont ete traverses
        if(SceneManager.GetActiveScene().name == "niveau1")
        {
            tousLesCheckPoints = GameObject.FindGameObjectsWithTag("checkPoint");
        }else if(SceneManager.GetActiveScene().name == "niveau2")
        {
            tousLesCheckPoints = GameObject.FindGameObjectsWithTag("checkPoint2");
        }
        tousLesCheckPoints.OrderBy(cp => cp.name).Select(cp => cp.transform).ToArray();
        Array.Sort(tousLesCheckPoints, (cp1, cp2) => string.Compare(cp1.name, cp2.name, StringComparison.Ordinal));
        indexCheckpoints = 0;

        // creer un liste pour mettre tous les checkpoints traverser 
        List<GameObject> checkpointsTraverses = new List<GameObject>();

        //son 
        sonAvion = true;
    }

    private void Update()
    {
        //---------------------------------------------- Reset au dernier check point et au depart --------------------------------//
        //remettre l'Avion au dernier checkpoint qu'elle a franchit
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = resetCheckpoint.transform.position;
            rotationZMonter = 0;
            rotationZDescendre = 0;
        }
        //redemarrer le niveau (compteur = 0 et position de l'avion au debut) recommencer a 0
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (SceneManager.GetActiveScene().name == "niveau1")
            {
                SceneManager.LoadScene("niveau1");
            }
            else if (SceneManager.GetActiveScene().name == "niveau2")
            {
                SceneManager.LoadScene("niveau2");
            }
        }
        //------------------------------------------------- Indication du circuit ----------------------------------------------//
        // la fleche pointe vers le haut puis qunad on est vraimment proche du CP elle se met a tourner juste un peu
        if(SceneManager.GetActiveScene().name == "niveau1")
        {
            flecheCheckPoint.transform.LookAt(prochainCheckPoint1.transform.position);
        }else if(SceneManager.GetActiveScene().name == "niveau2")
        {
            flecheCheckPoint.transform.LookAt(prochainCheckPoint2.transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //----------------------------- Verification ------------------------------//
        //verifier quand la vitesse est plus grande que le max
        if(vitesseAvant > vitesseAvantMax) {vitesseAvant = vitesseAvantMax;}
        if(vitesseAvant < vitesseAvantMin) {vitesseAvant=vitesseAvantMin;}
        //----------------------------- Transform --------------------------------//
        transform.Translate(vitesseAvant, 0f, 0f);

        //pour faire monter l'avion
        transform.Rotate(0f, 0f, rotationZMonter);
        transform.Rotate(0f, 0f, rotationZDescendre);

        //pour faire tourner l'avion
        transform.Rotate(0f, rotationYGauche, 0f);
        transform.Rotate(0f, rotationYDroite, 0f);

        //----------------------------------------- Controle Avion pour Clavier ---------------------------------//
        if(peutJouer == true)
        {
            //partir le son de l'avion
            if(sonAvion == true)
            {
                GetComponent<AudioSource>().Play();
                sonAvion = false;
            }

            //pour faire avancer l'avion 
            if (Input.GetKey(KeyCode.LeftShift))
            {
                vitesseAvant += 0.1f;
            }
            else
            {
                vitesseAvant -= 0.01f;
            }

            //pour faire monter l'avion
            if (Input.GetKey(KeyCode.S))
            {
                rotationZMonter += 0.01f;
                if(rotationZMonter > 75f) {rotationZMonter = 75f;}
                if (vitesseAvant > vitesseAvantRecommender) {vitesseAvant += 0.00001f;}
            }
            else
            {
                rotationZMonter -= 0.05f;
                if( rotationZMonter < 0) {rotationZMonter = 0;}
            }

            //pour faire dessandre l'avion
            if (Input.GetKey(KeyCode.W))
            {
                rotationZDescendre -= 0.01f;
                if(rotationZDescendre < -75F) {rotationZDescendre = -75f;}
                if (vitesseAvant > vitesseAvantRecommender) { vitesseAvant += 0.0001f; }
            }
            else
            {
                rotationZDescendre += 0.05f;
                if( rotationZDescendre > 0) {rotationZDescendre = 0;}
            }

            //pour faire tourner l'avion à GAUCHE
            if (Input.GetKey(KeyCode.A))
            {
                rotationYGauche -= 0.01f;
                AvionAnim.GetComponent<Animator>().SetBool("rotationGauche", true);
            }
            else
            {
                rotationYGauche += 0.01f;
                if (rotationYGauche >= 0f) {rotationYGauche = 0f;}
                AvionAnim.GetComponent<Animator>().SetBool("rotationGauche", false);
            }

            //pour faire tourner l'avion à DROITE
            if (Input.GetKey(KeyCode.D))
            {
                rotationYDroite += 0.01f;
                AvionAnim.GetComponent<Animator>().SetBool("rotationDroite", true);
            }
            else
            {
                rotationYDroite -= 0.01f;
                if(rotationYDroite <= 0f) {rotationYDroite = 0f;}
                AvionAnim.GetComponent<Animator>().SetBool("rotationDroite", false);
            }

            //pour changer de caméra
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(camera3emPersonne.activeSelf)
                {
                    camera3emPersonne.SetActive(false);
                    cameraPilot.SetActive(true);
                }else if (cameraPilot.activeSelf)
                {
                    camera3emPersonne.SetActive(true);
                    cameraPilot.SetActive(false);
                }
            }
        }

    }

    //--------------------------------------------------- Colision -----------------------------------------------------------------//
    public void OnTriggerEnter(Collider infoCollision)
    {
        //une fois que l'avion traverse la ligne de départ, on part un chronometre pour suivre le temps du joueur
        if(infoCollision.gameObject.name == "ligneDepart")
        {
            InvokeRepeating("MilliSeconde", 0f, 0.001f);
        }

        //afficher le temps lorsque l'avion passe dans un checkpoint + verifier si l'Avion est passe dans le checkpoin + changer la cible de la fleche
        if(infoCollision.gameObject.tag == "checkPoint" || infoCollision.gameObject.tag == "checkPoint2" && !checkpointsTraverses.Contains(infoCollision.gameObject))
        {
            TMPchronometre.GetComponent<TextMeshProUGUI>().text = minutes.ToString() + " : " + secondes.ToString() + " : " + milisecondes.ToString();
            Invoke("AffichageTemps", 1.5f);
            checkPoint.transform.position = infoCollision.transform.position;
            Invoke("ProchainCheckpoint", 0f);
            indexCheckpoints++;
            checkpointsTraverses.Add(infoCollision.gameObject);
            resetCheckpoint.transform.position = infoCollision.gameObject.transform.position ;
        }

        //detection de la fin du parcour ----- afficher le temps, proposer d'autres niveaux ou de rejouer.
        if(infoCollision.gameObject.name == "ligneArrivee" && checkpointsTraverses.Count == tousLesCheckPoints.Length)
        {
            CancelInvoke("MilliSeconde");
            TMPchronometre.GetComponent<TextMeshProUGUI>().text = minutes.ToString() + " : " + secondes.ToString() + " : " + milisecondes.ToString();


            if(SceneManager.GetActiveScene().name == "niveau1")
            {
                //niveau 1 en bas de 1minutes 40secondes -- medaile or
                if(minutes < 1 && secondes <= 60)
                {
                    medaileOr.SetActive(true);
                    medaileOr.GetComponent<Animator>().SetBool("victoire", true);
                }
                //niveau 1 en bas de 2minutes 10secondes mais plus haut que 2minutes 40secondes -- medaile argent
                else if(minutes > 1 && minutes <= 2 && secondes <= 30)
                {
                    medaileArgent.SetActive(true);
                    medaileArgent.GetComponent<Animator>().SetBool("victoire", true);
                }
                //niveau 1 en haut de 2 minutes 30secondes -- medaile bronze
                else if( minutes >=2 && minutes <= 3&& secondes >=30 )
                {
                    medaileBronze.SetActive(true);
                    medaileBronze.GetComponent<Animator>().SetBool("victoire", true);
                }
            }else if(SceneManager.GetActiveScene().name == "niveau2")
            {
                //niveau 2 en bas de 1minutes 40secondes -- medaile or
                if (minutes < 1 && secondes <= 60)
                {
                    medaileOr.SetActive(true);
                    medaileOr.GetComponent<Animator>().SetBool("victoire", true);
                }
                //niveau 2 en bas de 2minutes 10secondes mais plus haut que 2minutes 40secondes -- medaile argent
                else if (minutes > 1 && minutes <= 2 && secondes <= 30)
                {
                    medaileArgent.SetActive(true);
                    medaileArgent.GetComponent<Animator>().SetBool("victoire", true);
                }
                //niveau 2 en haut de 2 minutes 30secondes -- medaile bronze
                else if (minutes >= 2 && minutes <= 3 && secondes >= 30)
                {
                    medaileBronze.SetActive(true);
                    medaileBronze.GetComponent<Animator>().SetBool("victoire", true);
                }
            }

        }else if (infoCollision.gameObject.name == "ligneArrivee" && checkpointsTraverses.Count != tousLesCheckPoints.Length)
        {
            TMPchronometre.GetComponent<TextMeshProUGUI>().text = "Vous n'avez pas traverser tous les checkpoints!";
            Invoke("AffichageTemps", 1.5f);
        }

        //detecter une collision avec le bord de la map
        if(infoCollision.gameObject.tag == "nuage")
        {
            explosionNuage.Play();
            Invoke("ReloadScene", 2f);
        }

        //detecter une collision avec un nuage deans le ciel 
        if(infoCollision.gameObject.tag == "particuleNuage")
        {
            explosionNuage.Play();
        }
    }
    private void OnCollisionEnter(Collision infoCollision)
    {
        //detection de la collision avec le terrain
        if (infoCollision.gameObject.name == "Terrain" || infoCollision.gameObject.tag == "planche")
        {
            peutJouer = false;
            explosion.Play();
            Invoke("ReloadScene", 2f);
        }

        //detection de la collision avec l'eau
        if(infoCollision.gameObject.name == "eau")
        {
            peutJouer = false;
            splash.Play();
            Invoke("ReloadScene", 2f);
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

    //--------------------------- Gestion des checkpoint ---------------------------//
    public void ProchainCheckpoint()
    {
        //passer au prochain checkpoint
       if(indexCheckpoints < tousLesCheckPoints.Length) 
       {
            if (SceneManager.GetActiveScene().name == "niveau1")
            {
                prochainCheckPoint1 = tousLesCheckPoints[indexCheckpoints];
            }else if (SceneManager.GetActiveScene().name == "niveau2")
            {
                prochainCheckPoint2 = tousLesCheckPoints[indexCheckpoints];
            }
       }
    }

    //------------------------ Redemarer la scene --------------------------------//
    public void ReloadScene()
    {
        if (SceneManager.GetActiveScene().name == "niveau1")
        {
            SceneManager.LoadScene("niveau1");
        }
        else if (SceneManager.GetActiveScene().name == "niveau2")
        {
            SceneManager.LoadScene("niveau2");
        }
        GetComponent<AudioSource>().Pause();
    }
}
