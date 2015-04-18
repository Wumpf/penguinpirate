using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	private const float JUMP_DURATION = 2.0f;
	private const float JUMP_TAP_DURATION = 0.15f;
	private const float JUMP_HEIGHT = 5.0f;
	private const float JUMP_MAX_DISTANCE = 5.0f;

	// If the player position is below, this height, it will be reset to its start position.
	private const float SUNK_HEIGHT = -4.0f;

	private float lastTapTime = -999999.0f;

	private Vector3 startPosition;
	private IceFloe startFloe;
	

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

		startPosition = transform.position;
		if (transform.parent != null && transform.parent.GetComponent<IceFloe>() != null)
		{
			startFloe = transform.parent.GetComponent<IceFloe>();
		}
		else
			Debug.LogError("Please attach the player to a starting IceFloe!");
	}

	public void Reset()
	{
		transform.position = startPosition;
		transform.parent = startFloe.transform;
	}

	void JumpTap()
	{
		// If penguin is on top of the floe
		if (TouchInput.lastTapReleaseTime - TouchInput.lastTapStartTime < JUMP_TAP_DURATION && 
			transform.parent != null && transform.parent.GetComponent<IceFloe>())
		{
			Vector3 destination = TouchInput.lastTapReleasePosition.groundPosition;
			if (Vector3.Distance(destination, transform.position) < JUMP_MAX_DISTANCE)
				StartCoroutine("Jump", TouchInput.lastTapReleasePosition.groundPosition);
			else
				Debug.Log("Too far away.");
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
			transform.forward = Vector3.Slerp(-LastJumpDirectionWorld, transform.forward, Mathf.Exp(-Time.time * 0.1f));
			jumpTimer += Time.fixedDeltaTime / JUMP_DURATION;

			if (transform.position.y < SUNK_HEIGHT)
			{
				Reset();

				// Reset all floes
				var allIceFloes = GameObject.FindObjectsOfType<IceFloe>();
				foreach(var iceFloe in allIceFloes)
					iceFloe.Reset();

				Debug.Log("Reset position");
				yield return null;
			}

			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
