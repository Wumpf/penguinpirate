using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	private const float JUMP_DURATION = 2.0f;
	private const float JUMP_TAP_DURATION = 0.15f;
	private const float JUMP_HEIGHT = 5.0f;

	private float lastTapTime = -999999.0f;

	/// <summary>
	/// Plane used for picking (used for getting a jump destination)
	/// </summary>
	private Plane groundplane = new Plane(Vector3.up, 0.0f);

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			lastTapTime = Time.time;
		
		// If penguin is on top of the floe
		if (Input.GetMouseButtonUp(0) && transform.parent != null && transform.parent.GetComponent<IceFloe>())
		{
			if (Time.time - lastTapTime < JUMP_TAP_DURATION)
			{
				Vector2 screenPosition = Input.mousePosition; // TODO make compatible to tap!

				Ray pickingRay = Camera.main.ScreenPointToRay(screenPosition);
				float rayDist;
				if (groundplane.Raycast(pickingRay, out rayDist)) // should never be false.
				{
					StartCoroutine("Jump", pickingRay.GetPoint(rayDist));
				}
			}
		}	
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


		while (transform.parent == null) // Not yet reattached to an icefloe
		{
			transform.position = bezierPath.CalculateBezierPoint(0, jumpTimer);

			jumpTimer += Time.fixedDeltaTime / JUMP_DURATION;
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
