using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class REBACal{
	
	
	public Vector3 HeadPos;
	public Vector3 NeckPos;
	public Vector3 TorsoPos;
	public Vector3 HipPos;
	public Vector3 KneePos;
	public Vector3 AnklePos;
	public Vector3 ShouldPos;
	public Vector3 ElbowPos;
	public Vector3 HandPos;
	public Vector3 FingerPos;
	
	public int NeckVal = 0;
	public int TrunkVal = 0;
	public int LegVal = 1;
	public int UpperArmVal = 0;
	public int LowerArmVal = 0;
	public int WristVal = 0;
	
	public float LegA=0.0f;
	public float LegB=0.0f;
	public float LegC=0.0f;
	public float NeckAngle=0.0f;
	public float NeckSideAngle = 0.0f;
	public float TrunkAngle=0.0f;
	public float TrunkSideAngle = 0.0f;
	public float LegAngle=0.0f;
	public float UArmAngle=0.0f;
	public float UArmSideAngle=0.0f;
	public float LArmAngle=0.0f;
	public float WristAngle=0.0f;
	
	public int TableA;
	public int TableB;
	public int REBAScore;
	
	public int[,] TrunkLegA = new int[,] {{1,2,3,4},
								{2,3,4,5},
								{2,4,5,6},
								{3,5,6,7},
								{4,6,7,8}};
	
	public int[,] TrunkLegB = new int[,] {{1,2,3,4},
								{3,4,5,6},
								{4,5,6,7},
								{5,6,7,8},
								{6,7,8,9}};
	

	public int[,] TrunkLegC = new int[,] {{3,3,5,6},
								{4,5,6,7},
								{5,6,7,8},
								{6,7,8,9},
								{7,8,9,9}};
	
	public int[,] UArmWristA = new int[,] {{1,2,2},
								{1,2,3},
								{3,4,5},
								{4,5,5},
								{6,7,8},
								{7,8,8}};
	
	public int[,] UArmWristB = new int[,] {{1,2,3},
								{2,3,4},
								{4,5,5},
								{5,6,7},
								{7,8,8},
								{8,9,9}};
	
	public int[,] REBATable = new int[,] {{1,1,1,2,3,3,4,5,6,7,7,7},
								{1,2,2,3,4,4,5,6,6,7,7,8},
								{2,3,3,3,4,5,6,7,7,8,8,8},
								{3,4,4,4,5,6,7,8,8,9,9,9},
								{4,4,4,5,6,7,8,8,9,9,9,9},
								{6,6,6,7,8,8,9,9,10,10,10,10},
								{7,7,7,8,9,9,9,10,10,11,11,11},
								{8,8,8,9,10,10,10,10,10,11,11,11},
								{9,9,9,10,10,10,11,11,11,12,12,12}};

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void CalculationREBA(Vector3 JointHead, Vector3 JointNeck, Vector3 JointTorso, Vector3 JointKnee, Vector3 JointAnkle, Vector3 JointHip,Vector3 JointElbow, Vector3 JointShoulder,Vector3 JointHand, Vector3 JointFinger){
		
		//Neck Position
		NeckVal = 0;
		HeadPos = JointHead;
		NeckPos = JointNeck;
		//Debug.Log("Head pos" + HeadPos.z + "NeckPos" + NeckPos.z);
		//Locate Neck Position
		NeckAngle = (float)Math.Atan((NeckPos.z-HeadPos.z)/(HeadPos.y-NeckPos.y))*(float)(180/Math.PI);
		//Calculate Neck side angle
		NeckSideAngle = (float)Math.Abs(Math.Atan((HeadPos.x-NeckPos.x)/(HeadPos.y-NeckPos.y))*(float)(180/Math.PI));
		
		//Debug.Log("Neck Angle: " + NeckAngle + "NeckSideAngle: " + NeckSideAngle);
		if(NeckAngle>=0 && NeckAngle<=20){
			NeckVal++;
		}
		else if (NeckAngle > 20){
			NeckVal += 2;
		}
		else if (NeckAngle < 0){
			NeckVal += 2;
		}
		else{
			NeckVal++;
		}
		//if Neck is side bending
		if(NeckSideAngle >=15){
			NeckVal++;
		}
		
		//Trunk Position
		TrunkVal = 0;
		TorsoPos = JointTorso;
		//Locate Trunk Position
		TrunkAngle=(float)Math.Atan((TorsoPos.z-NeckPos.z)/(NeckPos.y-TorsoPos.y))*(float)(180/Math.PI);
		//Calculate Trunk side Angle
		TrunkSideAngle=(float)Math.Abs(Math.Atan((NeckPos.x-TorsoPos.x)/(NeckPos.y-TorsoPos.y))*(float)(180/Math.PI));
		//Debug.Log("Trunk Angle: " + TrunkAngle + "TrunkSideAngle: " + TrunkSideAngle);
		if(TrunkAngle == 0){
			TrunkVal++;
		}
		else if(TrunkAngle > 0 && TrunkAngle <= 20){
			TrunkVal += 2;
		}
		else if(TrunkAngle > 20 && TrunkAngle <= 60){
			TrunkVal += 3;
		}
		else if(TrunkAngle > 60){
			TrunkVal += 4;
		}
		else if(TrunkAngle < 0){
			TrunkVal += 2;
		}
		else{
			TrunkVal++;
		}
		//if Trunk is side bending
		if(TrunkSideAngle >= 15){
			TrunkVal++;
		}
		
		//Knee Angles
		LegVal = 1;
		KneePos = JointKnee;
		AnklePos = JointAnkle;
		HipPos = JointHip;
		//Locate Right Legs
		LegA = (float)Math.Sqrt((float)Math.Pow(HipPos.y-AnklePos.y,2)+(float)Math.Pow(AnklePos.z-HipPos.z,2));
		LegB = (float)Math.Sqrt((float)Math.Pow(KneePos.y-AnklePos.y,2)+(float)Math.Pow(KneePos.z-AnklePos.z,2));
		LegC = (float)Math.Sqrt((float)Math.Pow(HipPos.y-KneePos.y,2)+(float)Math.Pow(KneePos.z-HipPos.z,2));
		
		LegAngle = (float)Math.Acos(((float)Math.Pow(LegA,2)+(float)Math.Pow(LegC,2)-(float)Math.Pow(LegB,2))/(2*LegA*LegC))*(float)(180/Math.PI);
		//Debug.Log ("LegAngle: " + LegAngle);
		if(LegAngle >=30 && LegAngle <=60){
			LegVal++;
		}
		else if(LegAngle > 60){
			LegVal += 2;
		}
		else{
			LegVal++;
		}
		
		//Upper Arm Postion
		UpperArmVal = 0;
		ElbowPos = JointElbow;
		ShouldPos = JointShoulder;
		//Locate Upper Arm Position
		
		UArmAngle = (float)Math.Atan(((ShouldPos.z-ElbowPos.z))/(ShouldPos.y - ElbowPos.y))*(float)(180/Math.PI);
		//UArmSideAngle = (float)Math.Atan((((ElbowPos.x-ShouldPos.x)>0)?(ElbowPos.x-ShouldPos.x)*(-1):(ElbowPos.x-ShouldPos.x))/(ElbowPos.y-ShouldPos.y))*(float)(180/Math.PI);
		//Debug.Log("UArmAngle: " + UArmAngle + "UArmSideAngle: " + UArmSideAngle);
		
		if (UArmAngle >= 0 && UArmAngle <=20){
			UpperArmVal++;
		}
		else if (UArmAngle > 20 && UArmAngle <= 45){
			UpperArmVal += 2;
		}
		else if (UArmAngle > 45 && UArmAngle <= 90){
			UpperArmVal += 3;
		}
		else if (UArmAngle < 0){
			UpperArmVal += 4;
		}
		else{
			UpperArmVal++;
		}
		//if arm is abducted
		/*if(UArmSideAngle > 60)
		{
			UpperArmVal++;
		}*/
		
		
		//Lower Arm Position
		LowerArmVal = 0;
		HandPos = JointHand;
		//Locate Lower Arm Position
		LArmAngle = (float)Math.Atan(Math.Abs(HandPos.z-ElbowPos.z)/(HandPos.y-ElbowPos.y))*(float)(180/Math.PI);
		//Debug.Log("LowerArmAngle: " + LArmAngle);
		if(LArmAngle < 0){
			LArmAngle = Math.Abs(LArmAngle);
		}
		else if (LArmAngle >= 0){
			LArmAngle = 180-LArmAngle;
		}

		if(LArmAngle >= 60 && LArmAngle <= 100){
			LowerArmVal++;
		}
		else if((LArmAngle>=0 && LArmAngle <60)||(LArmAngle>100)){
			LowerArmVal += 2;
		}
		else{
			LowerArmVal++;
		}
		
		//Wrist Position
		WristVal = 0;
		FingerPos = JointFinger;
		//Locate Wrist Position
		WristAngle = (float)Math.Atan((HandPos.z-FingerPos.z)/(HandPos.y-FingerPos.y))*(float)(180/Math.PI);
		//Debug.Log("LowerWristAngle: " + WristAngle);
		WristAngle = 90-Math.Abs(WristAngle);
		if(WristAngle>=0 && WristAngle <= 15){
			WristVal++;
		}
		else if(WristAngle>15){
			WristVal += 2;
		}
		else{
			WristVal++;
		}
		
		CalREBA();
		
	}
	
	public void CalREBA (){
		
		if(NeckVal == 1){
			TableA = TrunkLegA[TrunkVal-1, LegVal-1];
		}
		else if(NeckVal == 2){
			TableA = TrunkLegB[TrunkVal-1, LegVal-1];
		}
		else if(NeckVal ==3){
			TableA = TrunkLegC[TrunkVal-1, LegVal-1];
		}
		
		if(LowerArmVal == 1){
			TableB = UArmWristA[UpperArmVal-1, WristVal-1];
		}
		else if(LowerArmVal == 2){
			TableB = UArmWristB[UpperArmVal-1, WristVal-1];
		}
		
		REBAScore = REBATable[TableA-1, TableB-1];
		
		//Debug.Log("REBAScore: " + REBAScore);
		if(REBAScore>=8)
		{
			
			object[] aObjects = GameObject.FindGameObjectsWithTag("ObjectGen");

			foreach(GameObject thisObject in aObjects)
   				if (((GameObject) thisObject).activeInHierarchy){
     			// Debug.Log(thisObject.name + " is an active object") ;
				if(!GenObjScript.BlackList.Contains(thisObject.name))
					GenObjScript.BlackList.Add(thisObject.name);
				
				//REBACollider.getRisk = true;
				break;
				}
			
		}
		
	}

	
}
