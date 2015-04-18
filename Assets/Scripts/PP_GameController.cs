using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PP_GameController : MonoBehaviour {
	
	public int currentLevel =1;
	//public int score=0;
	public GameObject bannerObj;
	//public GameObject scoreCount;
	public GameObject levelCount;
	private bool bannerAnimationPlayed=false;


	public void updateGameLevels(){
		// increment game level when player has reached ISLAND

		// when level has been incremented, switch to new level enable
		currentLevel++;
		levelCount.GetComponent<Text>().text = currentLevel.ToString();
	}

	public void gameCompletedStatus(){
		// check if the game has completed and what are the scores to be shown on HUD
		animateTheBannerMessage ("You Win!");
	}

	void animateTheBannerMessage(string msg){
		if (bannerAnimationPlayed == true)
			return;

		bannerObj.SetActive (true);
		bannerObj.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		GameObject textMsg = bannerObj.transform.Find("Message").gameObject;
		textMsg.GetComponent<Text>().text = msg;
		//scale up the banner with sfx
		iTween.ScaleTo (bannerObj,iTween.Hash("scale",new Vector3(1.0f,1.0f,1.0f),"time",0.75f,
		                                      "oncompletetarget",gameObject,"oncomplete",
		                                      "restartGameAfterSomePause","easetype",iTween.EaseType.easeInOutSine));
		}

	void restartGameAfterSomePause(){
		bannerAnimationPlayed = true;
		CancelInvoke("resetTheGameState");
		Invoke("resetTheGameState",0.5f);
		}

	public void resetTheGameState(){
		//disable the banner
		if(bannerObj != null && bannerObj.activeSelf == true) {
			bannerObj.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			bannerObj.SetActive (false);
			}
		// Reset player
		var players = GameObject.FindObjectsOfType<Player>();
		foreach (var player in players)
			player.Reset();
		
		// Reset all floes
		var allIceFloes = GameObject.FindObjectsOfType<IceFloe>();
		foreach (var iceFloe in allIceFloes)
			iceFloe.Reset(); 

		bannerAnimationPlayed = false;
		}

	public void gameEndStatus(){ 	
		animateTheBannerMessage("You've lost it :(");

	}

//	public void updatePlayerScore(){ //no score required for this game
//		score = score+100;
//		scoreCount.GetComponent<Text>().text = score.ToString();
//	}

}
