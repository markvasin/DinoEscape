using UnityEngine;
using System.Collections;

public class TrexSound : MonoBehaviour {
	
	public AudioClip Sound;
	private bool play;
	public Transform ObjToShake;
	public Vector3 campos;
	public AudioClip FootSound;
	private bool play1;
	private bool play2;
	private bool play3;
	private bool play4;
	
	
	// Use this for initialization
	void Start () {
	play = false;
	play1 = false;
	play2 = false;
	play3 = false;	
	play4 = false;
	campos = new Vector3(-4.768363f,1f,39.86575f);

	}
	
	// Update is called once per frame
	void Update () {
		
		//Debug.Log(Time.timeSinceLevelLoad);
		if (Time.timeSinceLevelLoad > 0.5 && Time.timeSinceLevelLoad < 4 ){
		if(!play){
			play=true;
		AudioSource.PlayClipAtPoint (Sound, transform.position,1f);
		//audio.PlayOneShot(Sound);
		}
		float randNrX = Random.Range(-4.768363f,-4.8f);
		float randNrY = Random.Range(1f,1.1f);
		float randNrZ = Random.Range(39.86575f,39.9f);
		Vector3 shake = new Vector3(randNrX,randNrY,randNrZ);
		ObjToShake.transform.position = shake;
		//campos = ObjToShake.transform.position;
		}
		
		
		if (Time.timeSinceLevelLoad > 4 )
			ObjToShake.transform.position = campos;
		
		
//		if (Time.timeSinceLevelLoad > 7.41 && Time.timeSinceLevelLoad < 8.41){
//			if(!play1){
//			play1 = true;
//			audio.Play();
//			}
//		}
//		
//		else if (Time.timeSinceLevelLoad > 6.540979 && Time.timeSinceLevelLoad < 7.41){
//			if(!play2){
//			play2 = true;
//			audio.Play();
//			}
//		}
//		
//		else if (Time.timeSinceLevelLoad > 5.730179 && Time.timeSinceLevelLoad < 6.540979){
//			if(!play3){
//			play3 = true;
//			audio.Play();
//			}
//		}
//		
//		else if (Time.timeSinceLevelLoad > 4.85098 && Time.timeSinceLevelLoad<5.730179){
//			if(!play4){
//			play4 = true;
//			audio.Play();
//			}
//		}
		
		
	}
}
