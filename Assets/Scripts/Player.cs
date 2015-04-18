using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private const float JUMP_DURATION = 2.0f;

	/// <summary>
	/// Plane used for picking (used for getting a jump destination)
	/// </summary>
	private Plane groundplane = new Plane(Vector3.up, 0.0f);

	// Use this for initialization
	void Start()
	{
	}

	void OnEnable()
	{
		FingerGestures.OnFingerTap += JumpTap;
	}
	void OnDisable()
	{
		FingerGestures.OnFingerTap -= JumpTap;
	}

	void JumpTap(int fingerIndex, Vector2 fingerPos, int tapCount)
	{
		if (tapCount == 0 && fingerIndex == 0)
		{
			Ray pickingRay = Camera.main.ScreenPointToRay(fingerPos);
			float rayDist;
			if (groundplane.Raycast(pickingRay, out rayDist)) // should never be false
			{
				pickingRay.GetPoint(rayDist);
				StartCoroutine("Jump", rayDist);
			}
		}
	}
	

	// Update is called once per frame
	void Update()
	{
	}

	IEnumerator Jump(Vector3 destination)
	{
		transform.parent = null; // Detach from IceFloe
		FingerGestures.OnFingerTap -= JumpTap; // No more jumping

		float jumpTimer = 0.0f;
		Vector3 startPosition = transform.position;

		while (transform.parent == null) // Not yet reattached to an icefloe
		{
			// todo

			jumpTimer += Time.fixedDeltaTime / JUMP_DURATION;
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
