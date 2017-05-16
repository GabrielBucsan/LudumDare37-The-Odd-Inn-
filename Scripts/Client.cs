using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Client : MonoBehaviour {

	//Client variables
	public bool isBusy;
	public bool drink;//if client drinks or dance

	public bool isOrdering;
	public int order;
	public bool seated;
	public bool isLeaving;

	public bool isDancing;

	public float actionChance;
	public float timeToAction;
	private float timeToNextAction;

	public int state;

	public int id;

	//Navigation Variables
	public UnityEngine.AI.NavMeshAgent agent;
	public bool hasPath;

	float timeWalking;

	//UI variables
	public Canvas canvas;
	public RawImage imageBeer;
	public RawImage imageState;

	//Seat Reference
	public Seat actualSeat;

	//Animation variables
	public VikingAnimation anim;

	//Beer variables
	[SerializeField]
	int drunkNumber;
	int toHappy;

	int probPuke;

	//Time to leave variables
	public float timeTolerance;
	float timeToBad;

	//Start Method
	void Start(){

		probPuke = Random.Range (2, 4);

		timeToNextAction = Time.time + 5;

		state = 3;

	}

	//Update Method
	void Update(){

		if (GameController.game.isPlaying && !GameController.game.isPaused) {
			float doSomething = Random.Range (0.0f, 1.0f);

			if (!drink) {//Dancing
				//Verifies if client reached destination
				if (!agent.pathPending) {
					if (agent.remainingDistance <= agent.stoppingDistance) {
						if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
							if (hasPath) {
								hasPath = false;
								isBusy = false;
								anim.ChangeWalkState (false);
							}
						}
					}
				}

				if (doSomething < actionChance && !isBusy && timeToNextAction < Time.time && !isLeaving) {//Do something

					DoSomethingWalk ();

				}

			} else {
				//Verifies if client reached destination
				if (!agent.pathPending) {
					if (agent.remainingDistance <= agent.stoppingDistance) {
						if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
							if (hasPath) {
								hasPath = false;
								isBusy = false;
								anim.ChangeWalkState (false);
								seated = true;
								anim.ChangeSeatState (true);
								if (isLeaving) {//Leaves bar
									if (state == 1) {
										GameController.game.DecreaseLife ();
									}
									GameController.game.RemoveClient (id);
									FindObjectOfType<SpawnClient> ().actualNumber--;
									Destroy (gameObject);
								}

								FaceCenter ();
							}
						}
					}
				}

				if (doSomething < actionChance && !isBusy && !isOrdering && timeToNextAction < Time.time && !isLeaving) {//Do something
					DoSomethingDrink();
				}
			}

			if (timeWalking < Time.time && hasPath && !isLeaving) {
				hasPath = false;
				MoveClient ();
			}

			DisplayUI ();
			VerifyState ();
			CheckTime ();
		}
	}

	//Moves client to random place
	public void MoveClient(){

		Vector3 wayPoint = Random.insideUnitSphere*20;
		wayPoint.y = 0;
		anim.ChangeSeatState (false);
		anim.ChangeWalkState (true);
		agent.SetDestination (wayPoint);
		hasPath = true;
		isBusy = true;
		seated = false;
		timeWalking = Time.time + 20f;

	}

	//Sets order
	void SetOrder(){

		if (!isOrdering && order == 0) {
			isOrdering = true;
			isBusy = true;
			order = Random.Range (1, 4);
			timeToBad = Time.time + timeTolerance;
		}

	}

	//Get order
	public void GetOrder(int _beer){

		if (_beer == order) {
			order = 0;
			isBusy = false;
			isOrdering = false;
			timeToNextAction = Time.time + timeToAction;
			drunkNumber++;
			toHappy++;
			GameController.game.IncreaseScore ();
		} else {
			order = 0;
			state--;
			toHappy = 0;
			isBusy = false;
			isOrdering = false;
			timeToNextAction = Time.time + timeToAction;
			if (state == 1) {
				LeaveBar ();
			}
		}

	}

	//Client leaves bar
	void LeaveBar(){
		
		isLeaving = true;
		isBusy = true;
		hasPath = true;
		seated = false;
		agent.SetDestination (GameController.game.door.position);
		anim.ChangeSeatState (false);
		anim.ChangeWalkState (true);
		if (state == 1) {
			actualSeat.hasSomeone = false;
			actualSeat.client = null;
		}

	}

	//Face center of table
	void FaceCenter(){

		if (!actualSeat.isSide) {
			Table table = actualSeat.transform.parent.GetComponent<Table> ();

			transform.LookAt (table.transform);
		} else {
			Transform keeper = FindObjectOfType<KeeperMovement>().transform;

			transform.LookAt (keeper);
		}

	}

	//Displays mugs
	void DisplayUI(){

		//Display mug
		if (order == 0) {//Empty hand
			//imageBeer.texture = GameController.game.emptyHand;
			Color a = imageBeer.color;
			a.a = 0;
			imageBeer.color = a;
		}else {

			Color a = imageBeer.color;
			a.a = 1;
			imageBeer.color = a;

			if (order == 1) {
				imageBeer.texture = GameController.game.mug1;
			}else if (order == 2) {
				imageBeer.texture = GameController.game.mug2;
			}else if (order == 3) {
				imageBeer.texture = GameController.game.mug3;
			}
		}

		//Display state
		if (state == 3) {
			imageState.texture = GameController.game.happy;
		}else if (state == 2) {
			imageState.texture = GameController.game.normal;
		}else if (state == 1) {
			imageState.texture = GameController.game.mad;
		}else if (state == 4) {
			imageState.texture = GameController.game.sick;
		}

	}

	//Dance
	void Dance(){

		int rand = Random.Range (1, 10);

		if (rand == 1) {
			anim.anim.Play ("Dance1");
		}else if (rand == 2) {
			anim.anim.Play ("Dance2");
		}else if (rand == 3) {
			anim.anim.Play ("Dance3");
		}else if (rand == 4) {
			anim.anim.Play ("Dance4");
		}else if (rand == 5) {
			anim.anim.Play ("Dance5");
		}else if (rand == 6) {
			anim.anim.Play ("Dance6");
		}else if (rand == 7) {
			anim.anim.Play ("Dance7");
		}else if (rand == 8) {
			anim.anim.Play ("Dance8");
		}else if (rand == 9) {
			anim.anim.Play ("Dance9");
		}
	}

	//Sleep
	void Sleep(){

		int rand = Random.Range (1, 3);

		if (rand == 1) {
			anim.anim.Play ("Slipping1");
		}else if (rand == 2) {
			anim.anim.Play ("Slipping2");
		}

	}

	//Puke
	void Puke(){

		Instantiate (GameController.game.puke, new Vector3 (transform.position.x, 0, transform.position.z), Quaternion.identity);

		state = 2;
		drunkNumber = 0;
		toHappy = 0;

	}

	//Sets client to drink behaviour
	public void DrinkBehaviour(){

		Seat seat = FindObjectOfType<SpawnClient>().GetRandomSeat ();

		if (seat != null) {
			drink = true;
			agent.SetDestination (seat.transform.position);
			timeWalking = Time.time + 10f;
			anim.ChangeWalkState (true);
			isBusy = true;
			seat.hasSomeone = true;
			seat.client = this;
			actualSeat = seat;
			hasPath = true;
		} else {
			MoveClient ();
		}
	}

	//Sets client to walk behaviour
	public void WalkBehaviour(){

		drink = false;
		MoveClient ();

	}

	//Do something Drink behaviour
	void DoSomethingDrink(){

		float rand = Random.Range (0.0f, 1.0f);

		if (rand < 0.3f) {
			actualSeat.hasSomeone = false;
			actualSeat.client = null;
			WalkBehaviour ();
		} else if (rand < 0.6f) {
			SeatedAnimation ();
		}else {
			SetOrder ();
		}

	}

	//Do something Walk behaviour
	void DoSomethingWalk(){

		float rand = Random.Range (0.0f, 1.0f);

		if (rand < 0.3f) {
			MoveClient ();
		} else if (rand < 0.6f) {
			Dance ();
		} else if (rand < 0.7f && drunkNumber >= 2) {
			Sleep ();
		}else if (rand < 0.8f) {
			if (state == 4) {
				Puke ();
			} else {
				MoveClient ();
			}
		}else {
			if (state != 4) {
				DrinkBehaviour ();
			} else {
				Puke ();
			}
		}

		timeToNextAction = Time.time + timeToAction;

	}

	//Verifies states
	void VerifyState(){

		if (drunkNumber > probPuke) {
			state = 4;

			if (drink) {
				WalkBehaviour ();
			}
		}

		if (toHappy > 1 && state == 2) {
			state = 3;
		}

		if (state == 1 && !isLeaving) {
			LeaveBar ();
		}

	}

	//Change seated animation
	void SeatedAnimation(){

		int rand = Random.Range (1, 3);

		if (rand == 1) {
			anim.anim.Play ("Seated2");
		}else if (rand == 2) {
			anim.anim.Play ("Seated3");
		}

	}

	//Checks time
	void CheckTime(){

		if (timeToBad < Time.time && isOrdering) {
			state--;
			timeToBad = Time.time + timeTolerance;
		}

	}

}
