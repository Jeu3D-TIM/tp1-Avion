using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionMissile : MonoBehaviour
{
    public GameObject missile;
    public ParticleSystem explosionLancementMissile;
    public ParticleSystem flammeMissile;
    public float vitesseMissile;
    private bool peutTirer; 

    // Start is called before the first frame update
    void Start()
    {
        peutTirer = true;
        missile.SetActive(false);
        explosionLancementMissile.Pause();
        flammeMissile.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Tir();
        }
    }

    //Fonction pour dupliquer et lancer le missile ---------- LE MISSILE PAR DANS LA MAUVAISE DIRECTION (parte sur la gauche de l'Avion tout le temps)
    //en tournant le missile de 90degrer, il part dans la bonne direction mais visuellement le missile est de cote
    void Tir()
    {
        peutTirer = false;
        Invoke("ReactiverTir", 0.1f);

        explosionLancementMissile.Play();
        flammeMissile.Play();

        
        GameObject nouveauMissile = Instantiate(missile, missile.transform.position, missile.transform.rotation);
        nouveauMissile.SetActive(true);
        nouveauMissile.GetComponent<Rigidbody>().velocity = nouveauMissile.transform.forward * vitesseMissile;
    }

    //Fonction pour reactiver le tir apres un tire (eviter la repetition)
    void ReactiverTir()
    {
        peutTirer = true;
        explosionLancementMissile.Pause();
    }
}
