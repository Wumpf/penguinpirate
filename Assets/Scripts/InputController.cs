using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	void OnEnable()
	{
		FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;	
	}
	
	
	void OnDisable()
	{
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
	}

	
	void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
	{
		Debug.Log ("On tap pos:"+ fingerPos.x + "_" + fingerPos.y);

		GameObject tappedObject = PickObject (fingerPos);
		Vector3 worldPos = GetWorldPos (fingerPos);

		if (tappedObject != null)
			Debug.Log ("Tapped Object is:" + tappedObject.name);
	}


	#region Utils
	
	// Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
	 Vector3 GetWorldPos( Vector2 screenPos )
	{
		Ray ray = Camera.main.ScreenPointToRay( screenPos );
		
		// we solve for intersection with z = 0 plane
		float t = -ray.origin.z / ray.direction.z;
		
		return ray.GetPoint( t );
	}
	
	// Return the GameObject at the given screen position, or null if no valid object was found
	 GameObject PickObject( Vector2 screenPos )
	{
		Ray ray = Camera.main.ScreenPointToRay( screenPos );
		RaycastHit hit;
		
		if( Physics.Raycast( ray, out hit ) )
			return hit.collider.gameObject;
		
		return null;
	}
	
	#endregion
}
