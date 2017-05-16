using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnClient : MonoBehaviour {

	//Spawn variables
	public float timeToSpawn;
	public float chanceToSpawn;
	public int maxNumber;

	private float timeToNext;
	[HideInInspector]
	public int actualNumber;

	public Seat[] seats;

	int id;

	//Start Method
	void Start(){

		timeToNext = Time.time + 2;

	}

	//Update Method
	void Update(){

		if (GameController.game.isPlaying && !GameController.game.isPaused) {
			float rand = Random.Range (0.0f, 1.0f);

			if (timeToNext < Time.time && rand < chanceToSpawn && actualNumber < maxNumber) {
				InstantiateClient ();
				actualNumber++;
				timeToNext = Time.time + timeToSpawn;
			}
		}

	}

	//Instantiates client
	public void InstantiateClient(){

		//Instantiates client
		int rand2 = Random.Range (0, 3);

		GameObject cl = (GameObject) Instantiate (GameController.game.client[rand2], transform.position, Quaternion.identity);
		Client client = cl.GetComponent<Client> ();
		client.id = id;
		id++;

		float rand = Random.Range (0f, 100f);
		if (rand < 50 && SeatsAvaiable() > 0) {
			client.DrinkBehaviour ();
		} else {
			client.WalkBehaviour ();
		}

		GameController.game.clients.Add (client);

	}

	//Get avaiable seats
	int SeatsAvaiable(){

		int seatNumber = 0;

		Seat[] seats = FindObjectsOfType<Seat> ();

		foreach (var item in seats) {
			if (!item.hasSomeone) {
				seatNumber++;
			}
		}

		return seatNumber;

	}

	//Returns random seat from avaiable seats list
	public Seat GetRandomSeat(){

		//Seat[] seats = FindObjectsOfType<Seat> ();
		List<Seat> avaiableSeats = new List<Seat> ();

		foreach (var item in seats) {
			if (!item.hasSomeone) {
				avaiableSeats.Add (item);
			}
		}

		if (avaiableSeats.Count == 0) {
			Debug.Log ("vazio");
			return null;
		}

		int rand = Random.Range (0, avaiableSeats.Count - 1);

		return avaiableSeats [rand];

	}

	//Increase client number
	public void IncreaseClient(){
	
		maxNumber += 10;

	}

}
