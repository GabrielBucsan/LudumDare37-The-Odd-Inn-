using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlassMat : MonoBehaviour {

	//Glassmat variables
	public bool hasGlass;
	public bool isSide;
	public Transform glassSpot;

	//Start Method
	void Start(){

		hasGlass = false;

	}

	//Gives glass to client
	public void GiveGlass(int _beer){
	
		if (!hasGlass) {
			hasGlass = true;
			InstantiateGlass (glassSpot, _beer);
		}

	}

	//Takes glass from client
	public void TakeGlass(){

		if (hasGlass) {
			hasGlass = false;
			DestroyChildren ();
		}

	}

	//Instantiates glass
	void InstantiateGlass(Transform _transform, int _beer){
	
		Vector3 position = new Vector3 (_transform.position.x, _transform.position.y + 0.295f, _transform.position.z);

		GameObject glass = (GameObject) Instantiate (GameController.game.glass, position, Quaternion.Euler(new Vector3(-90, -90, 0)));
		glass.transform.parent = glassSpot;
		glass.GetComponent<Glass> ().actual = _beer;
		if (_beer > 0) {
			glass.GetComponent<Glass>().liquid.GetComponent<Renderer> ().material.color = GameController.game.caskColors [_beer - 1];
		}
	}

	//Destroys all children
	void DestroyChildren(){

		var children = new List<GameObject>();
		foreach (Transform child in glassSpot) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

	}

	//Serve client
	public void ServeClient(int _beer){

		if (isSide) {
			Seat seat = transform.FindChild ("Seat").GetComponent<Seat>();

			seat.client.GetOrder (_beer);
		}

	}

}
