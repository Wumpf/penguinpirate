using UnityEngine;
using System.Collections;

class PlayerMovementController : MonoBehaviour
{
    public TouchInput TouchInput;

    public static float SECONDS_PER_SPLINE = 10;
    public static float MIN_DIRECTION_SIZE_PIXELS = 10;

    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

    private Helpers.HermiteSpline currentMovement;

    public GameObject dot;
    private GameObject[] hintSplineDots;

    private bool touching;
    private Vector2 touchStart;

    // Time ranging from 0 to 1. 
    private float relativeTime;

    /// <summary>
    /// Plane used for picking (used for getting a jump destination)
    /// </summary>
    private Plane groundplane = new Plane(Vector3.up, 0.0f);

    // Use this for initialization
    public void Start()
    {
        touching = false;

        hintSplineDots = new GameObject[10];
        for (int i = 0; i < hintSplineDots.Length; ++i)
            hintSplineDots[i] = Instantiate(dot, Vector3.zero, Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!TouchInput.tapping)
        {

        }

        if (!touching && Input.GetMouseButtonDown(0))
        {
            touching = true;

            Vector2 screenPosition = Input.mousePosition; // TODO make compatible to tap!

            Ray pickingRay = Camera.main.ScreenPointToRay(screenPosition);
            float rayDist;
            if (groundplane.Raycast(pickingRay, out rayDist)) // should never be false.
            {
                touchStart = pickingRay.GetPoint(rayDist);
            }
        }

        if (touching)
        {
            // Released
            if (Input.GetMouseButtonUp(0))
            {
                touching = false;
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
        if (iceFloe == null)
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
