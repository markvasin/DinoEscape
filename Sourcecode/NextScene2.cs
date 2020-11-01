using UnityEngine;
using System.Collections;

public class NextScene2 : MonoBehaviour {

	private float time;
	
	// Use this for initialization
	void Start () {
	Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	//Debug.Log(time);
		time = Time.timeSinceLevelLoad;
		if (time > 6)
		{
			Application.LoadLevel("Menu");
			
		}
	}
}
