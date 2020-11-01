using UnityEngine;
using System.Collections;

public class InputNameScript : MonoBehaviour {
	
	//initial value
	public static string UserName = "";
	
	void Start ()
	{
		guiText.text = "";
		UserName = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		
		
		foreach (char inputName in Input.inputString)
		{
			if ( (inputName < 'a' || inputName > 'z') && (inputName < 'A' || inputName > 'Z') && (inputName < '0' || inputName > '9')&& inputName!= "\b"[0] ) 
				break;
			
			if(inputName != "\b"[0] && UserName.Length < 7)
			{
			
				guiText.text = guiText.text + inputName;
				UserName = guiText.text;
				
			}
			//if the input is backspace(delete character)
			else if(inputName == "\b"[0])
			{
			
				if (guiText.text.Length != 0)
					guiText.text = guiText.text.Substring(0,guiText.text.Length-1);
			
				UserName = guiText.text;
			
			}
	
		}
	
	}
}
