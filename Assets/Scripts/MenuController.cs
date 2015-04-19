using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playGame() {
		PlayerPrefs.SetInt ("CurrentLevel",1);
		Application.LoadLevel("PenguinPirate");
	}

	public void quitGame() {
		Debug.Log("Quit");
		Application.Quit();
	}
}
