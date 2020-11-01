using UnityEngine;
using System.Collections;

public class Roundbar : MonoBehaviour {
	
	private float value =0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
//		Debug.Log("screen: " + Screen.width);
//		Debug.Log("mouse: " + Input.mousePosition.x);
//		if (Input.GetKey ("left")){ 
//			value+=10;
//			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, 100, value));
//			Debug.Log("1value: " + value);
//		}
//		if (Input.GetKey ("right")){ 
//			value-=10;
//			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, 100, value)); 
//			Debug.Log("2value: " + value);
//		}
		
	
		
		if (KinectManager.NumLife != 7){
		renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, 200f, 199.99f - KinectManager.NumEgg)); 
		}else renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, 200f, 0.0000001f));
		
	}
}
