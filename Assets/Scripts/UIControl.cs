using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	public float iceFloeTTL = 100;
	public Image iceFloeTTLImage;

	private Text textField;
	 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		iceFloeTTLImage.GetComponent<Image>().fillAmount = iceFloeTTL/100;
	}

	public void switchToMenu() {
		Application.LoadLevel("Menu");
	}

	public void retryLevel() {
		PP_GameController gameCtrl = Camera.main.GetComponent<PP_GameController>();
		gameCtrl.resetTheGameState();
	}
}
