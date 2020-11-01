using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public class AvatarController : MonoBehaviour
{	
	//public Texture[] aTexture = new Texture[20];
	public static bool hitplay = false;
	
	// Bool that determines whether the avatar is active.
	//public bool Active = true;
	
	// Bool that has the characters (facing the player) actions become mirrored. Default false.
	private float Jumpsmooth = 5; 
	public bool MirroredMovement = false;
	
	// Bool that determines whether the avatar will move or not in space.
	//public bool MovesInSpace = true;
	
	// Bool that determines whether the avatar is allowed to jump -- vertical movement
	// can cause some models to behave strangely, so use at your own discretion.
	public bool VerticalMovement = false;
	
	// Rate at which avatar will move through the scene. The rate multiplies the movement speed (.001f, i.e dividing by 1000, unity's framerate).
	public float MoveRate = 1;
	
	// Slerp smooth factor
	public float SmoothFactor = 3.0f;
	
	//life count
	public static int life = 6;
	
	
	// Public variables that will get matched to bones. If empty, the kinect will simply not track it.
	// These bones can be set within the Unity interface.
	public Transform Hips;
	public Transform Spine;
	public Transform Neck;
	public Transform Head;
	
	//public Transform LeftShoulder;
	public Transform LeftUpperArm;
	public Transform LeftElbow; 
	public Transform LeftWrist;
	public Transform LeftHand;
	public Transform LeftFingers;
	
	//public Transform RightShoulder;
	public Transform RightUpperArm;
	public Transform RightElbow;
	public Transform RightWrist;
	public Transform RightHand;
	public Transform RightFingers;
	
	public Transform LeftThigh;
	public Transform LeftKnee;
	public Transform LeftFoot;
	public Transform LeftToes;
	
	public Transform RightThigh;
	public Transform RightKnee;
	public Transform RightFoot;
	public Transform RightToes;
	
	public Transform Root;
	
	// A required variable if you want to rotate the model in space.
	public GameObject offsetNode;
	
	// Variable to hold all them bones. It will initialize the same size as initialRotations.
	private Transform[] bones;
	
	// Rotations of the bones when the Kinect tracking starts.
    private Quaternion[] initialRotations;
	
	// Calibration Offset Variables for Character Position.
	private bool OffsetCalibrated = false;
	private float XOffset, YOffset, ZOffset;
	private Quaternion originalRotation;
	
	// GUI Text to display the gesture messages.
	private GameObject GestureInfo;
	// GUI Texture to display the hand cursor
	private GameObject HandCursor;
	// private bool to track if progress message has been displayed
	private bool progressDisplayed;
	
	//fatigue calculation value
	JumpFatigue JumpCal = new JumpFatigue();
	ArmFatigue LeftArmCal = new ArmFatigue();
	ArmFatigue RightArmCal = new ArmFatigue();
	public static bool LeftArmAllowed = true;
	public static bool RightArmAllowed = true;
	public static float CalTime = 0;
	public static int CountLeftArm = 0;
	public static int CountRightArm = 0;
	
	//new variables
	private Vector3 lastpos;
	public static float charZ=0; //character starting position
	public static float charX=0;
	public static float velocity=0f;
	private float FirstspinePOS;
	private float FirsthipCharPOS;
	private float FirstHeadPos;
	public static bool dBodyget=false;
	private float initialYpos;
	
	
	public static bool gamestart;
	public static bool newgame;
	
	private GameObject clickText;
	public static Vector3 screenPixelPos = Vector3.zero;
	private GameObject playtext;
	private GameObject play;
	
	public static float UpdateTimeRun = 0.1f, timeRun=0.0f,Tmax=4.0f;
	static public float factor=2;
	static public int stop=0;
	public static float Vmax=25f;
	public static float GameVelocity;
	public static float clickProgress = 0f;
	private GameObject robot;
	private GameObject mainguy;
	private GameObject mia;
	private GameObject returnButton;
	private GameObject ReturnText;
	private GameObject highscoreButton;
	private GameObject highscoreText;
	private GameObject loginButton;
	private GameObject loginText;
	private GameObject NewUserbutton;
	private GameObject NewUserText;
	private GameObject goButton;
	private GameObject goText;
	private GameObject ConfirmButton;
	private GameObject ConfirmText;
	private GameObject LogoutButton;
	private GameObject LogoutText;
	private GameObject removeButton;
	private GameObject removeText;
//	private GameObject fatigueButton;
//	private GameObject fatigueText;
	private GameObject EasyButton;
	private GameObject EasyText;
	private GameObject MediumButton;
	private GameObject MediumText;
	private GameObject HardButton;
	private GameObject HardText;
	private GameObject SettingIcon;
	private GameObject PlusIcon;
	private GameObject MinusIcon;
	private GameObject WrongUserButton;
	private GameObject WrongUserText;
	private GameObject User1Button;
	private GameObject User1Text;
	private GameObject User2Button;
	private GameObject User2Text;
	private GameObject User3Button;
	private GameObject User3Text;
	private GameObject ManualLoginButton;
	private GameObject ManualLoginText;
	private GameObject ManualGO;
	private GameObject ManualGOText;
	
	public float inXL;
	public float inYL;
	public float inZL;
	public float inXR;
	public float inYR;
	public float inZR;
	
	private GameObject back1;
	private GameObject back2;
	private GameObject back3;
	private GameObject next1;
	private GameObject next2;
	private GameObject next3;
	private GameObject backtext;
	private GameObject nexttext;
	
	private GameObject howtoplay;
	private GameObject howtext;
	
	public static bool LeftTurn = false;
	public static bool RightTurn = false;
	public static bool isAllow = true;
	public static bool firstTurn = false;
	
	public static bool rightthenleft = false;
	public static bool leftthenright  = false;
	
	private Quaternion left;
	private Quaternion right;
	
	private GameObject InnerCharacter;
	private GameObject character;
	public static float turnspeed;
	
	
	// if user is close to kinect. Use for face recognition
	public static bool UserNear = false; 
	private bool FaceRecognition = false;
	
	
	void Awake()
	{
		InnerCharacter = GameObject.FindGameObjectWithTag("InnerChar");
		character = GameObject.FindGameObjectWithTag("character");
		
		if(Application.loadedLevelName=="FaceLogin" || Application.loadedLevelName=="FaceRegister")
		{
			FaceRecognition=true;
			
		}	
	}
	
	public void Start()
    {	
				
		initialYpos = Root.parent.localPosition.y;
		GestureInfo = GameObject.Find("GestureInfo");
		HandCursor = GameObject.Find("HandCursor");
		clickText = GameObject.Find("ClickText");
		
		back1 = GameObject.Find("BackButton1");
		back2 = GameObject.Find("BackButton2");
		back3 = GameObject.Find("BackButton3");
		next1 = GameObject.Find("NextButton1");
		next2 = GameObject.Find("NextButton2");
		next3 = GameObject.Find("NextButton3");
		backtext = GameObject.Find("BackText");
	    nexttext = GameObject.Find("NextText");
		
		howtoplay = GameObject.Find("Howtoplay");
		howtext = GameObject.Find("howtext");
		
		
		play = GameObject.Find("playbutton");
		
		playtext = GameObject.Find("playtext");
		returnButton = GameObject.Find("ReturnButton");
		ReturnText = GameObject.Find ("returntext");
		
		highscoreButton = GameObject.Find("HighScoreButton");
	
		highscoreText = GameObject.Find("HighScoreText");
		
		loginButton = GameObject.Find("LogInButton");
		loginText = GameObject.Find("LogInText");
		
		NewUserbutton = GameObject.Find("Newuser");
		NewUserText = GameObject.Find("newuserText");
		
		goButton = GameObject.Find("GoButton");
		goText = GameObject.Find("GoText");
		
		ConfirmButton = GameObject.Find("Confirmbutton");
		ConfirmText = GameObject.Find("Confirmtext");
		
		LogoutButton = GameObject.Find("LogOutButton");
		LogoutText = GameObject.Find("LogOutText");
		
		removeButton = GameObject.Find("RemoveButton");
		removeText = GameObject.Find("removetext");
		
//		fatigueButton = GameObject.Find("FatigueButton");
//		fatigueText = GameObject.Find("fatiguetext");
		
		EasyButton = GameObject.Find("EasyButton");
		EasyText = GameObject.Find("EasyText");
		
		MediumButton = GameObject.Find("MediumButton");
		MediumText = GameObject.Find("MediumText");
		
		HardButton = GameObject.Find("HardButton");
		HardText = GameObject.Find("HardText");
		
		SettingIcon = GameObject.Find("Setting");
		PlusIcon = GameObject.Find("Plus");
		MinusIcon = GameObject.Find("Minus");
		
		WrongUserButton = GameObject.Find("WrongUserButton");
		WrongUserText = GameObject.Find("WrongUserText");
		
		User1Button = GameObject.Find("User1Button");
		User1Text = GameObject.Find("User1Text");

		User2Button = GameObject.Find("User2Button");
		User2Text = GameObject.Find("User2Text");
		
		User3Button = GameObject.Find("User3Button");
		User3Text = GameObject.Find("User3Text");
		
		ManualLoginButton = GameObject.Find("ManualLoginButton");
		ManualLoginText = GameObject.Find("ManualText");
		
		ManualGO = GameObject.Find ("ManualGo");
		ManualGOText = GameObject.Find ("ManualGoText");
		
		robot = GameObject.Find("robotclick");
		mainguy = GameObject.Find("mainguyclick");
		mia = GameObject.Find("miaclick");
		
		// Holds our bones for later.
		bones = new Transform[25];
		
		// Initial rotations of said bones.
		initialRotations = new Quaternion[bones.Length];
		
		// Map bones to the points the Kinect tracks.
		MapBones();
		
		// Get initial rotations to return to later.
		GetInitialRotations();
		
				
		// Set the model to the calibration pose.
        RotateToCalibrationPose(0, KinectManager.IsCalibrationNeeded());
		
		FirsthipCharPOS = Hips.transform.position.y;
				
		//load blacklist
		if(Application.loadedLevelName=="PlayGame")
		{
			ProjAvatarDB.GetBlackList();
		}
		
		
		//initialize variable
		
		dBodyget=false;
		
		charZ=0; //character starting position
		velocity=0.9f;
		UpdateTimeRun = 0.1f;
		timeRun=0.0f;
		stop=0;
		JumpCal.SetValue();
		LeftArmCal.SetValue();
		RightArmCal.SetValue();
		
		GameVelocity = Vmax;
				
    }
	
	// Update the avatar each frame.
    public void UpdateAvatar(uint UserID, bool IsNearMode)
    {	
		
		bool flipJoint = !MirroredMovement;
		
		// Update Head, Neck, Spine, and Hips normally.
		TransformBone(UserID, KinectWrapper.SkeletonJoint.HIPS, 1, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.SPINE, 2, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.NECK, 3, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.HEAD, 4, flipJoint);
		
		// Beyond this, switch the arms and legs.
		
		// Left Arm --> Right Arm
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_COLLAR, !MirroredMovement ? 5 : 11, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_SHOULDER, !MirroredMovement ? 6 : 12, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ELBOW, !MirroredMovement ? 7 : 13, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_WRIST, !MirroredMovement ? 8 : 14, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HAND, !MirroredMovement ? 9 : 15, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FINGERTIP, !MirroredMovement ? 10 : 16, flipJoint);
		
		// Right Arm --> Left Arm
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_COLLAR, !MirroredMovement ? 11 : 5, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_SHOULDER, !MirroredMovement ? 12 : 6, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ELBOW, !MirroredMovement ? 13 : 7, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_WRIST, !MirroredMovement ? 14 : 8, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HAND, !MirroredMovement ? 15 : 9, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FINGERTIP, !MirroredMovement ? 16 : 10, flipJoint);
		
		if(!IsNearMode)
		{
			// Left Leg --> Right Leg
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HIP, !MirroredMovement ? 17 : 21, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_KNEE, !MirroredMovement ? 18 : 22, flipJoint);
			//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ANKLE, !MirroredMovement ? 19 : 23, flipJoint);
			//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FOOT, !MirroredMovement ? 20 : 24, flipJoint);
			
			// Right Leg --> Left Leg
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HIP, !MirroredMovement ? 21 : 17, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_KNEE, !MirroredMovement ? 22 : 18, flipJoint);
			//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ANKLE, !MirroredMovement ? 23 : 19, flipJoint);
			//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FOOT, !MirroredMovement ? 24 : 20, flipJoint);	
		}
		
		
		//***Call Calculation Function (REBACal)***//
		if(REBACollider.REBACalAllowed)
		{
			REBACollider.REBACalAllowed = false;
			REBACollider.getREBAPos(KinectManager.Instance.GetJointPosition(UserID,3),KinectManager.Instance.GetJointPosition(UserID,2),KinectManager.Instance.GetJointPosition(UserID,1),KinectManager.Instance.GetJointPosition(UserID,13),KinectManager.Instance.GetJointPosition(UserID,14),KinectManager.Instance.GetJointPosition(UserID,12),KinectManager.Instance.GetJointPosition(UserID,5),KinectManager.Instance.GetJointPosition(UserID,4),KinectManager.Instance.GetJointPosition(UserID,6),KinectManager.Instance.GetJointPosition(UserID,7),KinectManager.Instance.GetJointPosition(UserID,17),KinectManager.Instance.GetJointPosition(UserID,18),KinectManager.Instance.GetJointPosition(UserID,16),KinectManager.Instance.GetJointPosition(UserID,9),KinectManager.Instance.GetJointPosition(UserID,8),KinectManager.Instance.GetJointPosition(UserID,10),KinectManager.Instance.GetJointPosition(UserID,11));
		}

		//Debug.Log ("RightArm: "+ RElbowAcc +"LeftArm: "+ LElbowAcc +"Torso: "+ JTorsoAcc);
		

		// If the game is started, move the avatar
		if (gamestart)
		{
			MoveAvatar(UserID);
		}
		
		if(FaceRecognition)
		{
			Vector3 userpos = KinectManager.Instance.GetUserPosition(UserID);
			//Debug.Log (userpos.z);
			if(userpos.z <= 2.0f)
				UserNear = true;
			else 
				UserNear = false;
		}
		
		
    }
	
	// Calibration pose is simply initial position with hands raised up. Rotation must be 0,0,0 to calibrate.
    public void RotateToCalibrationPose(uint userId, bool needCalibration)
    {	
		// Reset the rest of the model to the original position.
        RotateToInitialPosition();
		
		if(needCalibration)
		{
			if(offsetNode != null)
			{
				// Set the offset's rotation to 0.
				offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
			}
			
			// Right Elbow
			if(RightElbow != null)
	        	RightElbow.rotation = Quaternion.Euler(0, -90, 90) * 
					initialRotations[(int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW];
			
			// Left Elbow
			if(LeftElbow != null)
	        	LeftElbow.rotation = Quaternion.Euler(0, 90, -90) * 
					initialRotations[(int)KinectWrapper.SkeletonJoint.LEFT_ELBOW];

			if(offsetNode != null)
			{
				// Restore the offset's rotation
				if((LeftTurn && RightTurn)||(!LeftTurn && !RightTurn))
					offsetNode.transform.rotation = originalRotation;
				if(LeftTurn && !RightTurn)
				{
					character.transform.rotation = Quaternion.Euler(0, -90f, 0);
					InnerCharacter.transform.rotation = Quaternion.Euler(0, -90f, 0);
				}
				if(!LeftTurn && RightTurn)
				{
					character.transform.rotation = Quaternion.Euler(0, 90f, 0);
					InnerCharacter.transform.rotation = Quaternion.Euler(0, 90f, 0);
				}
			}
		}
		
		if(userId != 0)
		{
			// clear gesture info
			KinectManager.Instance.ClearGestures(userId);
			
			if(GestureInfo != null)
			{//GestureInfo.guiText.text = string.Empty;
				//clickText.guiText.text = string.Empty;
			}
		}
    }
	
	// Invoked on the successful calibration of a player.
	public void SuccessfulCalibration(uint userId)
	{
		if(Application.loadedLevelName=="PlayGame")
			{
				GameObject.FindGameObjectWithTag("RaiseText").renderer.enabled = false;
				KinectManager.pause = false;
				GameObject.FindGameObjectWithTag("PauseText").renderer.enabled = false;
				GameObject.FindGameObjectWithTag("PauseText2").renderer.enabled = false;
			
			}	
		// reset the models position
		if(offsetNode != null)
		{
			if((LeftTurn && RightTurn) || (!LeftTurn && RightTurn))
				offsetNode.transform.rotation = originalRotation;
			if(LeftTurn && !RightTurn)
			{
				character.transform.rotation = Quaternion.Euler(0, -90f, 0);
				InnerCharacter.transform.rotation = Quaternion.Euler(0, -90f, 0);
			}
			if(!LeftTurn && RightTurn)
			{
				character.transform.rotation = Quaternion.Euler(0, 90f, 0);
				InnerCharacter.transform.rotation = Quaternion.Euler(0, 90f, 0);
			}
		}
		
		// re-calibrate the position offset
		OffsetCalibrated = false;
		
		//if(GestureInfo != null)
		//	GestureInfo.guiText.text = "";
		//if(clickText != null)
		//clickText.guiText.text="";
	}
	
	// Invoked when a gesture is in progress 
	// The gesture must be added first, with KinectManager.Instance.DetectGesture()
	public void GestureInProgress(uint userId, KinectWrapper.Gestures gesture, float progress, 
		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		
		//Debug.Log( string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100)));
		//GestureInfo.guiText.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));
		if((gesture == KinectWrapper.Gestures.RightHandCursor || gesture == KinectWrapper.Gestures.LeftHandCursor) && progress > 0.5f)
		{
			if(HandCursor != null)
				HandCursor.transform.position = Vector3.Lerp(HandCursor.transform.position, screenPos, 4 * Time.deltaTime);
			
			screenPixelPos.x = (int)(HandCursor.transform.position.x * Camera.mainCamera.pixelWidth);
			screenPixelPos.y = (int)(HandCursor.transform.position.y * Camera.mainCamera.pixelHeight);
			Ray ray = Camera.mainCamera.ScreenPointToRay(screenPixelPos);
			
			// check if the button is hit
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit))
			{
				
					if(hit.collider.gameObject == play)
					{
						hitplay = true;
						if(playtext!=null)
							playtext.renderer.material.color = Color.yellow;
					}
					else if(hit.collider.gameObject == returnButton)
					{
						hitplay = true;
						if(ReturnText!=null)
							ReturnText.renderer.material.color = Color.yellow;
					}
					else if(hit.collider.gameObject == highscoreButton)
					{
						hitplay = true;
						if(highscoreText!=null)
						 highscoreText.renderer.material.color = Color.yellow;
					}
					
					else if(hit.collider.gameObject == loginButton)
					{
						hitplay = true;
						if(loginText!=null)
						 loginText.renderer.material.color = Color.yellow;
					}
					
					else if(hit.collider.gameObject == NewUserbutton)
					{
						hitplay = true;
						if(NewUserText!=null)
						 NewUserText.renderer.material.color = Color.yellow;
					}

					else if(hit.collider.gameObject == ConfirmButton)
					{
						hitplay = true;
						if(ConfirmText!=null)
						 ConfirmText.renderer.material.color = Color.yellow;
					}	

					else if(hit.collider.gameObject == LogoutButton)
					{
						hitplay = true;
						if(LogoutText!=null)
						 LogoutText.renderer.material.color = Color.yellow;
					}	

					else if(hit.collider.gameObject == removeButton)
					{
						hitplay = true;
						if(removeText!=null)
						 removeText.renderer.material.color = Color.yellow;
					}	

//										else if(hit.collider.gameObject == fatigueButton)
//										{
//											hitplay = true;
//											if(fatigueText!=null)
//											 fatigueText.renderer.material.color = Color.yellow;
//										}	

					else if(hit.collider.gameObject == EasyButton)
					{
						hitplay = true;
						if(EasyText!=null)
						 EasyText.renderer.material.color = Color.yellow;
					}	
					
					else if(hit.collider.gameObject == MediumButton)
					{
						hitplay = true;
						if(MediumText!=null)
						 MediumText.renderer.material.color = Color.yellow;
					}	
					
					else if(hit.collider.gameObject == HardButton)
					{
						hitplay = true;
						if(HardText!=null)
						 HardText.renderer.material.color = Color.yellow;
					}	
					
					else if(hit.collider.gameObject == WrongUserButton)
					{
						hitplay = true;
						if(WrongUserText!=null)
						 WrongUserText.renderer.material.color = Color.yellow;
					}		
					
					else if(hit.collider.gameObject == User1Button)
					{
						hitplay = true;
						if(User1Text!=null)
						 User1Text.renderer.material.color = Color.yellow;
					}	
					
					else if(hit.collider.gameObject == User2Button)
					{
						hitplay = true;
						if(User2Text!=null)
						 User2Text.renderer.material.color = Color.yellow;
					}
					
					else if(hit.collider.gameObject == User3Button)
					{
						hitplay = true;
						if(User3Text!=null)
						 User3Text.renderer.material.color = Color.yellow;
					}
					
					else if(hit.collider.gameObject == ManualLoginButton)
					{
						hitplay = true;
						if(ManualLoginText!=null)
						 ManualLoginText.renderer.material.color = Color.yellow;
					}

					else if(hit.collider.gameObject == ManualGO)
					{
						hitplay = true;
						if(ManualGOText!=null)
						 ManualGOText.renderer.material.color = Color.yellow;
					}

					else if(hit.collider.gameObject == SettingIcon)
					{
						hitplay = true;
					
					}	
					
					else if(hit.collider.gameObject == PlusIcon)
					{
						hitplay = true;
					
					}	

					else if(hit.collider.gameObject == MinusIcon)
					{
						hitplay = true;
					
					}	
					
					else if(hit.collider.gameObject == goButton)
					{
						hitplay = true;
						if(goText!=null)
						 goText.renderer.material.color = Color.yellow;
					}
					else if(hit.collider.gameObject == howtoplay)
					{
						hitplay = true;
						if(howtoplay!=null)
						 howtext.renderer.material.color = Color.yellow;
					}
					else if(hit.collider.gameObject == next1 || hit.collider.gameObject == next2 || hit.collider.gameObject == next3)
					{
						hitplay = true;
						if(nexttext!=null)
						 nexttext .renderer.material.color = Color.yellow;
					}
					else if(hit.collider.gameObject == back1 || hit.collider.gameObject == back2 || hit.collider.gameObject == back3)
					{
						hitplay = true;
						if(backtext!=null)
						backtext.renderer.material.color = Color.yellow;
					}
					else if (hit.collider.gameObject == robot || hit.collider.gameObject == mainguy || hit.collider.gameObject == mia)
					{
						hitplay = true;
					}
					else
					{	
						hitplay = false;
						clickProgress = 0;
						if(playtext!=null)
							playtext.renderer.material.color = Color.white;

						if(ReturnText!=null)
							ReturnText.renderer.material.color = Color.white;

						if(highscoreText!=null)
							highscoreText.renderer.material.color = Color.white;
						
						if(loginText!=null)
							loginText.renderer.material.color = Color.white;
						
						if(NewUserText!=null)
							NewUserText.renderer.material.color = Color.white;

						if(backtext!=null)
							backtext.renderer.material.color = Color.white;

						if(nexttext!=null)
							nexttext.renderer.material.color = Color.white;

						if(howtext!=null)
							howtext.renderer.material.color = Color.white;

						if(goText!=null)
							goText.renderer.material.color = Color.white;

						if(ConfirmText!=null)
							ConfirmText.renderer.material.color = Color.white;
						
						if(LogoutText!=null)
							LogoutText.renderer.material.color = Color.white;

						if(removeText!=null)
							removeText.renderer.material.color = Color.white;

						if(EasyText!=null)
							EasyText.renderer.material.color = Color.white;

						if(MediumText!=null)
							MediumText.renderer.material.color = Color.white;
						
						if(HardText!=null)
							HardText.renderer.material.color = Color.white;
						
						if(WrongUserText!=null)
							WrongUserText.renderer.material.color = Color.white;
						
						if(User1Text!=null)
							User1Text.renderer.material.color = Color.white;
	
						if(User2Text!=null)
							User2Text.renderer.material.color = Color.white;
						
						if(User3Text!=null)
							User3Text.renderer.material.color = Color.white;

						if(ManualLoginText!=null)
							ManualLoginText.renderer.material.color = Color.white;
						
						if(ManualGOText!=null)
							ManualGOText.renderer.material.color = Color.white;

//											if(fatigueText!=null)
//												fatigueText.renderer.material.color = Color.white;
						
					}
			
			}
							
			
			
			
//			string sGestureText = string.Format("{0} - ({1:F1}, {2:F1} {3:F1})", gesture, screenPos.x, screenPos.y, progress * 100);
//			Debug.Log(sGestureText);
		}
		else if(gesture == KinectWrapper.Gestures.Click && progress > 0.3f)
		{	
			
			clickProgress = progress;
			//string sGestureText =string.Format("{0}  {1:F1}%", " ", (progress * 100));
			
			
			
			//clickText.transform.position = HandCursor.transform.position;
			//clickText.guiText.text=sGestureText;
			
			
			
			if(GestureInfo != null)
				//GestureInfo.guiText.text = sGestureText;
			progressDisplayed = true;
		}		
		else if(gesture == KinectWrapper.Gestures.Click && progress < 0.3f)
		{
			clickProgress = progress;		
		}
		
		/*else if((gesture == KinectWrapper.Gestures.ZoomOut || gesture == KinectWrapper.Gestures.ZoomIn) && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
			if(GestureInfo != null)
				GestureInfo.guiText.text = sGestureText;
			progressDisplayed = true;
		}
		else if(gesture == KinectWrapper.Gestures.Wheel && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
			if(GestureInfo != null)
				GestureInfo.guiText.text = sGestureText;
			progressDisplayed = true;
		}*/
	}
	
	// Invoked when a gesture is complete.
	// Return true, if the gesture must be detected again, false otherwise
	public bool GestureComplete(uint userId, KinectWrapper.Gestures gesture,
		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		if(gesture == KinectWrapper.Gestures.Click)
			sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
		
		if(GestureInfo != null)
			//GestureInfo.guiText.text = sGestureText;
		progressDisplayed = false;
		
		
		return true;
	}
	
	// Invoked when a gesture is cancelled.
	public bool GestureCancelled(uint userId, KinectWrapper.Gestures gesture, 
		KinectWrapper.SkeletonJoint joint)
	{
		
		//clickText.guiText.text=String.Empty;
		
		if(progressDisplayed)
		{
			// clear the progress info
			if(GestureInfo != null)
				GestureInfo.guiText.text = String.Empty;
			progressDisplayed = false;
		}
		
		
		
		return true;
	}

	
	// Apply the rotations tracked by kinect to the joints.
    void TransformBone(uint userId, KinectWrapper.SkeletonJoint joint, int boneIndex, bool flip)
    {
		Transform boneTransform = bones[boneIndex];
		if(boneTransform == null)
			return;
		
		// Grab the bone we're moving.
		int iJoint = (int)joint;
		if(iJoint < 0)
			return;
		
		// Get Kinect joint orientation
		Quaternion jointRotation = KinectManager.Instance.GetJointOrientation(userId, iJoint, flip);
		if(jointRotation == Quaternion.identity)
			return;
		
		// Apply the new rotation.
        Quaternion newRotation = jointRotation * initialRotations[boneIndex];
		
		//If an offset node is specified, combine the transform with its
		//orientation to essentially make the skeleton relative to the node
		if (offsetNode != null)
		{
			// Grab the total rotation by adding the Euler and offset's Euler.
			Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
			// Grab our new rotation.
			newRotation = Quaternion.Euler(totalRotation);
		}
		
		// Smoothly transition to our new rotation.
        boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, Time.deltaTime * SmoothFactor);
	}
	
	// Moves the avatar in 3D space - pulls the tracked position of the spine and applies it to root.
	// Only pulls positional, not rotational.
	void MoveAvatar(uint UserID)
	{
		if(Root == null || Root.parent == null)
		{
			Debug.Log("error");
			return;
		}
		if(!KinectManager.Instance.IsJointTracked(UserID, (int)KinectWrapper.SkeletonJoint.HIPS))
			return;
		
        // Get the position of the body and store it.
		Vector3 trans = KinectManager.Instance.GetUserPosition(UserID);
		Vector3 spinepos = KinectManager.Instance.GetJointPosition(UserID,0);
		Vector3 hippos = KinectManager.Instance.GetJointPosition(UserID,1);
		Vector3 headpos = KinectManager.Instance.GetJointPosition(UserID,3);
		Vector3 LeftWristPos = KinectManager.Instance.GetJointPosition(UserID,6);
		Vector3 RightWristPos = KinectManager.Instance.GetJointPosition(UserID,10);
		
		CalTime += Time.deltaTime;
//		Debug.Log("spine"+hippos.y);
//		Debug.Log("hip"+spinepos.y);
//		Debug.Log("head"+headpos.y);
		
		//Debug.Log("Spine: " + hippos + " RIghtArm: " + RightWristPos);
		
		//save value only the first time
		if(!dBodyget)
		{	
			FirstspinePOS = spinepos.y+0.05f;
			FirstHeadPos = headpos.y;
			//Debug.Log ("spinePOSition = " + FirstspinePOS);
			dBodyget=true;
		}
//		Debug.Log("firsthead"+FirstHeadPos);
//		
//		Debug.Log("head"+headpos.y);

	
		// If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
//		if (!OffsetCalibrated)
//		{
//			OffsetCalibrated = true;
//			
//			XOffset = !MirroredMovement ? trans.x * MoveRate : -trans.x * MoveRate;
//			YOffset = trans.y * MoveRate;
//			ZOffset = -trans.z * MoveRate;
//		}
	
		float xPos;
		float yPos;
		float zPos;
		
		MoveRate=1.5f;
		// If movement is mirrored, reverse it.
		
		
			/*isAllow = false;
			if(LeftTurn&&RightTurn){
				LeftTurn = false;
				RightTurn = false;
			}
			isAllow = true;*/
			if(!MirroredMovement)
				charX = trans.x * MoveRate;//- XOffset;
			//xPos = trans.x * MoveRate;//- XOffset;
			else
				charX = -trans.x * MoveRate;//- XOffset;
			//xPos = -trans.x * MoveRate;//- XOffset;
		
		//fatigue
		if(LeftWristPos.y > hippos.y && !LeftArmCal.isFall)
		{
			LeftArmCal.ArmFatigueCal(LeftWristPos);
		}
		else if(LeftWristPos.y < hippos.y)
		{
			LeftArmCal.isFall = false;
			CountLeftArm = LeftArmCal.getCountArm();
		}
		LeftArmAllowed = LeftArmCal.ResultBanned;
		
		if(RightWristPos.y > hippos.y && !RightArmCal.isFall)
		{
			RightArmCal.ArmFatigueCal(RightWristPos);
		}
		else if(RightWristPos.y < hippos.y)
		{
			RightArmCal.isFall = false;
			CountRightArm = RightArmCal.getCountArm();
		}
		RightArmAllowed = RightArmCal.ResultBanned;
		
		
	
		//yPos = trans.y * MoveRate - YOffset;
		//zPos = -trans.z * MoveRate - ZOffset;
		
		if (spinepos.y<FirstspinePOS-0.17)//crouch
		{	
			//yPos = (spinepos.y - FirstspinePOS+0.1f);
			yPos = initialYpos+(spinepos.y - FirstspinePOS+0.1f);
						
		}
		
	
		//else if (spinepos.y>FirstspinePOS+0.15 )//jump up
		else if(headpos.y>FirstHeadPos+0.05)
		{
			//yPos = (spinepos.y - FirstspinePOS)+0.8f;
		//	yPos = initialYpos+(spinepos.y - FirstspinePOS)+0.8f;
			if(headpos.y -( FirstHeadPos+0.05)<0.05)
			{
				yPos = initialYpos+(headpos.y - FirstHeadPos)+0.1f;
				if(JumpFatigue.FirstHeight)
					JumpCal.JumpFatigueAnalysis();
				
			}	
			else
			{
				yPos = initialYpos+(headpos.y - FirstHeadPos)+0.6f;
				JumpCal.MaxHeightCalculation(headpos,FirstHeadPos);
			}	
			
		}
		else
			yPos=initialYpos;
		//yPos=0;
		
			//zPos = trans.z;
		
		
		timeRun+=UpdateTimeRun;
		velocity = Vmax*factor / ( 1 + Mathf.Exp( ( -timeRun+5*(Tmax/3))/(Tmax/3)) ) * (1-stop);
		
		
		
//		Debug.Log ("x "+charX+"y " + yPos +"z "+ charZ);
	//	charZ += velocity;
		
		if(!BoxCollision.bounce){
		character.transform.Translate(0,0,velocity*Time.deltaTime);
			
		}
		else if (BoxCollision.bounce)
		{
			character.transform.Translate(0,0,-25f*Time.deltaTime);
			
		}
		
		if (rightthenleft)
		{
			//Debug.Log("right then left");
			left = Quaternion.Euler(0,0,0);
			character.transform.rotation = Quaternion.Slerp (character.transform.rotation, left, Time.deltaTime*turnspeed);
			Vector3 leftposition = new Vector3 (TurnLeftAfterRight.pos , character.transform.position.y, character.transform.position.z);
			character.transform.position = Vector3.Lerp(character.transform.position, leftposition, Time.deltaTime);	
			//character.transform.rotation = Quaternion.Euler(0,0,0);
		}
		else if (leftthenright)
		{
			//Debug.Log("left then right");
			right = Quaternion.Euler(0,0,0);
			character.transform.rotation = Quaternion.Slerp (character.transform.rotation, right, Time.deltaTime*turnspeed);
			Vector3 rightposition = new Vector3 (TurnRightAfterLeft.pos, character.transform.position.y, character.transform.position.z);
			character.transform.position = Vector3.Lerp(character.transform.position, rightposition, Time.deltaTime);	
			//character.transform.rotation = Quaternion.Euler(0,0,0);
		}
		
		else if(LeftTurn&&!RightTurn)
		{
			left = Quaternion.Euler(0,-90f,0);
			character.transform.rotation = Quaternion.Lerp (character.transform.rotation, left, Time.deltaTime*turnspeed);
			Vector3 leftposition = new Vector3 (character.transform.position.x, character.transform.localPosition.y, TurnLeft.pos);
			character.transform.position = Vector3.Lerp(character.transform.position, leftposition, Time.deltaTime);
			//character.transform.rotation = Quaternion.Euler(0,-90f,0);
			
		}
		else if(!LeftTurn&&RightTurn)
		{
			right = Quaternion.Euler(0,90f,0);
			character.transform.rotation = Quaternion.Slerp (character.transform.rotation, right, Time.deltaTime*turnspeed);
			Vector3 rightposition = new Vector3 (character.transform.position.x, character.transform.position.y, TurnRight.pos);
			character.transform.position = Vector3.Lerp(character.transform.position, rightposition, Time.deltaTime);
			//character.transform.rotation = Quaternion.Euler(0,90f,0);
		}
		
				
		// If we are tracking vertical movement, update the y. Otherwise leave it alone.
		Vector3 targetPos = new Vector3(charX, yPos , 0f);
		
		//transform.localPosition = (transform.rotation * targetPos);
	
		Root.parent.localPosition = Vector3.Lerp(Root.parent.localPosition, targetPos, Jumpsmooth * Time.deltaTime);
		
	
		if((LeftTurn && RightTurn)||(!LeftTurn && !RightTurn))
						lastpos=Root.parent.localPosition;
		if((!LeftTurn && RightTurn) || (LeftTurn && !RightTurn))
						lastpos=character.transform.position;
		
		}
	
	// If the bones to be mapped have been declared, map that bone to the model.
	void MapBones()
	{
		// If they're not empty, pull in the values from Unity and assign them to the array.
		if(Hips != null)
			bones[1] = Hips;
		if(Spine != null)
			bones[2] = Spine;
		if(Neck != null)
			bones[3] = Neck;
		if(Head != null)
			bones[4] = Head;
		
//		if(LeftShoulder != null)
//			bones[5] = LeftShoulder;
		if(LeftUpperArm != null)
			bones[6] = LeftUpperArm;
		if(LeftElbow != null)
			bones[7] = LeftElbow;
		if(LeftWrist != null)
			bones[8] = LeftWrist;
		if(LeftHand != null)
			bones[9] = LeftHand;
		if(LeftFingers != null)
			bones[10] = LeftFingers;
		
//		if(RightShoulder != null)
//			bones[11] = RightShoulder;
		if(RightUpperArm != null)
			bones[12] = RightUpperArm;
		if(RightElbow != null)
			bones[13] = RightElbow;
		if(RightWrist != null)
			bones[14] = RightWrist;
		if(RightHand != null)
			bones[15] = RightHand;
		if(RightFingers != null)
			bones[16] = RightFingers;
		
		if(LeftThigh != null)
			bones[17] = LeftThigh;
		if(LeftKnee != null)
			bones[18] = LeftKnee;
		if(LeftFoot != null)
			bones[19] = LeftFoot;
		if(LeftToes != null)
			bones[20] = LeftToes;
		
		if(RightThigh != null)
			bones[21] = RightThigh;
		if(RightKnee != null)
			bones[22] = RightKnee;
		if(RightFoot != null)
			bones[23] = RightFoot;
		if(RightToes!= null)
			bones[24] = RightToes;
	}
	
	// Capture the initial rotations of the model.
	void GetInitialRotations()
	{
		if(offsetNode != null)
		{
			// Store the original offset's rotation.
			originalRotation = offsetNode.transform.rotation;
			// Set the offset's rotation to 0.
			offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		
		for (int i = 0; i < bones.Length; i++)
		{
			
			
			if (bones[i] != null)
			{	
				
				initialRotations[i] = bones[i].rotation;
			}
		}

		if(offsetNode != null)
		{
			// Restore the offset's rotation
			offsetNode.transform.rotation = originalRotation;
		}
	}

	// Set bones to initial position.
    public void RotateToInitialPosition()
    {	
		if(bones == null)
			return;
		
		if(offsetNode != null)
		{
			// Set the offset's rotation to 0.
			offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		
		// For each bone that was defined, reset to initial position.
		for (int i = 0; i < bones.Length; i++)
		{
			if (bones[i] != null)
			{
				bones[i].rotation = initialRotations[i];
			}
		}

		if(Root != null && Root.parent != null)
		{
			//Root.parent.localPosition = Vector3.zero;
			//Root.parent.localPosition = lastpos;
			if(Application.loadedLevelName == "PlayGame"){
				if((LeftTurn && RightTurn)||(!LeftTurn && !RightTurn))
					Root.parent.localPosition = Vector3.Lerp(Root.parent.localPosition, lastpos, 3 * Time.deltaTime);
				if(LeftTurn && !RightTurn)
				{
					//Debug.Log(charX);
					lastpos =new Vector3(lastpos.x,lastpos.y,lastpos.z+charX);
					//character.transform.position = lastpos;
					character.transform.position = Vector3.Lerp(character.transform.position, lastpos, 3 * Time.deltaTime);
					//Debug.Log(character.transform.position.z);
				}
				if(!LeftTurn && RightTurn)
				{
					//Debug.Log(charX);
					lastpos =new Vector3(lastpos.x,lastpos.y,lastpos.z-charX);
					//character.transform.position = lastpos;
					character.transform.position = Vector3.Lerp(character.transform.position, lastpos, 3 * Time.deltaTime);
					//Debug.Log(character.transform.position.z);
				}
			//Debug.Log("Root Postion: X " + Root.parent.localPosition.x + "Y :" + Root.parent.localPosition.y + "Z: " + Root.parent.localPosition.z);
			//Debug.Log("LastPos: X " + lastpos.x + "Y :" + lastpos.y + "Z: " + lastpos.z);
			//Debug.Log("Character Transform: X " + character.transform.position.x + "Y: " + character.transform.position.y + "Z: " + character.transform.position.z);
			
			}
		}

		if(offsetNode != null)
		{
			// Restore the offset's rotation
			if((LeftTurn && RightTurn)||(!LeftTurn && !RightTurn))
				offsetNode.transform.rotation = originalRotation;
			if(LeftTurn && !RightTurn)
			{
				character.transform.rotation = Quaternion.Euler(0, -90f, 0);
				InnerCharacter.transform.rotation = Quaternion.Euler(0, -90f, 0);
			}
			if(!LeftTurn && RightTurn)
			{
				character.transform.rotation = Quaternion.Euler(0, 90f, 0);
				InnerCharacter.transform.rotation = Quaternion.Euler(0, 90f, 0);
			}
		}
		setinitialrotation();
    }
	
	//new function to return to the last position when user is lost
	public void RotateToLastPosition()
	{
		//Vector3 trans = KinectManager.Instance.GetUserPosition();
		Root.parent.localPosition = lastpos;
	}
	
	public void setinitialrotation()
	{
		LeftUpperArm.localRotation = Quaternion.Euler(inXL,inYL,inZL);
		RightUpperArm.localRotation = Quaternion.Euler(inXR,inYR,inZR);
	}
	
		
}

