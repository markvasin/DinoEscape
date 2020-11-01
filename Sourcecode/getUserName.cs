using UnityEngine;
using System.Collections;

public class getUserName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh>().text = Recognition.UserName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
