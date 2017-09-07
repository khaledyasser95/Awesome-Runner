using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public float offsetZ = -15f;
    public float offsetX = 5f;
    public float constantY = 5f;
    public float CameraLerpTime = 0.05f;
    private Transform playerTarget;
    // Use this for initialization
    void Awake () {
        playerTarget = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerTarget)
        {
            Vector3 targetPosition = new Vector3(playerTarget.position.x+offsetX,constantY,playerTarget.position.z+offsetZ);

            transform.position = Vector3.Lerp(transform.position, targetPosition, CameraLerpTime);

        }
		
	}
}
