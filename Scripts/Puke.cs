using UnityEngine;
using System.Collections;

public class Puke : MonoBehaviour {

	//Puke variables
	public float timeToDestroy;

	//Animation variables
	public RuntimeAnimatorController animator;

	public Animator anim;

	//Start Method
	void Start(){

		StartCoroutine (Fade ());

	}

	//Fade puke
	IEnumerator Fade(){
	
		yield return new WaitForSeconds (timeToDestroy);

		anim.Play ("PukeFade");

		yield return new WaitForSeconds (1.0f);

		Destroy (gameObject);	

	}

}
