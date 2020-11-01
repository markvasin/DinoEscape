using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {
	
	public Transform ObjToShake;
	public Vector3 campos;
	
	// Use this for initialization
	void Start () {
	
		campos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		
		float randNrX = Random.Range(0.1f,-0.1f);
		float randNrY = Random.Range(0.1f,-0.1f);
		float randNrZ = Random.Range(0.1f,-0.1f);
		
		Vector3 shake = new Vector3(randNrX,randNrY,randNrZ);
	
		ObjToShake.transform.position += shake;
		//ObjToShake.transform.position = campos;
		
	}
}
