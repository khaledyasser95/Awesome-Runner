using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour {

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.HEALTH_TAG || target.tag== Tags.PLATRFORM_TAG || target.tag == Tags.MONSTER_TAG)
        {
            target.gameObject.SetActive(false);
        }
    }
	
}//class
