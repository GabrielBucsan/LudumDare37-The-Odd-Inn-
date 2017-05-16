using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	//HUD variables
	public RawImage[] lifes;

	public Text score;

	public Text[] clients;

	//Update Method
	void Update(){

		UpdateClients ();

	}

	//Change life
	public void ChangeLife(){
	
		int life = 10 - GameController.game.lives;

		if (life != 0) {
			for (int i = 0; i < life; i++) {
				lifes [i].gameObject.SetActive(false);
			}
		}

	}

	//Change score
	public void ChangeScore(){

		score.text = GameController.game.score.ToString ();

	}

	//Update client interface
	void UpdateClients(){

		int mad = 0;
		int normal = 0;
		int happy = 0;
		int sick = 0;
		int total = 0;

		if (GameController.game.clients.Count > 0) {
			foreach (var item in GameController.game.clients) {
				if (item.state == 1) {
					mad++;
				}else if (item.state == 2) {
					normal++;
				}else if (item.state == 3) {
					happy++;
				}else if (item.state == 4) {
					sick++;
				}

				total++;
			}
		}

		clients [0].text = total.ToString ();
		clients [1].text = happy.ToString ();
		clients [2].text = normal.ToString ();
		clients [3].text = mad.ToString ();
		clients [4].text = sick.ToString ();

	}

}
