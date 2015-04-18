using UnityEngine;
using System.Collections;

public class WaterCollision : MonoBehaviour {


	public GameObject waterSplash;
	public AudioClip waterSplashSfx;
	public AudioSource aSource;

	void OnTriggerEnter(Collider collidedObj){

		if (collidedObj.gameObject.name == "Player" || collidedObj.transform.parent.gameObject.name == "IceFloePlayer"){

			//play water splash sound
			aSource.clip = waterSplashSfx;
			aSource.Play();

			// instantiate a particle system of water splash
			Vector3 waterSplashPos = new Vector3(collidedObj.transform.position.x,collidedObj.transform.position.y+1f,collidedObj.transform.position.z);
			GameObject splash = Instantiate(waterSplash,waterSplashPos,Quaternion.identity) as GameObject;
			ParticleSystem ps = splash.GetComponent<ParticleSystem>();
			Destroy(splash,ps.duration);
			}
		}

	// This is handled in the player, since... because.. well there is the code that works. This code here triggers loosing too early. Please don't ask any more questions.
	/*void OnTriggerStay(Collider collidedObj){

		if (collidedObj.gameObject.name == "Player") {
			//player died, call game lose method of gamecontroller
			PP_GameController gameCtrl = Camera.main.GetComponent<PP_GameController>();
			gameCtrl.gameEndStatus();
		}
	}*/

}
