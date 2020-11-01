using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour
{
	
	//public GameObject Healthbar;
	private int temp = 7;
	
	//private GameObject leafbar;
	
	
	// Use this for initialization
	void Start ()
	{
		GameObject.Find("NamePlay").GetComponent<TextMesh>().text = Recognition.UserName;
	}
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("numlife: " + KinectManager.NumLife);
		
		if (KinectManager.NumLife <= 1) {
			renderer.material.color = Color.red;
		} else if (KinectManager.NumLife <= 2) {
			renderer.material.color = Color.yellow;
		} else if (KinectManager.NumLife >= 3) {
			renderer.material.color = Color.green;
		}
		
		
		if (temp != KinectManager.NumLife) {
				
			if (temp > KinectManager.NumLife) {
				//Debug.Log("1temp: " + temp + " numlife: " + KinectManager.NumLife);
				transform.localScale = new Vector3 (transform.localScale.x - 1f, transform.localScale.y, transform.localScale.z);		
				transform.localPosition = new Vector3 (transform.localPosition.x + 0.5f, transform.localPosition.y, transform.localPosition.z);
			} else if (temp < KinectManager.NumLife) {
				//Debug.Log("2temp: " + temp + " numlife: " + KinectManager.NumLife);
				transform.localScale = new Vector3 (transform.localScale.x + 1f, transform.localScale.y, transform.localScale.z);
				transform.localPosition = new Vector3 (transform.localPosition.x - 0.5f, transform.localPosition.y, transform.localPosition.z);
			}
			
			temp = KinectManager.NumLife;
		
		}
	
	}
	
}
