using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//Game variables
	public int score;
	public int lives = 10;

	public int maxClients;

	public bool isPlaying;
	public bool isPaused;
	public bool isSound;

	//Singleton reference
	public static GameController game;

	//Glass reference
	public GameObject glass;

	//Client variables
	public GameObject[] client;

	public List<Client> clients = new List<Client> ();

	//Mug sprites
	public Texture mugEmpty;
	public Texture mug1;
	public Texture mug2;
	public Texture mug3;

	//Cask variables
	public Color[] caskColors;

	//Environment variables
	public Transform door;

	//Happyness sprites
	public Texture happy;
	public Texture normal;
	public Texture mad;
	public Texture sick;

	//Puke variables
	public GameObject puke;

	//HUD variable
	public HUDManager hud;
	public GameObject lostPanel;
	public Text scoreTxt;

	public Sprite play;
	public Sprite pause;
	public Button playBtn;

	public Sprite playSound;
	public Sprite pauseSound;
	public Button soundBtn;

	//Time variables
	public float secondsToHard;
	float timeToHard;

	//Start Method
	void Start(){

		game = this;
		timeToHard = Time.time + secondsToHard;
		isSound = true;

		UpdatePlayBtn ();
		UpdateSoundBtn ();

	}

	//Update Method
	void Update(){

		CheckTime ();

		if (lives <= 0) {
			LostGame ();
		}

	}

	//Increase Score
	public void IncreaseScore(){

		score++;
		hud.ChangeScore ();

	}

	//Decrease Life
	public void DecreaseLife(){

		lives--;
		hud.ChangeLife ();

	}

	//Checks Time
	void CheckTime(){
	
		if (timeToHard < Time.time) {
			timeToHard = Time.time + secondsToHard;
			if (FindObjectOfType<SpawnClient>().maxNumber < maxClients) {
				FindObjectOfType<SpawnClient> ().IncreaseClient ();	
			}
		}
	}

	//Remove client from list
	public void RemoveClient(int _id){

		for (int i = 0; i < clients.Count; i++) {
			if (clients[i].id == _id) {
				clients.Remove(clients[i]);
				break;
			}
		}

	}

	//Change game state
	public void ChangeGameState(bool _state){

		isPlaying = _state;

	}

	//Game Over
	void LostGame(){

		isPlaying = false;
		scoreTxt.text = score.ToString ();
		PauseGame ();
		lostPanel.SetActive (true);

	}

	//Reset game
	public void RestartGame(){

		SceneManager.LoadScene ("Main");

	}

	//Pause game
	public void PauseGame(){

		if (isPaused) {
			isPaused = false;
			Time.timeScale = 1;
		} else {
			isPaused = true;
			Time.timeScale = 0;
		}

		UpdatePlayBtn ();

	}

	//Change Sound
	public void ChangeSoundState(){

		if (isSound) {
			isSound = false;

			AudioSource[] audios = FindObjectsOfType<AudioSource> ();

			foreach (var item in audios) {
				item.volume = 0;
			}

		} else {
			isSound = true;

			AudioSource[] audios = FindObjectsOfType<AudioSource> ();

			foreach (var item in audios) {
				item.volume = 1;
			}

		}

		UpdateSoundBtn ();

	}

	//Changes sprite of play button
	void UpdatePlayBtn(){

		if (isPaused) {
			playBtn.GetComponent<Image>().sprite = play;
		} else {
			playBtn.GetComponent<Image>().sprite = pause;
		}

	}

	//Changes sprite of sound button
	void UpdateSoundBtn(){

		if (isSound) {
			soundBtn.GetComponent<Image>().sprite = playSound;
		} else {
			soundBtn.GetComponent<Image>().sprite = pauseSound;
		}

	}

	//Quit Game
	public void QuitGame(){

		Application.Quit ();

	}
}
