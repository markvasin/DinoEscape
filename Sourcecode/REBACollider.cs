using UnityEngine;
using System.Collections;

public class REBACollider : MonoBehaviour {
	
	
	REBACal REBARight = new REBACal();
	REBACal REBALeft = new REBACal();
	
	private static Vector3 Head;
	private static Vector3 Neck;
	private static Vector3 Spine;
	
	private static Vector3 LeftKnee;
	private static Vector3 LeftFoot;
	private static Vector3 LeftThigh;
	private static Vector3 LeftElbow;
	private static Vector3 LeftUpperArm;
	private static Vector3 LeftHand;
	private static Vector3 LeftFingers;
	
	private static Vector3 RightKnee;
	private static Vector3 RightFoot;
	private static Vector3 RightThigh;
	private static Vector3 RightElbow;
	private static Vector3 RightUpperArm;
	private static Vector3 RightHand;
	private static Vector3 RightFingers;
	private bool Enter = false;
	private bool ExitREBA = false;
	public static bool REBACalAllowed = false;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public static void getREBAPos(Vector3 JointHead, Vector3 JointNeck, Vector3 JointTorso, Vector3 JointLKnee, Vector3 JointLAnkle, Vector3 JointLHip,Vector3 JointLElbow, Vector3 JointLShoulder,Vector3 JointLHand, Vector3 JointLFinger, Vector3 JointRKnee, Vector3 JointRAnkle, Vector3 JointRHip,Vector3 JointRElbow, Vector3 JointRShoulder,Vector3 JointRHand, Vector3 JointRFinger){
	
		Head = JointHead;
		Neck = JointNeck;
		Spine = JointTorso;
		
		LeftKnee = JointLKnee;
		LeftFoot = JointLAnkle;
		LeftThigh = JointLHip;
		LeftElbow = JointLElbow;
		LeftUpperArm = JointLShoulder;
		LeftHand = JointLHand;
		LeftFingers = JointLFinger;
		
		RightKnee = JointRKnee;
		RightFoot = JointRAnkle;
		RightThigh = JointRHip;
		RightElbow = JointRElbow;
		RightUpperArm = JointRShoulder;
		RightHand = JointRHand;
		RightFingers = JointRFinger;
	
	}
	
	void OnTriggerEnter ()
	{
		if(!Enter)
		{
			Enter = true;
			REBACalAllowed = true;
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		//Call Calculation Function (REBACal)
		
		
	}
	
	void OnTriggerExit(Collider other)//after pass this box half of the time to max acceleration 
	{
		if(!ExitREBA)
		{
			ExitREBA = true;
			REBALeft.CalculationREBA(Head, Neck, Spine, LeftKnee, LeftFoot, LeftThigh, LeftElbow, LeftUpperArm, LeftHand, LeftFingers);
			REBARight.CalculationREBA(Head, Neck, Spine, RightKnee, RightFoot, RightThigh, RightElbow, RightUpperArm, RightHand, RightFingers);
		}
	}
	
	
	
}
