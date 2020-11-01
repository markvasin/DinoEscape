using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {
	
	public GameObject[] character = new GameObject [3];
	
	// Use this for initialization
	void Start () {
		Destroy(GameObject.Find("MenuMusic"));
		character[KinectManager.CharacterNo].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Time.timeSinceLevelLoad > 8)
			Application.LoadLevel("LoadingScreen");
			
		
	}
}
