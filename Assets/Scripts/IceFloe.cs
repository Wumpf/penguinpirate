using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour
{
	private const float SINKING_SPEED = 0.1f;

	/// <summary>
	/// Factor to get Y position from sink-percentage.
	/// </summary>
	private const float SINK_TO_Y = 1.0f;

	/// <summary>
	/// If below zero, the shelf is gone and the player on it has lost.
	/// </summary>
	public float SinkPercentage
	{
		get { return sinkPercentage; }
	}
	private float sinkPercentage = 1.0f;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		// Shelves are sinking if they have a player transform child
		if (transform.GetComponentsInChildren(typeof(Transform)).Any(x => x.tag == "Player"))
		{
			sinkPercentage -= Time.deltaTime * SINKING_SPEED;
			transform.position = new Vector3(transform.position.x, sinkPercentage * SINK_TO_Y, transform.position.z);

			if (sinkPercentage < 0.0f)
			{
				// TODO
			}
		}

		// TODO add some buoyancy animation :)
	}
}
