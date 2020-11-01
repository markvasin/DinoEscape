using UnityEngine;
using System.Collections;

public class getScore : MonoBehaviour {
	
	public static bool ExerScore = true;
	public static int endscore = 0;
	public static int LeftUsed = 0;
	public static int RightUsed = 0;
	public static int JumpUsed = 0;
	// Use this for initialization
	void Start ()
	{
		
		GameObject.Find("EndScore").GetComponent<TextMesh>().text = endscore.ToString("00000");
		
		if(!ExerScore)
		{
			GameObject.Find("LeftArmUsed").renderer.enabled = false;
			GameObject.Find("RightArmUsed").renderer.enabled = false;
			GameObject.Find("JumpCount").renderer.enabled = false;
		}
		else
		{
			GameObject.Find("LeftArmUsed").GetComponent<TextMesh>().text = "Left Arm\n\n" + LeftUsed;
			GameObject.Find("RightArmUsed").GetComponent<TextMesh>().text = "Right Arm\n\n" + RightUsed;
			GameObject.Find("JumpCount").GetComponent<TextMesh>().text = "Jump\n\n" + JumpUsed;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
