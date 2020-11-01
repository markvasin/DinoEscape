using UnityEngine;
using System.Collections;

public class speed : MonoBehaviour {
	
	private float vmax;
	private bool Enter = false;
	
	// Use this for initialization
	void Start () {
		vmax = AvatarController.GameVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		//sDebug.Log(" "+AvatarController.Vmax);
	}
	
	void OnTriggerEnter()//take in Box Block
	{
		if(!Enter)
		{
			Enter = true;
			KinectManager.Score += 100;
			GameObject.Find("Score").GetComponent<TextMesh>().text = KinectManager.Score.ToString("00000");
			//AvatarController.velocity = 20;
			AvatarController.Vmax = 60f;
			StartCoroutine (delay(0.8f));
		}
	}
	
	IEnumerator delay(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		AvatarController.Vmax = vmax;
		
	}
	
	
	
	
	
}
