using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PP_GameController : MonoBehaviour {


	public int currentLevel =1;
	public int score=0;
	public GameObject bannerObj;

	// Update is called once per frame
	void Update () {
	
	}

	// user methods for game status
	public void updateGameLevels(){
		// increment game level when player has reached ISLAND

		// when level has been incremented, switch to new level enable
	}


	public void gameCompletedStatus(){
		// check if the game has completed and what are the scores to be shown on HUD
	}

	void animateTheBannerMessage(string msg){
		bannerObj.SetActive (true);
		//bannerObj.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		GameObject textMsg = bannerObj.transform.Find("Message").gameObject;
		textMsg.GetComponent<Text>().text = msg;
	}

	void resetTheGameState(){

		//disable the banner
		bannerObj.SetActive (false);

		// Reset player
		var players = GameObject.FindObjectsOfType<Player>();
		foreach (var player in players)
			player.Reset();
		
		// Reset all floes
		var allIceFloes = GameObject.FindObjectsOfType<IceFloe>();
		foreach (var iceFloe in allIceFloes)
			iceFloe.Reset();
	}

	public void gameEndStatus()
	{
		// TODO messages ... 
		animateTheBannerMessage ("You've lost it!");
		Invoke ("resetTheGameState", 1.0f);
	}

	public void updatePlayerScore(){

	}


}
