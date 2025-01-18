using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplossionParticle : MonoBehaviour
{
 
    void Start()
    {
        Destroy(gameObject,1.3f);
    }

  
}
