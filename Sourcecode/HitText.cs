using UnityEngine;
using System.Collections;

public class HitText : MonoBehaviour
{
	private GameObject hit;
	// Use this for initialization
	
	
	void Start ()
	{
	
		hit = GameObject.Find ("Hit");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void AppearHit ()
	{
		Debug.Log("appearhit");
		hit.renderer.enabled = true;
	}
	
	public void DisappearHit ()
	{
		Debug.Log("disappearhit");
		StartCoroutine (delay ());
		hit.renderer.enabled = false;
	}
	
	IEnumerator delay ()
	{
		yield return new WaitForSeconds(5);	
	}
	
	
}
