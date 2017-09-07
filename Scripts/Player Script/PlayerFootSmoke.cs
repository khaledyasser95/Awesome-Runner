using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSmoke : MonoBehaviour {
    public GameObject smokeEffect;
    public GameObject smokeposition;
      
	void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.PLATRFORM_TAG)
        {
           if (smokeposition.activeInHierarchy)
            {
                Instantiate(smokeEffect, smokeposition.transform.position, Quaternion.identity); 
            }
        }
    }
}//class
