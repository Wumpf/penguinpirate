﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour
{
	public float SINKING_SPEED = 0.05f;

	private const float SINK_MOVEMENT = 0.1f;

	private const float JUMP_FORCE_FACTOR = 1.0f;

	private Vector3 startPosition;
	private Quaternion startOrientation;

	/// <summary>
	/// If below zero, the shelf is gone and the player on it has lost.
	/// </summary>
	public float SinkPercentage
	{
		get { return sinkPercentage; }
	}
	private float sinkPercentage = 1.0f;

	// Use this for initialization
	void OnEnable()
	{
		startPosition = transform.position;
		startOrientation = transform.rotation;
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
			sinkPercentage -= Time.deltaTime * SINKING_SPEED;
			transform.Translate(0, -SINK_MOVEMENT * Time.deltaTime, 0);

			if (sinkPercentage < 0.0f)
			{
				// TODO
			}
		}

		// TODO add some buoyancy animation :)
	}


	void OnCollisionEnter(Collision col)
	{
		Player player = col.gameObject.GetComponent<Player>();
		if (player != null && col.gameObject != transform.parent)
		{
			Quaternion rotation = col.gameObject.transform.rotation;
			col.gameObject.transform.parent = transform;
			col.gameObject.transform.rotation = rotation;
			this.GetComponent<Rigidbody>().AddForce(player.LastJumpDirectionWorld * JUMP_FORCE_FACTOR);
		}
	}
}
