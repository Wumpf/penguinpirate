﻿using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playGame() {
		Application.LoadLevel("PenguinPirate");
	}

	public void quitGame() {
		Application.Quit();
	}
}
