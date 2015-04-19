using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PlayerStates{
	idle=0,
	jumping=1,
	falling=2,
	sinking=3,
	died
};

public class PP_GameController : MonoBehaviour {
	
	public int currentLevel =1;
	public GameObject bannerObj;
	public GameObject levelCount;
	public GameObject allLevels;
	private bool bannerAnimationPlayed=false;
	private bool didUpdateLevel=false;

	public void updateGameLevels(){
		// increment game level when player has reached ISLAND
			currentLevel++;
			levelCount.GetComponent<Text> ().text = currentLevel.ToString ();
			// when level has been incremented, switch to new level enable
			animateTheBannerMessage ("You've done it!", true);
	}

	void animateTheBannerMessage(string msg,bool isLevelCompleted){
		string methodName;
		if (bannerAnimationPlayed == true)
			return;

		if (!isLevelCompleted) {
			methodName="restartGameAfterSomePause";
		}
		else 
			methodName="switchToNextLevelAfterSomePause";

		bannerObj.SetActive (true);
		bannerObj.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
		GameObject textMsg = bannerObj.transform.Find("Message").gameObject;
		textMsg.GetComponent<Text>().text = msg;
		//scale up the banner with sfx
		iTween.ScaleTo (bannerObj,iTween.Hash("scale",new Vector3(1.0f,1.0f,1.0f),"time",0.75f,
		                                      "oncompletetarget",gameObject,"oncomplete",
		                                      methodName,"easetype",iTween.EaseType.easeInOutSine));
		}

	void switchToNextLevelAfterSomePause(){
		bannerAnimationPlayed = true;
		CancelInvoke("panCameraToNextLevel");
		Invoke("panCameraToNextLevel",0.5f);
	}
	
	void panCameraToNextLevel(){
		//disable the banner
		if(bannerObj != null && bannerObj.activeSelf == true) {
			bannerObj.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);
			bannerObj.SetActive (false);
		}

		GameObject currentLevelObj = allLevels.transform.FindChild(currentLevel.ToString()).gameObject;
		currentLevelObj.SetActive (true);

		GameObject prevLevelObj = allLevels.transform.FindChild((currentLevel-1).ToString()).gameObject;
		prevLevelObj.SetActive (false);

		//animate camera to the new Level Position, pan to right position only
		iTween.MoveTo (Camera.main.gameObject,new Vector3(Camera.main.transform.position.x + 100f,Camera.main.transform.position.y,
		                                                  Camera.main.transform.position.z),1.0f);

		bannerAnimationPlayed = false;
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

		// Reset all floes
		var allIceFloes = GameObject.FindObjectsOfType<IceFloe>();
		foreach (var iceFloe in allIceFloes)
			iceFloe.Reset(); 

		// Reset player
		var players = GameObject.FindObjectsOfType<Player>();
		foreach (var player in players)
			player.Reset();

        CameraController cam = GetComponent<CameraController>();
        if(cam != null)
        {
            cam.Player = players[0].transform;
        }

		bannerAnimationPlayed = false;
		didUpdateLevel =false;
		}

	public void gameEndStatus(){ 	
		animateTheBannerMessage("You've lost it!",false);
	}

//	public void updatePlayerScore(){ //no score required for this game
//		score = score+100;
//		scoreCount.GetComponent<Text>().text = score.ToString();
//	}

}
