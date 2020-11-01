using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class ConfigFile : MonoBehaviour {
	
	private static bool done = false;
	
	void Start () 
	{
		//get config value only once
		if(!done)
		{
			int i, times=0;
			//Debug.Log("config");
			StreamReader reader;
		   	// int count = 0;  
		
		
			if(File.Exists("config.txt"))
			{
				using ( reader = File.OpenText("config.txt"))  
       			{  
					string line; 
			
            		while ((line = reader.ReadLine()) != null)  
					{
						times++;
					}
				 }
				reader.Close();
	
				using ( reader = File.OpenText("config.txt"))  
        		{
					string []parts;
			
					for(i=0; i<times; i++)
					{
						parts = reader.ReadLine().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
				
						string variable = parts[0];
						string valueInput = parts[1]; 
				
						if(variable == "EasySpeed")
							KinectManager.easyspeed= float.Parse(valueInput);
							
						if(variable == "MediumSpeed")
							KinectManager.mediumspeed = float.Parse(valueInput);
							
						if(variable == "HardSpeed")
							KinectManager.hardspeed = float.Parse(valueInput);
							
						if(variable == "KinectAngle")
							KinectManager.SensorAngle = Int32.Parse(valueInput);	
							
						if(variable == "ColorMapping")
						{
							if(valueInput=="off")
								KinectManager.ColorMapping = false;
							if(valueInput=="on")
								KinectManager.ColorMapping = true;	
						}
								
						if(variable == "FatigueBannedTime")		
							JumpFatigue.seconds = Int32.Parse(valueInput);
								
						if(variable == "ExtremeMode")		
						{
							if(valueInput=="on")
								KinectManager.ExtremeModeOn = true;
							if(valueInput=="off")
								KinectManager.ExtremeModeOn = false;
						}
						
						if(variable == "ExtremeModeScore")		
							KinectManager.ExtremeModeScore = Int32.Parse(valueInput);
									
						if(variable == "ShowExerciseScore")		
						{	
							if(valueInput=="on")
								getScore.ExerScore = true;
							if(valueInput=="off")
								getScore.ExerScore = false;
						}
						
						if(variable == "StudyMode")			
						{		
							if(valueInput=="on")
								KinectManager.StudyMode = true;
							if(valueInput=="off")
								KinectManager.StudyMode = false;
						}
				
					}
		
				}
		
				reader.Close();
			}
		
			else
				Debug.Log("Config file not exist.");
			
			done = true;
		}
		
	}
	
	
}
