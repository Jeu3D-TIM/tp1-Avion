//Script pour controler les bouton dans la scene d'introduction pour pouvoir changer de scene et demarrer le jeu.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionUI_intro : MonoBehaviour
{
    public Button boutonTitre;
    public Button boutonJouer;
    public Button boutonCosmetique;
    public Button boutonParametre;

    // Start is called before the first frame update
    void Start()
    {
        boutonTitre.onClick.AddListener(RetourAcceuil);

        boutonJouer.onClick.AddListener(DeplacementBoutonAcceuil);
        boutonJouer.onClick.AddListener(AfficherChoixNiveau);

        boutonParametre.onClick.AddListener(DeplacementBoutonAcceuil);
        boutonCosmetique.onClick.AddListener(DeplacementBoutonAcceuil);

    }

    // fonction qui afficher le choix des niveaux a l'utilisateur ainsi que ses precedents records
    public void AfficherChoixNiveau()
    {

    }

    //fonction qui deplace les 3 boutons de l'acceuil pour faire place a l'autre interface
    public void DeplacementBoutonAcceuil()
    {
        boutonJouer.GetComponent<Animator>().SetBool("deplacement", true);
        boutonCosmetique.GetComponent<Animator>().SetBool("deplacement", true);
        boutonParametre.GetComponent<Animator>().SetBool("deplacement", true);
    }

    //fonction qui remet les boutons de l'acceuil
    public void RetourAcceuil()
    {
        boutonJouer.GetComponent<Animator>().SetBool("deplacement", false);
        boutonCosmetique.GetComponent<Animator>().SetBool("deplacement", false);
        boutonParametre.GetComponent<Animator>().SetBool("deplacement", false);
    }

    void OnDisable()
    {
        boutonJouer.onClick.RemoveListener(DeplacementBoutonAcceuil);
    }
}
