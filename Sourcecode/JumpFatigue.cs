using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class JumpFatigue{
	
	private static float MaxHeight = 0;
	private static float MaxAll = 0;
	public static bool FirstHeight = false;
	private bool FirstTIMER = false;
	private System.Timers.Timer JumpBannedTimer;
	public static int seconds = 30;
	public static bool JumpAllowed = true;
	public static int countJump = 0;
	private Queue CountJumpQueue = new Queue();
	private float TimeJump = 0;
	private float CurrentHeadPost;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StopTimer()
	{
		JumpBannedTimer.Stop();
	}
	
	public void SetValue()
	{
		MaxHeight = 0;
		MaxAll = 0;
		FirstHeight = false;
		FirstTIMER = false;
		JumpAllowed = true;
		TimeJump = 0;
		countJump = 0;
		CountJumpQueue.Clear();
		JumpBannedTimer = new System.Timers.Timer();
		JumpBannedTimer.Stop();
	}
	
	public void JumpFatigueAnalysis()
	{
		float diffTime;
		FirstHeight = false;
		TimeJump = AvatarController.CalTime;
		CountJumpQueue.Enqueue(TimeJump);
		
		int NumJump = CountJumpQueue.Count;
		//Debug.Log(CountJumpList[0]+"jaa");
		//count = 0;
		countJump++;
		//Debug.Log("Jump: "+countJump);
		while(true)
		{
			//count++;
			//Debug.Log("Count in While: " + count);
			//Debug.Log ("Jumping Time: " + TimeJump);
			diffTime = TimeJump - (float)CountJumpQueue.Peek();
			if(diffTime > 30 && CountJumpQueue.Count != 0)
				CountJumpQueue.Dequeue();
			else
				break;
		}
		
		if(MaxAll < MaxHeight)
			MaxAll = MaxHeight;
		else if (MaxAll > MaxHeight && (MaxHeight/MaxAll)*100 <= 70)
		{
			//Debug.Log("Banned MaxAll: " + MaxAll + " MaxHeight: " + MaxHeight);
			Debug.Log("You are tired from jumping");
			
			JumpAllowed = false;
			MaxAll = MaxHeight;
			CountJumpQueue.Clear();
		}
		else if(JumpAllowed && CountJumpQueue.Count >= 7)
		{
			JumpAllowed = false;
			CountJumpQueue.Clear();
			Debug.Log("You are jumping too many times");
		}
		if(!JumpAllowed && !FirstTIMER)
		{
			FirstTIMER = true;
			// Hook up the Elapsed event for the timer.
			JumpBannedTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			// Set the Interval to 0.1 seconds (100 milliseconds).
			JumpBannedTimer.Interval = seconds * 1000;
	    	JumpBannedTimer.Enabled = true;
			JumpBannedTimer.Start();
		}
		//Debug.Log("MaxAll: " + MaxAll + " MaxHeight: " + MaxHeight);
		//Debug.Log("JumpQueue " + CountJumpQueue.Count);
		
				
		
	}
	
	public void MaxHeightCalculation(Vector3 HeadPos, float FirstHeadPos)
	{
		//Debug.Log("FirstHead: " + FirstHeadPos + "HeadPos");
		CurrentHeadPost = HeadPos.y-FirstHeadPos;
		
		//Max at Seperated Jump
		if(!FirstHeight)
		{	
			FirstHeight = true;
			MaxHeight = CurrentHeadPost;
			
		}
	
		if(CurrentHeadPost > MaxHeight)
		{
			MaxHeight = CurrentHeadPost;
		}
		
			
		
	}
		
	private void OnTimedEvent(object source, ElapsedEventArgs e)
	{
		Debug.Log("Jumping banned finish(30 secs)");
		JumpBannedTimer.Stop();
		JumpAllowed = true;
		FirstTIMER = false;
	}
		
	}
	

