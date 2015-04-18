using UnityEngine;
using System.Collections;

public class PP_GameController : MonoBehaviour {


	public int currentLevel =1;
	public int score=0;
	
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

	public void gameEndStatus()
	{
		// TODO messages ... 


		// Reset player
		var players = GameObject.FindObjectsOfType<Player>();
		foreach (var player in players)
			player.Reset();

		// Reset all floes
		var allIceFloes = GameObject.FindObjectsOfType<IceFloe>();
		foreach (var iceFloe in allIceFloes)
			iceFloe.Reset();
	}

	void updatePlayerScore(){

	}


}
