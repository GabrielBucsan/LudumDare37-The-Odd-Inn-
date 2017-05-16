using UnityEngine;
using System.Collections;

public class GoblinAnimation : MonoBehaviour {

	//Animation variables
	public RuntimeAnimatorController animator;

	public Animator anim;

	//Start Method
	void Start(){

		anim = GetComponent<Animator> ();
		anim.runtimeAnimatorController = animator;

	}

	//Change goblin animation
	public void ChangeAnimation(bool _state){

		anim.SetBool ("isWalking", _state);

	}

}
