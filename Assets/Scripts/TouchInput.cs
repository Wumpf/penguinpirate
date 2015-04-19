using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour
{
    public struct PositionInfo
    {
        public Vector3 hitPosition;
        public Vector3 groundPosition;
        public Vector2 screenPosition;
    }

    /// <summary>
    /// Plane used for picking (used for getting a jump destination)
    /// </summary>
    private Plane groundplane = new Plane(Vector3.up, 0.0f);


    /// <summary>whether the player touches the screen / mouse button is down</summary>
    public bool tapping { get; private set; }

    
    /// <summary>last time, the player started tapping</summary>
    public float lastTapStartTime { get; private set; }

    /// <summary>last time, the player started tapping</summary>
    public PositionInfo lastTapStartPosition { get; private set; }

    
    /// <summary>last time, the player stopped tapping</summary>
    public float lastTapReleaseTime { get; private set; }

    /// <summary>last time, the player started tapping</summary>
    public PositionInfo lastTapReleasePosition { get; private set; }


    public PositionInfo position { get; private set; }

    /// <summary>currently tapped Position in XZ-Plane</summary>
    public Vector3 groundPosition { get; private set; }

    public delegate void EventHandler();
    public event EventHandler onTapStart;
    public event EventHandler onTapRelease;

    // Use this for initialization
    void Start()
    {
        lastTapReleaseTime = float.MinValue;
        lastTapStartTime = float.MinValue;

        position = new PositionInfo();
        lastTapStartPosition = new PositionInfo();
        lastTapReleasePosition = new PositionInfo();
    }

    // Update is called once per frame
    void Update()
    {
        bool tapStart = !tapping && Input.GetMouseButtonDown(0);
        bool tapRelease = tapping && Input.GetMouseButtonUp(0);

        if (tapStart)
        {
            tapping = true;
            lastTapStartTime = Time.time;
        }
        else if (tapRelease)
        {
            tapping = false;
            lastTapReleaseTime = Time.time;
        }


        PositionInfo currentPosition = new PositionInfo();

        //if (tapping)
        {
            currentPosition.screenPosition = Input.mousePosition;

            Ray pickingRay = Camera.main.ScreenPointToRay(currentPosition.screenPosition);
            float rayDist;
            if (groundplane.Raycast(pickingRay, out rayDist)) // should never be false.
            {
                currentPosition.groundPosition = pickingRay.GetPoint(rayDist);
            }

            RaycastHit hit;
            if (Physics.Raycast(pickingRay, out hit))
            {
                currentPosition.hitPosition = hit.point;
            }
        }

        position = currentPosition;

        if (tapStart)
        {
            lastTapStartPosition = currentPosition;
            if (onTapStart != null)
            {
                onTapStart.Invoke();
            }
        }
        else if (tapRelease)
        {
            lastTapReleasePosition = currentPosition;
            if (onTapRelease != null)
            {
                onTapRelease.Invoke();
            }
        }
    }
}
