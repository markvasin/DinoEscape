using UnityEngine;
using System.Collections;

public class BoxCollision : MonoBehaviour
{
	
	
	public AudioClip Sound;
	public bool getDamage = false;//prevent noise	
	private GameObject hit;
	private GameObject character;
	public static bool bounce = false;
	
	
	void Start ()
	{
		hit = GameObject.Find ("Hit");
		hit.renderer.enabled = false;
		character = GameObject.FindGameObjectWithTag("character");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	
	}

	void OnTriggerEnter (Collider collision)//take in Box Block
	{
		
		//if(collision.tag == "character")
		//{
		hit.renderer.enabled = true;
		bounce = true;
		AudioSource.PlayClipAtPoint(Sound,transform.position,1f);
		
		
		if (!getDamage) {
			getDamage = true;
			if (KinectManager.NumLife > 0){
			KinectManager.NumLife--;
			//KinectManager.LifeObj [KinectManager.NumLife].SetActive (false);
			}
			if (KinectManager.NumLife == 0) {
			KinectManager.EndGame = true;
		}
			
			
			
			
		}
		//}
	
	
	}

	void OnTriggerExit (Collider collision)//after pass this box half of the time to max acceleration
	{
		//Debug.Log ("exit");
		/*
		ZigSkeleton.timeRun=ZigSkeleton.Tmax/2;
		ZigSkeleton.stop=0;
		*/
		//if(collision.tag == "character")
		//{
		if(KinectManager.Score> KinectManager.ExtremeModeScore && KinectManager.ExtremeModeOn)
				getDamage = false;
		
		AvatarController.timeRun = AvatarController.Tmax / 2;
		AvatarController.stop = 0;
		StartCoroutine (HitDisappear (1f));
		StartCoroutine (delay (0.2f));
		//}
	}

	void OnTriggerStay (Collider collision)
	{
		/*
		ZigSkeleton.stop=1;
		ZigSkeleton.timeRun=0;
		*/
		//Debug.Log("STAY!!!!!!!!!!");
			
		//if(collision.tag == "character")
		//{
		AvatarController.stop = 1;
		AvatarController.timeRun = 0;
		if (!bounce)
		{
		
//		Vector3 temp = character.transform.position;
//		if((AvatarController.LeftTurn && AvatarController.RightTurn)||((!AvatarController.LeftTurn && !AvatarController.RightTurn)))
//			temp.z -= 5f;
//		else if(AvatarController.LeftTurn && !AvatarController.RightTurn)
//			temp.x += 5f;
//		else if(!AvatarController.LeftTurn && AvatarController.RightTurn)
//			temp.x -= 5f;	
//		//character.transform.position = temp;
//		Debug.Log("temp: " + temp);
//		Debug.Log("character.transform.position: "+character.transform.position);
//		character.transform.position = Vector3.Lerp(character.transform.position,temp,Time.deltaTime*50f);
			
			
		//Debug.Log("bounce");
		}
		//AvatarController.charZ -= 5f;
		//}
		//AvatarController.velocity = 0;
		//Debug.Log ("velocitystay " + AvatarController.velocity);
	}
	
	IEnumerator HitDisappear(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		hit.renderer.enabled = false;
	}
	
	IEnumerator delay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		bounce = false;
	}
	
}
