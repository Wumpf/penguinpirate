using UnityEngine;
using System.Collections;

public class WaterCollision : MonoBehaviour {


	public GameObject waterSplash;
	public AudioClip waterSplashSfx;
	public AudioSource aSource;

	void OnTriggerEnter(Collider collidedObj){

		if (collidedObj.gameObject.name == "Player"){

			//play water splash sound
			aSource.clip = waterSplashSfx;
			aSource.Play();

			// instantiate a particle system of water splash
			GameObject splash = Instantiate(waterSplash,collidedObj.transform.localPosition,Quaternion.identity) as GameObject;
			ParticleSystem ps = splash.GetComponent<ParticleSystem>();
			Destroy(splash,ps.duration);
			}
		}

	void OnTriggerStay(Collider collidedObj){

		if (collidedObj.gameObject.name == "Player") {
			//player died, call game lose method of gamecontroller
			PP_GameController gameCtrl = Camera.main.GetComponent<PP_GameController>();
			gameCtrl.gameEndStatus();
		}
	}

}
