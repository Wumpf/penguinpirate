using UnityEngine;
using System.Collections;

class PlayerMovementController : MonoBehaviour
{
    public TouchInput touchInput;

    public static float SECONDS_PER_SPLINE = 10;
    public static float MIN_DIRECTION_SIZE_PIXELS = 10;
    public static float TAP_DURATION = 0.15f;

    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }

    private Helpers.HermiteSpline currentMovement;
    private Helpers.HermiteSpline hintMovement;

    public GameObject dot;
    private GameObject[] hintSplineDots;

    private bool touching;
    private Vector3 touchStart;

    // Time ranging from 0 to 1. 
    private float relativeTime;

    /// <summary>
    /// Plane used for picking (used for getting a jump destination)
    /// </summary>
    private Plane groundplane = new Plane(Vector3.up, 0.0f);

    // Use this for initialization
    public void Start()
    {
        touchInput.onTapRelease += UpdateCurrentMovement;

        touching = false;

        hintSplineDots = new GameObject[10];
        for (int i = 0; i < hintSplineDots.Length; ++i)
            hintSplineDots[i] = Instantiate(dot, Vector3.zero, Quaternion.identity) as GameObject;
    }

    private void UpdateCurrentMovement()
    {
        if(touchInput.lastTapReleaseTime - touchInput.lastTapStartTime < TAP_DURATION)
        {
            currentMovement = new Helpers.HermiteSpline(Position, transform.forward, touchInput.lastTapReleasePosition.groundPosition);
        }
        else
        {
            Vector3 targetDirection = touchInput.lastTapReleasePosition.groundPosition - touchInput.lastTapStartPosition.groundPosition;
            currentMovement = new Helpers.HermiteSpline(Position, transform.forward, touchInput.lastTapReleasePosition.groundPosition, targetDirection);
        }
    }

    float t = 0;

    void Update()
    {
        if(currentMovement != null)
        {
            transform.position = currentMovement.EvaluateAt(t);
            t += 0.01F;
            t %= 1F;
        }
        
    }
    
    /*
    void FixedUpdate()
    {
        Rigidbody iceFloe = transform.parent.GetComponent<Rigidbody>();
        if (iceFloe == null)
        {
            return;
        }

        float currentT;
        currentMovement.GetNearestPosition(Position, out currentT);
        iceFloe.AddForce(currentMovement.DirectionAt(currentT));
    }*/

    public void ChangeHint(Vector3 start, Vector3 direction)
    {
        hintMovement = new Helpers.HermiteSpline(Position, currentMovement.EvaluateAt(relativeTime), start, direction);
        ShowHint();
    }

    public void ChangeHint(Vector3 start)
    {
        hintMovement = new Helpers.HermiteSpline(Position, currentMovement.EvaluateAt(relativeTime), start);
        ShowHint();     
    }

    public void ResetHint()
    {
        hintMovement = null; // new Helpers.HermiteSpline(transform.position, currentMovement.EvaluateAt(relativeTime), start);
        foreach (GameObject hintDot in hintSplineDots)
            hintDot.SetActive(false);

        //int numDots = this.
    }

    public void ShowHint()
    {
        if (hintMovement == null)
            return;

        for (int i = 0; i < hintSplineDots.Length; ++i)
        {
            hintSplineDots[i].transform.position = hintMovement.EvaluateAt((float)(i + 1) / hintSplineDots.Length);
            hintSplineDots[i].SetActive(true);
        }
    }
}
