using UnityEngine;
using System.Collections;

public class ClearObjScript : MonoBehaviour {
	float destroyTime = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter () {
		Quaternion rot   = Quaternion.identity;
		Destroy(transform.parent.gameObject,destroyTime);//destroy this map 
		
	}
}
