using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PP_GameController : MonoBehaviour {
	
	public int currentLevel =1;
	public GameObject bannerObj;
	public GameObject levelCount;
	public GameObject allLevels;
	private bool bannerAnimationPlayed=false;
	private bool didUpdateLevel=false;

	public AudioClip jumpSfx,celebSfx,waterSplash,crying;
	public AudioSource aSource;

	void Start(){
		PlayerPrefs.SetInt ("CurrentLevel",currentLevel);
	}


	public void playSoundEffect(string _name){

		if(_name=="Jump")
			aSource.clip = jumpSfx;
		else if(_name=="Celeb")
			aSource.clip = celebSfx;
//		else if(_name=="WaterSplash")
//			aSource.clip = waterSplash;
		else if(_name=="Lose")
			aSource.clip = crying;

		aSource.Play();
	}

	public void updateGameLevels(){
		// increment game level when player has reached ISLAND
			currentLevel = 2;//currentLevel++; HARD CODED
			PlayerPrefs.SetInt ("CurrentLevel",currentLevel);
			levelCount.GetComponent<Text> ().text = currentLevel.ToString ();
			// when level has been incremented, switch to new level enable
			playSoundEffect("Celeb");
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

		// reset the game state here
		resetTheGameState ();

		GameObject currentLevelObj = allLevels.transform.FindChild(currentLevel.ToString()).gameObject;
		if(currentLevelObj!=null)
			currentLevelObj.SetActive (true);

		GameObject prevLevelObj = allLevels.transform.FindChild((currentLevel-1).ToString()).gameObject;
		if(currentLevelObj!=null)
			prevLevelObj.SetActive (false);

		//animate camera to the new Level Position, pan to right position only
		/*iTween.MoveTo (Camera.main.gameObject,new Vector3(Camera.main.transform.position.x + 100f,Camera.main.transform.position.y,
		                                                  Camera.main.transform.position.z),1.0f);
        */
		bannerAnimationPlayed = false;
	}


	void restartGameAfterSomePause(){
		bannerAnimationPlayed = true;
		CancelInvoke("resetTheGameState");
		Invoke("resetTheGameState",0.5f);
		}

	public void resetTheGameState(){
		currentLevel = PlayerPrefs.GetInt ("CurrentLevel");

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
		playSoundEffect("Lose");
		animateTheBannerMessage("You've lost it!",false);
	}

//	public void updatePlayerScore(){ //no score required for this game
//		score = score+100;
//		scoreCount.GetComponent<Text>().text = score.ToString();
//	}

}
