using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class ArmFatigue{
	
	
	private Vector3 Velocity;
	private Vector3 DistanceI;
	private Vector3 DistanceF;
	private Vector3 tempDistanceF;
	private float TimeI = 0;
	private float TimeF = 0;
	private float difftime = 0;
	private float MagVelocity;
	private float MaxVelocity;

	private bool FirstDis = false;
	private bool FirstMax = false;
	public bool isFall = true;
	private bool FirstTimeArm = false;
	private int countArmUse = 0;
	private System.Timers.Timer ArmBannedTimer;
	public bool ResultBanned = true;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetValue()
	{
		Velocity = Vector3.zero;
		DistanceI = Vector3.zero;
		DistanceF = Vector3.zero;
		tempDistanceF = Vector3.zero;
		TimeI = 0;
		TimeF = 0;
		difftime = 0;
		MagVelocity = 0;
		MaxVelocity = 0;
		FirstDis = false;
		FirstMax = false;
		isFall = true;
		FirstTimeArm = false;
		countArmUse = 0;
		ResultBanned = true;
		ArmBannedTimer = new System.Timers.Timer();
		ArmBannedTimer.Stop();
		
	}
	
	public void StopTimer()
	{
		ArmBannedTimer.Stop();
	}
	
	public int getCountArm()
	{
		return countArmUse;
	}
	
	public void ArmFatigueCal(Vector3 ArmPos){
		
		
		//Debug.Log("ArmPos: " + ArmPos);
		if(!FirstDis){
			//DistanceI = new Vector3(JAccelerate.position.x,JAccelerate.position.y,JAccelerate.position.z);
			FirstDis = true;
			isFall = false;
			DistanceI = ArmPos;
			TimeI = AvatarController.CalTime;
			DistanceF = ArmPos;
			TimeF = TimeI;
			//Debug.Log("DistanceI: " + DistanceI + " TimeI: " + TimeI);
		}
		else
		{
			tempDistanceF = ArmPos;
			//Debug.Log("DistanceF: " + DistanceF + " Current: " + tempDistanceF);
			if(DistanceF.y < tempDistanceF.y)
			{
				DistanceF = tempDistanceF;
				TimeF = AvatarController.CalTime;
			//	Debug.Log("DistanceFMore: " + DistanceF + " Current: " + tempDistanceF);
			}
			//if arm going downward
			else if(DistanceF.y > tempDistanceF.y)
			{
				if(!isFall)
				{
					countArmUse++;
					isFall = true;
					FirstDis = false;
					difftime = TimeF-TimeI;
					Velocity = new Vector3(Math.Abs(DistanceF.x-DistanceI.x)/(difftime),Math.Abs(DistanceF.y-DistanceI.y)/(difftime),Math.Abs(DistanceF.z-DistanceI.z)/(difftime));
					
					MagVelocity = Velocity.magnitude;
					//Debug.Log("VelMag: " + MagVelocity);
					if(MagVelocity >= 1)
					{
						if(!FirstMax)
						{
							FirstMax = true;
							MaxVelocity = MagVelocity;
						}
						else if(MaxVelocity < MagVelocity)
						{
							MaxVelocity = MagVelocity;
						}
						else if(MaxVelocity > MagVelocity && MagVelocity/MaxVelocity*100 <=60 && !FirstTimeArm)
						{	
							FirstTimeArm = true;
							
							// Hook up the Elapsed event for the timer.
							ArmBannedTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
							// Set the Interval to 0.1 seconds (100 milliseconds).
							ArmBannedTimer.Interval = JumpFatigue.seconds * 1000;
	       		    		ArmBannedTimer.Enabled = true;
							ArmBannedTimer.Start();
							ResultBanned = false;
							MaxVelocity = MagVelocity;
							//Debug.Log("Your Arm is tired");
						}
					}
					//Debug.Log("Max: " + MaxVelocity + " Current: " + MagVelocity);
				}
			}
		
			
		}
		
	}
	
	private void OnTimedEvent(object source, ElapsedEventArgs e)
	{
			//Debug.Log("Arm banned finish (30 seconds)");
			ArmBannedTimer.Stop();
			ResultBanned = true;
			FirstTimeArm = false;
	}
	
}
