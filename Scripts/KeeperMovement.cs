using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeeperMovement : MonoBehaviour {

	//Player variables
	public bool hasGlass;
	public int slot1;

	//Navigation variables
	public UnityEngine.AI.NavMeshAgent agent;
	bool hasPath;

	//Raycast info variables
	RaycastHit hit;

	//UI variables
	public RawImage image;

	//Animation variables
	public GoblinAnimation anim;

	//Awake Method
	void Awake(){

		slot1 = -1;

		DisplayMugs ();

	}

	//Update Method
	void Update(){
	
		if (GameController.game.isPlaying && !GameController.game.isPaused) {
			if (Input.GetMouseButtonDown(0)) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast(ray, out hit)) {

					if (hit.transform.tag == "Cask" || hit.transform.tag == "Glass" || hit.transform.tag == "GlassSupport") {
						MoveCharacter(hit.transform.position, hit);
					}

				}

			}

			if (!agent.pathPending)
			{
				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
					{
						if (hasPath) {
							hasPath = false;
							ManageGlass (hit);
							anim.ChangeAnimation (false);
						}
					}
				}
			}

			DisplayMugs ();
		}
	}

	//Display mugs
	void DisplayMugs(){

		 if (slot1 == -1) {//No mug
			//image.texture = GameController.game.emptyHand;
			Color a = image.color;
			a.a = 0;
			image.color = a;
		} else {

			Color a = image.color;
			a.a = 1;
			image.color = a;

			if (slot1 == 0) {//Empty mug
				image.texture = GameController.game.mugEmpty;
			} else if (slot1 == 1) {
				image.texture = GameController.game.mug1;
			}else if (slot1 == 2) {
				image.texture = GameController.game.mug2;
			}else if (slot1 == 3) {
				image.texture = GameController.game.mug3;
			}
		}

	}

	void ManageGlass(RaycastHit _hit){

		if (hit.transform.tag == "Cask") {//Cask
			Cask(_hit);
		}else if (hit.transform.tag == "Glass") {//Glass mat
			Glass(_hit);
		}else if (hit.transform.tag == "GlassSupport") {//Glass support
			GlassSupport (_hit);
		}

	}

	void Cask(RaycastHit _hit){

		if (hasGlass && slot1 == 0) {
			Cask cask = _hit.transform.gameObject.GetComponent<Cask> ();
			slot1 = cask.beer;
		}

	}

	void Glass(RaycastHit _hit){

		GlassMat glass = _hit.transform.gameObject.GetComponent<GlassMat> ();
		if (glass.hasGlass) {
			/*if (!hasGlass && glass.isSide && slot1 == -1) {
				glass.TakeGlass ();
				hasGlass = true;
				slot1 = 0;
			}*/
		} else {
			if (hasGlass && slot1 > 0) {
				if (!glass.isSide) {
					glass.GiveGlass (slot1);
					hasGlass = false;
					slot1 = -1;
				} else if (glass.transform.FindChild ("Seat").GetComponent<Seat>().hasSomeone) {
					glass.ServeClient (slot1);
					hasGlass = false;
					slot1 = -1;
				}
			}
		}
	}

	void GlassSupport(RaycastHit _hit){
	
		if (!hasGlass) {
			hasGlass = true;
			slot1 = 0;
		}

	}

	//Move character to destination
	void MoveCharacter(Vector3 _target, RaycastHit _hit){

		agent.SetDestination (_target);
		hasPath = true;
		anim.ChangeAnimation (true);

	}

}
