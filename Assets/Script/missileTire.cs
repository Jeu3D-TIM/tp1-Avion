//script pour detecter la collision des missiles avec les objects. Si ils touchent quelque chose, ils vont se détruire.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileTire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision infoCollision)
    {
        if(infoCollision.gameObject.tag == "planche")
        {
            Destroy(infoCollision.gameObject);
            Destroy(gameObject);
        }

        if(infoCollision.gameObject.name == "Terrain" || infoCollision.gameObject.name == "eau" 
           || infoCollision.gameObject.tag == "nuage" || infoCollision.gameObject.tag == "checkPoint" 
           || infoCollision.gameObject.name == "ligneArrivee" || infoCollision.gameObject.name == "ligneDepart")
        {
            Destroy(gameObject);
        }
    }
}
