using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour
{
    private float lastTapTime = -999999.0f;

    /// <summary>
    /// Plane used for picking (used for getting a jump destination)
    /// </summary>
    private Plane groundplane = new Plane(Vector3.up, 0.0f);

    /// <summary>whether the player touches the screen / mouse button is down</summary>
    public bool tapping { get; private set; }

    /// <summary>last time, the player started tapping</summary>
    public float lastTapStartTime { get; private set; }

    /// <summary>last time, the player stopped tapping</summary>
    public float lastTapReleaseTime { get; private set; }

    public Vector2 screenPosition { get; private set; }

    /// <summary>currently tapped Position in XZ-Plane</summary>
    public Vector3 groundPosition { get; private set; }


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!tapping && Input.GetMouseButtonDown(0))
        {
            tapping = true;
            lastTapStartTime = Time.time;
        }
        else if (tapping && Input.GetMouseButtonDown(0))
        {
            tapping = false;
            lastTapReleaseTime = Time.time;
        }

        if (tapping)
        {
            Vector2 screenPosition = Input.mousePosition; // TODO make compatible to tap!

            Ray pickingRay = Camera.main.ScreenPointToRay(screenPosition);
            float rayDist;
            if (groundplane.Raycast(pickingRay, out rayDist)) // should never be false.
            {
                groundPosition = pickingRay.GetPoint(rayDist);
            }
        }
    }
}
