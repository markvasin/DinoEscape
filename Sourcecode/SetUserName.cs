using UnityEngine;
using System.Collections;

public class SetUserName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		string playername = Recognition.UserName;
		//if user found
		if(playername != null)
		{
			string s = "Hello "+ playername +"!!";
			GameObject.Find("ShowUsertext").GetComponent<TextMesh>().text = s;
		}
		//if user not found
		else if(playername == null)
		{
			GameObject.Find("ShowUsertext").GetComponent<TextMesh>().text = "No User Found";
			GameObject.Find("Confirmbutton").renderer.enabled = false;
			GameObject.Find("Confirmtext").renderer.enabled = false;
			
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
