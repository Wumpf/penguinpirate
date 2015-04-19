using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour {

	public GameObject howToOverlay;
	public GameObject aboutOverlay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowHowToOverlay() {
		howToOverlay.SetActive(true);
	}

	public void HideHowToOverlay() {
		howToOverlay.SetActive(false);
	}

	public void ShowAboutOverlay() {
		aboutOverlay.SetActive(true);
	}

	public void HideAboutOverlay() {
		aboutOverlay.SetActive(false);
	}
}
