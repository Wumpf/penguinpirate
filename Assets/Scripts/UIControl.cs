using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	public float iceFloeTTL = 100;
	private Text textField;

	// Use this for initialization
	void Start () {
		textField = GameObject.Find("SCount").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		textField.text = ((int)iceFloeTTL).ToString() + " %";
	}

	public void switchToMenu() {
		Application.LoadLevel("Menu");
	}

	public void retryLevel() {
		//Application.LoadLevel("PenguinPirate");
		PP_GameController gameCtrl = Camera.main.GetComponent<PP_GameController>();
		gameCtrl.resetTheGameState();
	}
}
