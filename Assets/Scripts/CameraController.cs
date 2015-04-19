using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset = new Vector3(-5, 1, 0);
    public Vector3 Rotation = Vector3.zero;
    public float ZLerpSpeed = 0.01F;

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        transform.position = Player.position + Offset;
        transform.rotation = Quaternion.LookRotation(Rotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newZ = (Player.position + Offset).z;
        newZ = Mathf.Lerp(transform.position.z, newZ, ZLerpSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
