﻿using UnityEngine;
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

    private void ResetPath(Collision col)
    {
        Debug.Log("reset Path");
        path = null;
    }

    private void UpdateCurrentMovement()
    {
        Vector3 startPosition = Position;
        Vector3 targetPosition = touchInput.lastTapStartPosition.groundPosition;

        Vector3 startMovement = currentFloe != null ? currentFloe.GetComponent<Rigidbody>().velocity : (targetPosition - startPosition).normalized;
        if (startMovement.magnitude < MIN_START_SPEED)
        {
            startMovement.Normalize();
            startMovement *= MIN_START_SPEED;
            if (currentFloe != null)
                currentFloe.GetComponent<Rigidbody>().velocity = startMovement;
        }
        Vector3 targetMovement = touchInput.lastTapReleasePosition.groundPosition - touchInput.lastTapStartPosition.groundPosition;

        if(touchInput.lastTapReleaseTime - touchInput.lastTapStartTime > Player.JUMP_TAP_DURATION)
        {
            path = new Helpers.HermiteSpline(startPosition, startMovement, targetPosition, targetMovement);
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
        movement *= Time.fixedDeltaTime / path.Length;
        movement *= SpeedFactor;
        movement.y = 0F;
        if (movement.sqrMagnitude > MaxSpeed * MaxSpeed)
            movement = movement.normalized * MaxSpeed;

        iceFloeRB.velocity = movement;
    }

    public void ChangeHint(Vector3 start, Vector3 direction)
    {
        hintPath = new Helpers.HermiteSpline(Position, path.EvaluateAt(relativeTime), start, direction);
        ShowHint();
    }

    public void ChangeHint(Vector3 start)
    {
        hintPath = new Helpers.HermiteSpline(Position, path.EvaluateAt(relativeTime), start);
        ShowHint();     
    }

    public void ResetHint()
    {
        hintPath = null; // new Helpers.HermiteSpline(transform.position, currentMovement.EvaluateAt(relativeTime), start);
        foreach (GameObject hintDot in hintSplineDots)
            hintDot.SetActive(false);

        //int numDots = this.
    }

    public void ShowHint()
    {
        if (hintPath == null)
            return;

        for (int i = 0; i < hintSplineDots.Length; ++i)
        {
            hintSplineDots[i].transform.position = hintPath.EvaluateAt((float)(i + 1) / hintSplineDots.Length);
            hintSplineDots[i].SetActive(true);
        }
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
