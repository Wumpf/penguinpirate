﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	private const float JUMP_DURATION = 2.0f;
	private const float JUMP_TAP_DURATION = 0.15f;
	private const float JUMP_HEIGHT = 5.0f;

	private float lastTapTime = -999999.0f;

	public TouchInput TouchInput;

	/// <summary>
	/// Plane used for picking (used for getting a jump destination)
	/// </summary>
	private Plane groundplane = new Plane(Vector3.up, 0.0f);

	public Vector3 LastJumpDirectionWorld
	{
		get;
		private set;
	}

	// Use this for initialization
	void Start()
	{
		TouchInput.onTapRelease += JumpTap;
	}

	void JumpTap()
	{
		// If penguin is on top of the floe
		if (TouchInput.lastTapReleaseTime - TouchInput.lastTapStartTime < JUMP_TAP_DURATION && 
			transform.parent != null && transform.parent.GetComponent<IceFloe>())
		{
			StartCoroutine("Jump", TouchInput.lastTapReleasePosition.groundPosition);
		}	
	}

	// Update is called once per frame
	void Update()
	{
	}

	IEnumerator Jump(Vector3 destination)
	{
		transform.parent = null; // Detach from IceFloe

		float jumpTimer = 0.0f;

		// Compute jump spline
		List<Vector3> points = new List<Vector3>();
		points.Add(this.transform.position);
		points.Add(new Vector3(this.transform.position.x, this.transform.position.y + JUMP_HEIGHT, this.transform.position.z));
		points.Add(new Vector3(destination.x, this.transform.position.y + JUMP_HEIGHT, this.transform.position.z));
		points.Add(destination);

		BezierPath bezierPath = new BezierPath();
		bezierPath.SetControlPoints(points);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints0();

		LastJumpDirectionWorld = destination - transform.position;
		LastJumpDirectionWorld.Normalize();

		while (transform.parent == null) // Not yet reattached to an icefloe
		{
			transform.position = bezierPath.CalculateBezierPoint(0, jumpTimer);
			transform.forward = Vector3.Slerp(new Vector3(LastJumpDirectionWorld.y, LastJumpDirectionWorld.y, LastJumpDirectionWorld.x), transform.forward, Mathf.Exp(-Time.time * 0.1f));

			jumpTimer += Time.fixedDeltaTime / JUMP_DURATION;
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
