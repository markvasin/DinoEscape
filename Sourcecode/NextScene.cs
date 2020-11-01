using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {
	
	private float time;
	
	// Use this for initialization
	void Start () {
	
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	//Debug.Log(time);
		time = Time.time;
		if (time > 7)
		{
			Application.LoadLevel("Intro");
			
		}
	}
}
