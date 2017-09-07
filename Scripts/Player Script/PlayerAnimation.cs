using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {



    private Animation anim;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animation>();
	}
	public void DidJump()
    {
        anim.Play(Tags.ANIMATION_JUMP);
        //After Jump ENDS
        anim.PlayQueued(Tags.ANIMATION_JUMP_FALL);

    }
    public void DidLand()
    {
        anim.Stop(Tags.ANIMATION_JUMP_FALL);
        anim.Stop(Tags.ANIMATION_JUMP_LAND);
        //After Jump ENDS
        anim.Blend(Tags.ANIMATION_JUMP_LAND, 0);
        anim.CrossFade(Tags.ANIMATION_RUN);


    }
    public void PlayerRun()
    {
        anim.Play(Tags.ANIMATION_RUN);


    }

}
