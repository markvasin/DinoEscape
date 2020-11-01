using UnityEngine;
using System.Collections;

public class TurnRight: MonoBehaviour {
	
	public static float pos;
	float destroyTime = 2.0f;
	bool Enter = false;
	
	// Use this for initialization
	void Start () {
		pos = transform.parent.position.z+125f;
	}
	
	void Update () {
		
	}	

	void OnTriggerEnter ()
	{
		AvatarController.turnspeed = 2f;
		SmoothFollow.turns = true;
		AvatarController.RightTurn = true;
		GenMap.MapPosGenX = transform.parent.position.x + 1000 + 125;
		GenMap.PosCross = transform.parent.position.z;
		GenObjScript.ProCrossX = transform.parent.position.x;
		Destroy(GameObject.Find("left"));

	}
	
	
	void OnTriggerExit ()
	{
		if(!Enter)
		{
			Enter = true;
			object[] RObjects = GameObject.FindGameObjectsWithTag("LeftT");
			foreach(GameObject thisObject in RObjects)
   				Destroy(thisObject,destroyTime);
			
			/*object[] RGObjects = GameObject.FindGameObjectsWithTag("LeftGen");
			foreach(GameObject GObject in RGObjects)
   				Destroy(GObject,destroyTime);*/
		
			StartCoroutine (delay (2f));
			
			
		}
		
	
	}
	
	IEnumerator delay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SmoothFollow.turns = false;
		
	}
}
