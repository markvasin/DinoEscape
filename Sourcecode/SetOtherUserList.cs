using UnityEngine;
using System.Collections;

public class SetOtherUserList : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
		
			if (Recognition.NameList[0]!=null)
				GameObject.Find("User1Text").GetComponent<TextMesh>().text = Recognition.NameList[0];
			else
			{
				GameObject.Find("User1Button").SetActive(false);
				//GameObject.Find("User1Text").renderer.enabled = false;
			}
			if (Recognition.NameList[1]!=null)
				GameObject.Find("User2Text").GetComponent<TextMesh>().text = Recognition.NameList[1];
			else
			{
				GameObject.Find("User2Button").SetActive(false);
				//GameObject.Find("User2Text").renderer.enabled = false;
			}
			if (Recognition.NameList[2]!=null)
				GameObject.Find("User3Text").GetComponent<TextMesh>().text = Recognition.NameList[2];
			else
			{
				GameObject.Find("User3Button").SetActive(false);
				//GameObject.Find("User3Text").renderer.enabled = false;
			}
			
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
