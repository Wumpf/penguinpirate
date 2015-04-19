using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour
{
	private const float SINK_MOVEMENT = 0.05f;
	public const float SUNK_HEIGHT = 0f;

	private Vector3 startPosition;
	private Quaternion startOrientation;
	private float startUpperBound;

	private UIControl UIScript;

    public delegate void EventHandler(Collision col);
    public event EventHandler OnCollision;

	private Collider collider;

	void Start()
	{
		collider = GetComponent<Collider>();
		startUpperBound = collider.bounds.max.y;
	}

    // Use this for initialization
	void OnEnable()
	{
		startPosition = transform.position;
		startOrientation = transform.rotation;

		UIScript = GameObject.Find("UI_Control").GetComponent<UIControl>();
	}

	/// <summary>
	/// Resets position and orientation to original
	/// </summary>
	public void Reset()
	{
		transform.position = startPosition;
		transform.rotation = startOrientation;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().rotation = Quaternion.identity;
	}

	// Update is called once per frame
	void Update()
	{
		// Shelves are sinking if they have a player transform child
		if (transform.GetComponentsInChildren(typeof(Transform)).Any(x => x.tag == "Player"))
		{
			transform.Translate(0, -SINK_MOVEMENT * Time.deltaTime, 0);
			
			float currentUpperBound = collider.bounds.max.y;
			UIScript.iceFloeTTL = ((SUNK_HEIGHT - currentUpperBound) / (SUNK_HEIGHT - startUpperBound)) * 100;

			if (currentUpperBound < SUNK_HEIGHT)
			{
				Debug.Log("icefloe with player has sunk. You lost.");
				GameObject.FindObjectOfType<PP_GameController>().gameEndStatus();
			}
		}

		// TODO add some buoyancy animation :)
	}

    void OnCollisionEnter(Collision col)
    {
        
        if(OnCollision != null)
        {
            OnCollision.Invoke(col);
        }
    }
}
