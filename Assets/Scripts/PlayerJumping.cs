using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerJumping : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void jumpPath(Vector3 destination,float jumpHeight,float jumpHeight_2,float time,string methodName,iTween.EaseType easeType){

		List<Vector3> points = new List<Vector3>();
		points.Add(this.transform.parent.position);
		points.Add(new Vector3(this.transform.parent.position.x, this.transform.parent.position.y+jumpHeight, this.transform.parent.position.z));
		points.Add(new Vector3(destination.x, this.transform.parent.position.y+jumpHeight_2, this.transform.parent.position.z));
		points.Add(new Vector3(destination.x, destination.y, this.transform.parent.position.z));
		
		BezierPath bezierPath = new BezierPath();
		bezierPath.SetControlPoints(points);
		List<Vector3> drawingPoints = bezierPath.GetDrawingPoints0();
		
		Vector3[] path = new Vector3[drawingPoints.Count];
		for(int i = 0; i< drawingPoints.Count; i++) {
			path[i] = drawingPoints[i];
		}
		iTween.MoveTo(this.transform.parent.gameObject, 
		              iTween.Hash("path", path, "time", time, "onComplete",methodName,"oncompletetarget",
		            gameObject,  "easetype", easeType));
	}
}
