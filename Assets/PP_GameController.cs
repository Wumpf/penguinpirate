using UnityEngine;
using System.Collections;

public class PP_GameController : MonoBehaviour {


	public int currentLevel =1;
	public int score=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// user methods for game status


	void updateGameLevels(){
		// increment game level when player has reached ISLAND

		// when level has been incremented, switch to new level enable
	}


	void gameCompletedStatus(){
		// check if the game has completed and what are the scores to be shown on HUD

	}

	void gameEndStatus(){
		// when you lose game, show the message on screen
	}

	void updatePlayerScore(){

	}


}
