using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public const float JUMP_DURATION = 2.0f;
    public const float JUMP_TAP_DURATION = 0.15f;
    public const float JUMP_HEIGHT = 5.0f;
    public const float JUMP_MAX_DISTANCE = 5.0f;

	private const float JUMP_FORCE_FACTOR = 1.0f;

	// If the player position is below, this height, it will be reset to its start position.
	private float SUNK_HEIGHT;

	private float lastTapTime = -999999.0f;

	private Vector3 startPosition;
	

	public TouchInput TouchInput;
	private PP_GameController gameController;

	/// <summary>
	/// Plane used for picking (used for getting a jump destination)
	/// </summary>
	private Plane groundplane = new Plane(Vector3.up, 0.0f);

	public Vector3 LastJumpDirectionWorld
	{
		get;
		private set;
	}

	/// <summary>
	/// Current jump status. 0 just started, 1 done.
	/// </summary>
	public float JumpTimer
	{
		get;
		private set;
	}

	// Use this for initialization
	void Start()
	{
		startPosition = transform.position;

		TouchInput.onTapRelease += JumpTap;

		gameController = GameObject.FindObjectOfType<PP_GameController>();
		if(gameController == null)
		{
			Debug.LogError("Please add a game controller to the scene!");
		}

        SUNK_HEIGHT = GetComponent<Collider>().bounds.extents.y;
	}

	public void Reset()
	{
        transform.position = Vector3.up * 0.84F;

        GameObject startFloe = GameObject.FindGameObjectWithTag("StartFloe");
        transform.parent = startFloe.transform;

        GetComponent<PlayerMovementController>().currentFloe = startFloe.GetComponent<IceFloe>();
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
        GetComponent<PlayerMovementController>().currentFloe = null;

		JumpTimer = 0.0f;

		// Compute jump spline
		List<Vector3> points = new List<Vector3>();
		points.Add(this.transform.position);
		points.Add(new Vector3(this.transform.position.x, this.transform.position.y + JUMP_HEIGHT, this.transform.position.z));
		points.Add(new Vector3(destination.x, this.transform.position.y + JUMP_HEIGHT, this.transform.position.z));
		points.Add(destination);

		BezierPath bezierPath = new BezierPath();
		bezierPath.SetControlPoints(points);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints0();

		destination.y = transform.position.y;
		LastJumpDirectionWorld = destination - transform.position;
		LastJumpDirectionWorld.Normalize();

		while (transform.parent == null) // Not yet reattached to an icefloe
		{
			transform.position = bezierPath.CalculateBezierPoint(0, JumpTimer);
			transform.forward = Vector3.Slerp(-LastJumpDirectionWorld, transform.forward, Mathf.Exp(-Time.time * 0.1f));
			JumpTimer += Time.fixedDeltaTime / JUMP_DURATION;

			if (transform.position.y < SUNK_HEIGHT)
			{
				Debug.Log("Player has jumped into the water. You lost.");
				gameController.gameEndStatus();
				yield return null;
			}

			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}

	void OnCollisionEnter(Collision col)
	{
		IceFloe iceFloe = col.gameObject.GetComponent<IceFloe>();
		if (iceFloe != null && iceFloe != transform.parent && JumpTimer > 0.5f)
		{
			Quaternion rotation = transform.rotation;
			transform.parent = iceFloe.transform;
			transform.rotation = rotation;
			iceFloe.GetComponent<Rigidbody>().AddForce(LastJumpDirectionWorld * JUMP_FORCE_FACTOR);

            GetComponent<PlayerMovementController>().currentFloe = iceFloe;
		}

		else if(col.gameObject.tag == "Goal")
		{
			StopCoroutine("Jump");
			gameController.updateGameLevels();
			return;
		}
	}
}
