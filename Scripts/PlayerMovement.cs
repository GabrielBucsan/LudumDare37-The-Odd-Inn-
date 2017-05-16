using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	//Player variables
	public float speed;

	public int[] slot;

	public bool isWalking;
	bool changedAnimation;

	bool isSlipping;

	//UI variables
	public RawImage[] image;

	//Client variables
	List<Collider> clientColliders = new List<Collider> ();

	//Animation Variables
	public GoblinAnimation anim;

	//Model variable
	public GameObject model;

	//Start Method
	void Start(){

		slot = new int[2];

		for (int i = 0; i < slot.Length; i++) {
			slot [i] = -1;
		}

		DisplayMugs ();

	}

	//FixedUpdate Method
	void FixedUpdate(){

		if (GameController.game.isPlaying && !GameController.game.isPaused) {
			//Movement
			if (!isSlipping) {
				transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * Time.fixedDeltaTime * speed, 0, Input.GetAxisRaw ("Vertical") * Time.fixedDeltaTime * speed));

				//Animation control
				if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {

					Vector3 direction = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
					model.transform.rotation = Quaternion.LookRotation (direction);

					if (!isWalking) {
						changedAnimation = true;
					}
					isWalking = true;
				} else {
					if (isWalking) {
						changedAnimation = true;
					}
					isWalking = false;
				}

				if (changedAnimation) {
					changedAnimation = false;
					anim.ChangeAnimation (isWalking);
				}

				//Checks player input
				if (Input.GetKeyDown (KeyCode.Q) && slot[0] != -1) {//Left slot

					Collider coll = CheckClients ();

					if (coll != null) {
						if (!coll.GetComponent<Client>().isLeaving) {
							coll.GetComponent<Client> ().GetOrder (slot [0]);
							slot [0] = -1;
						}
					}
				}

				if (Input.GetKeyDown (KeyCode.E) && slot[1] != -1) {//Right slot

					Collider coll = CheckClients ();

					if (coll != null) {
						if (!coll.GetComponent<Client>().isLeaving) {
							coll.GetComponent<Client> ().GetOrder (slot [1]);
							slot [1] = -1;
						}
					}
				}
			}

			//Displays mugs
			DisplayMugs();
		}

	}

	//Display mugs
	void DisplayMugs(){

		for (int i = 0; i < slot.Length; i++) {
			 if (slot [i] == -1) {
				//image [i].texture = GameController.game.emptyHand;
				Color a = image[i].color;
				a.a = 0;
				image [i].color = a;
			} else {

				Color a = image[i].color;
				a.a = 1;
				image [i].color = a;

				if (slot [i] == 0) {//Empty mug
					image [i].texture = GameController.game.mugEmpty;
				} else	if (slot[i] == 1) {
					image [i].texture = GameController.game.mug1;
				}else if (slot [i] == 2) {
					image [i].texture = GameController.game.mug2;
				}else if (slot [i] == 3) {
					image [i].texture = GameController.game.mug3;
				}
			}
		}

	}

	//OnTriggerEnter Method
	void OnTriggerEnter(Collider _collider){

		if (_collider.tag == "PickArea") {
		
			GlassMat glassMat = _collider.transform.parent.GetComponent<GlassMat> ();

			if (glassMat.hasGlass && !glassMat.isSide) {

				Glass glass = glassMat.transform.GetComponentInChildren<Glass> ();

				if (slot [0] == -1) {
					slot [0] = glass.actual;
					glassMat.TakeGlass ();
				} else if (slot [1] == -1) {
					slot [1] = glass.actual;
					glassMat.TakeGlass ();
				}
			}
		}else if (_collider.tag == "GiveArea") {

			GlassMat glassMat = _collider.transform.parent.GetComponent<GlassMat> ();

			if (!glassMat.hasGlass && glassMat.isSide) {

				if (slot[0] == 0) {
					glassMat.GiveGlass (0);
					slot [0] = -1;
				}else if (slot[1] == 0) {
					glassMat.GiveGlass (0);
					slot [1] = -1;
				}

			}
		}else if (_collider.tag == "Client") {
	
			clientColliders.Add (_collider);

		}else if (_collider.tag == "Puke") {
			StartCoroutine (Slip ());
		}

	}

	//OnTrigerExit Method
	void OnTriggerExit(Collider _collider){

		if (_collider.tag == "Client") {

			clientColliders.Remove (_collider);

		}

	}

	//Checks clients
	Collider CheckClients(){

		for (int i = 0; i < clientColliders.Count; i++) {
			if (clientColliders[i].GetComponent<Client>().isOrdering) {
				return clientColliders [i];
			}
		}

		return null;

	}

	//Slipps
	IEnumerator Slip(){

		isSlipping = true;
		anim.anim.Play ("Slipping");

		yield return new WaitForSeconds (2);

		isSlipping = false;

	}
}
