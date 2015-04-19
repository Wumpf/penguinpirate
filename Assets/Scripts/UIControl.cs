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
		float iceFloePrcnt = iceFloeTTL/100;
		Image iceFloeImage = iceFloeTTLImage.GetComponent<Image>();

		iceFloeImage.color = Color.Lerp(new Color(0.59f, 0.13f, 0.07f), new Color(0.27f, 0.38f, 0.48f), iceFloePrcnt);
		iceFloeImage.fillAmount = iceFloePrcnt;
	}

	public void switchToMenu() {
		Application.LoadLevel("Menu");
	}

	public void retryLevel() {
		PP_GameController gameCtrl = Camera.main.GetComponent<PP_GameController>();
		gameCtrl.resetTheGameState();
	}
}
