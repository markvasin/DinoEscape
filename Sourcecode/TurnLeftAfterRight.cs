using UnityEngine;
using System.Collections;

public class TurnLeftAfterRight : MonoBehaviour {

	public static float pos;
	

	// Use this for initialization
	void Start () {
		pos = transform.parent.position.x+125f;
	}
	
	void OnTriggerEnter ()
	{
		
		SmoothFollow.turns = true;
		if(AvatarController.Vmax == 15){
			AvatarController.turnspeed = 2f;
		}
		else if(AvatarController.Vmax == 20){
			AvatarController.turnspeed = 3f;
		}
		else if(AvatarController.Vmax == 25){
			AvatarController.turnspeed = 3.4f;
		}
	}
	
	
	void OnTriggerExit ()
	{
		//Debug.Log("x= "+transform.parent.position.x);
		//Debug.Log("pos= "+pos);
		AvatarController.LeftTurn = true;
		AvatarController.rightthenleft = true;
		GenObjScript.countObj = 5;
		StartCoroutine (delay (2f));
		
	}
	
	IEnumerator delay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SmoothFollow.turns = false;
		
	}
	
}
