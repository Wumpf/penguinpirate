using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour
{
	private const float SINK_MOVEMENT = 0.05f;
	private const float SUNK_HEIGHT = -1.56f;

	private const float JUMP_FORCE_FACTOR = 1.0f;

	private Vector3 startPosition;
	private Quaternion startOrientation;

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
			transform.Translate(0, -SINK_MOVEMENT * Time.deltaTime, 0);

			if (transform.position.y < SUNK_HEIGHT)
			{
				Debug.Log("icefloe with player has sunk. You lost.");
				GameObject.FindObjectOfType<PP_GameController>().gameEndStatus();
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
