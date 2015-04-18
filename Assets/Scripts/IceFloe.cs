﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour
{
	private const float SINK_MOVEMENT = 0.05f;
	private const float SUNK_HEIGHT = -1.7f;


	private Vector3 startPosition;
	private Quaternion startOrientation;

	private UIControl UIScript;

	// Use this for initialization
	void OnEnable()
	{
		startPosition = transform.position;
		startOrientation = transform.rotation;

		UIScript = GameObject.Find("UI_Control").GetComponent<UIControl>();
	}

	/// <summary>
	/// Resets position and orientation to original
	/// </summary>
	public void Reset()
	{
		transform.position = startPosition;
		transform.rotation = startOrientation;
	}

	// Update is called once per frame
	void Update()
	{
		// Shelves are sinking if they have a player transform child
		if (transform.GetComponentsInChildren(typeof(Transform)).Any(x => x.tag == "Player"))
		{
			transform.Translate(0, -SINK_MOVEMENT * Time.deltaTime, 0);

			UIScript.iceFloeTTL = ((SUNK_HEIGHT - transform.position.y) / (SUNK_HEIGHT - startPosition.y)) * 100;


			if (transform.position.y < SUNK_HEIGHT)
			{
				Debug.Log("icefloe with player has sunk. You lost.");
				GameObject.FindObjectOfType<PP_GameController>().gameEndStatus();
			}
		}

		// TODO add some buoyancy animation :)
	}
}
