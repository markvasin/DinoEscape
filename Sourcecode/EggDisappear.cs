using UnityEngine;
using System.Collections;

public class EggDisappear : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter()
	{
		Destroy(this.gameObject,0.0f);
		
	}	
	
}
