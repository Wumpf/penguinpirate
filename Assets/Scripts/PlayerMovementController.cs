using UnityEngine;
using System.Collections;

class PlayerMovementController : MonoBehaviour{
    public static float SECONDS_PER_SPLINE = 10;
    public static float MIN_DIRECTION_SIZE_PIXELS = 10;

    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

    private Helpers.HermiteSpline currentMovement;
    private Helpers.HermiteSpline hintMovement;

    public GameObject dot; 
    private GameObject[] hintSplineDots;

    private bool touching;
    private Vector2 touchStart;

    // Time ranging from 0 to 1. 
    private float relativeTime;

	// Use this for initialization
    public void Start()
    {
        touching = false;

        hintSplineDots = new GameObject[10];
        for(int i = 0; i < hintSplineDots.Length; ++i)
            hintSplineDots[i] = Instantiate(dot, Vector3.zero, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
        relativeTime += Time.deltaTime / SECONDS_PER_SPLINE; // One spline will be followed for 3 secs.
        if(relativeTime > 1)
        {
            Position += (Vector3)currentMovement.FinalMovementDirection / SECONDS_PER_SPLINE;
            return;
        }

        Position = currentMovement.EvaluateAt(relativeTime);

        if(!touching && Input.touchCount > 0)
        {
            touchStart = Input.touches[0].position;
        }

        if(touching)
        {
            // Released
            if(Input.touchCount < 1)
            {
                currentMovement = hintMovement;
            }

            Vector2 touchDirection = Input.touches[0].position - touchStart;
            if (touchDirection.magnitude < MIN_DIRECTION_SIZE_PIXELS)
                ChangeHint(touchStart);
            else
                ChangeHint(touchStart, touchDirection);
        }
	}

    void FixedUpdate()
    {
        Rigidbody iceFloe = transform.parent.GetComponent<Rigidbody>();
        if(iceFloe == null)
        {
            return;
        }

        float currentT;
        currentMovement.GetNearestPosition((Vector2)Position, out currentT);
        iceFloe.AddForce(currentMovement.DirectionAt(currentT));
    }

    public void ChangeHint(Vector2 start, Vector2 direction)
    {
        hintMovement = new Helpers.HermiteSpline((Vector2)Position, currentMovement.EvaluateAt(relativeTime), start, direction);
        //int numDots = this.
    }

    public void ChangeHint(Vector2 start)
    {
        hintMovement = new Helpers.HermiteSpline((Vector2)Position, currentMovement.EvaluateAt(relativeTime), start);
        //int numDots = this.

       
    }

    public void ResetHint()
    {
        hintMovement = null; // new Helpers.HermiteSpline((Vector2)transform.position, currentMovement.EvaluateAt(relativeTime), start);
        //int numDots = this.
    }

    public void ShowHint()
    {
        if (hintMovement == null)
            return;
    }
}
