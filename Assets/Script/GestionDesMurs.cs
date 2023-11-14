//Script qui génere un nombre aléatoire et qui active 1 mur sur 3 qu'on doit détruire pour acceder au checkpoint.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionDesMurs : MonoBehaviour
{
    public int numeroPlancheDetruire;
    public GameObject plancheADetruire1;
    public GameObject plancheADetruire2;
    public GameObject plancheADetruire3;
    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------------ Mur actif ---------------------------------------------------------//
        //generer un nemero pour qu'une seul planche sur 3 soit active
        numeroPlancheDetruire = Random.Range(0, 3);
        if (numeroPlancheDetruire == 0)
        {
            plancheADetruire1.SetActive(true);
            plancheADetruire2.SetActive(false);
            plancheADetruire3.SetActive(false);
        }
        else if (numeroPlancheDetruire == 1)
        {
            plancheADetruire1.SetActive(false);
            plancheADetruire2.SetActive(true);
            plancheADetruire3.SetActive(false);
        }
        else if (numeroPlancheDetruire == 2)
        {
            plancheADetruire1.SetActive(false);
            plancheADetruire2.SetActive(false);
            plancheADetruire3.SetActive(true);
        }
    }
}

