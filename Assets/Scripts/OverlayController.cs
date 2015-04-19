using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour {

	public Image overlay;
	public Image swipe;
	public Image tap;
	public Button closeBtn;
	public Image closeImg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showOverlay() {
		overlay.enabled = true;
		swipe.enabled = true;
		tap.enabled = true;
		closeBtn.enabled = true;
		closeImg.enabled = true;
	}

	public void hideOverlay() {
		overlay.enabled = false;
		swipe.enabled = false;
		tap.enabled = false;
		closeBtn.enabled = false;
		closeImg.enabled = false;
	}

}
