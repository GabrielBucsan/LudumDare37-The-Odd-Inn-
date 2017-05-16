using UnityEngine;
using System.Collections;

public class VikingAnimation : MonoBehaviour {

	//Animation variables
	public RuntimeAnimatorController animator;

	public Animator anim;

	//Start Method
	void Start(){

		//anim = GetComponent<Animator> ();
		anim.runtimeAnimatorController = animator;

	}

	//Change Walk state
	public void ChangeWalkState(bool _state){

		anim.SetBool ("isWalking", _state);

	}

	//Change Seat state
	public void ChangeSeatState(bool _state){

		anim.SetBool ("isSeated", _state);

	}

}
