using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class PlayerMovementController : MonoBehaviour
{
    public TouchInput touchInput;


    public float MIN_START_SPEED;

    public float MaxSpeed;

    public float SpeedFactor;

    public Vector3 Position 
    { 
        get 
        { 
            return transform.parent != null ? transform.parent.position : transform.position; 
        }
    }

    IceFloe _currentFloe = null;
    public IceFloe currentFloe
    {
        get { return _currentFloe; }
        set
        {
            if(_currentFloe != null)
                _currentFloe.OnCollision -= ResetPath;
            _currentFloe = value;
            if (_currentFloe != null)
                _currentFloe.OnCollision += ResetPath;
        }
    }

    private Helpers.HermiteSpline path;
    private Helpers.HermiteSpline hintPath;

    public GameObject dot;
    private GameObject[] HintDots;
    private GameObject[] PathDots;

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
        touchInput.onTapRelease += UpdatePath;

        touching = false;

        PathDots = new GameObject[10];
        HintDots = new GameObject[10];
        for (int i = 0; i < PathDots.Length; ++i)
        {
            PathDots[i] = Instantiate(dot, Vector3.zero, Quaternion.identity) as GameObject;
            //HintDots[i] = Instantiate(dot, Vector3.zero, Quaternion.identity) as GameObject;
        }
    }

    private void ResetPath(Collision col)
    {
        Debug.Log("reset Path");
        path = null;
    }

    private void UpdatePath()
    {
        Vector3 startPosition = Position;
        Vector3 targetPosition = touchInput.lastTapStartPosition.groundPosition;

        Vector3 startMovement;
        if (transform.parent != null && transform.parent.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0F)
            startMovement = transform.parent.GetComponent<Rigidbody>().velocity;
        else
            startMovement = (targetPosition - startPosition).normalized;

        if (startMovement.magnitude < MIN_START_SPEED)
        {
            startMovement.Normalize();
            startMovement *= MIN_START_SPEED;
            if (transform.parent != null)
            {
                transform.parent.GetComponent<Rigidbody>().velocity = startMovement;
            }
        }

        Vector3 targetMovement = touchInput.lastTapReleasePosition.groundPosition - touchInput.lastTapStartPosition.groundPosition;

        if (touchInput.lastTapReleaseTime - touchInput.lastTapStartTime > Player.JUMP_TAP_DURATION)
        {
            path = new Helpers.HermiteSpline(startPosition, startMovement, targetPosition, targetMovement);
        }
    }

    void Update()
    {
        for (int i = 0; i < PathDots.Length; ++i)
        {
            PathDots[i].SetActive(path != null);
            if (path != null)
                PathDots[i].transform.position = path.EvaluateAt((float)(1 + i) * 1F / (float)PathDots.Length);
        }
    }

    void FixedUpdate()
    {
        if (transform.parent == null || path == null)
            return;

        Rigidbody iceFloeRB = transform.parent.GetComponent<Rigidbody>();
        if (iceFloeRB == null)
            return;

        float currentT;
        path.GetNearestPosition(Position, out currentT);

        Vector3 movement;
        movement = path.MovementAt(currentT);
        movement *= Time.fixedDeltaTime;
        movement *= SpeedFactor;
        movement.y = 0F;
        if (movement.sqrMagnitude > MaxSpeed * MaxSpeed)
            movement = movement.normalized * MaxSpeed;

        iceFloeRB.velocity = movement;
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.DrawLine(path.GetNearestPosition(Position), path.GetNearestPosition(Position) + Vector3.up * 2F);
        }
        DrawPath();
    }

    void DrawPath()
    {
        Gizmos.color = Color.white;
        if (path != null)
        {
            float stepSize = 0.05F;
            for (float t = 0F; t <= 1F; t += stepSize)
            {
                Gizmos.DrawSphere(path.EvaluateAt(t), 0.1F);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(touchInput.lastTapStartPosition.groundPosition, 0.5F);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(touchInput.lastTapReleasePosition.groundPosition, 0.5F);
    }
}
