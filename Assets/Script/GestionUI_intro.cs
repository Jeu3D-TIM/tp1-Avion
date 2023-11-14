//Script pour controler les bouton dans la scene d'introduction pour pouvoir changer de scene et demarrer le jeu.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestionUI_intro : MonoBehaviour
{
    public Button boutonJouer;
    public Button boutonCosmetique;
    public Button boutonParametre;

    // Start is called before the first frame update
    void Start()
    {
        boutonJouer.GetComponent<Button>();
        boutonJouer.onClick.AddListener(BoutonJouerAppuyer);
    }

    public void BoutonJouerAppuyer()
    {
        SceneManager.LoadScene("jeu");
    }

    void OnDisable()
    {
        boutonJouer.onClick.RemoveListener(BoutonJouerAppuyer);
    }
}
