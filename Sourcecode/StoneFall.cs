using UnityEngine;
using System.Collections;


public class StoneFall : MonoBehaviour {
	public GameObject stone;
	public GameObject wall;
	int random = 0;
	private bool fall = false;
	//private float fallspeed = 5;
	
	// Use this for initialization
	void Start () {
	//tree.rigidbody.useGravity = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter () {
		
		if (!fall){
			fall = true;
		random = Random.Range(0,3);
		//Debug.Log(random);
		}
			//tree.rigidbody.useGravity = true;
	}
		
	void OnTriggerExit()
	{
		
		if(random == 2)
		{
		wall.SetActive(true);
		stone.SetActive(true);
		stone.rigidbody.useGravity = true;
		}
		//Destroy(transform.parent.gameObject,2);
	}
	
}
