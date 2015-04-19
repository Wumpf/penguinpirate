using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset = new Vector3(-5, 1, 0);
    public Vector3 Rotation = Vector3.zero;
    public float LerpSpeed = 0.01F;

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
        transform.rotation = Quaternion.LookRotation(Rotation);
        Vector3 newPos = new Vector3(0, 0, Player.position.z);
        newPos += Offset;
        newPos = Vector3.Lerp(transform.position, newPos, LerpSpeed);
        transform.position = newPos;
    }
}
