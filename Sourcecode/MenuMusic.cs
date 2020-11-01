using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {
	
	//public GameObject menumusic;
	
	void Awake() {
    // see if we've got menu music still playing
	//menumusic = GameObject.Find("MenuMusic");
 
    // make sure we survive going to different scenes
    DontDestroyOnLoad(gameObject);
}
}
