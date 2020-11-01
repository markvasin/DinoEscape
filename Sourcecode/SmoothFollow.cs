using UnityEngine;
using System.Collections;
 
// Place the script in the Camera-Control group in the component menu
[AddComponentMenu("Camera-Control/Smooth Follow CSharp")]

	
	

 
public class SmoothFollow : MonoBehaviour
{
	/*
    This camera smoothes out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.
 
    There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
 
    For every of those smoothed values we calculate the wanted value and the current value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position.
    */
 
	// The target we are following
	private Transform target;
	public Transform[] targetChar = new Transform [3];
	public static int targetNo = 1;
	// The distance in the x-z plane to the target
	public float x = 0.0f;
	// the height we want the camera to be above the target
	public float y = 0.0f;
	// How much we 
	public float z = 0.0f;
	public Quaternion left;
	public Quaternion right;
	private float smooth = 1000f;
	private GameObject character;
	private Vector3 temp;
	public static bool turns = false;
	int lefttime = 0;

	void Start ()
	{
		
		target = targetChar [targetNo];
		character = GameObject.FindGameObjectWithTag ("character");
	}
	
	void Update ()
	{
//		Debug.Log("camerax: " + transform.position);
//		Debug.Log("targetx: " + target.position);
//		Vector3 diff = target.position - transform.position;
//		Debug.Log("vmax: " + AvatarController.Vmax);
		
	
		
	}
	
	void  LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target) {//Debug.Log("bad");
			return;
		}
		// Calculate the current rotation angles
//	       float wantedRotationAngle = target.eulerAngles.y;
//	       float wantedHeight = target.position.y + height;
//	       float currentRotationAngle = transform.eulerAngles.y;
//	       float currentHeight = transform.position.y;
//	 
//	       // Damp the rotation around the y-axis
//	       currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
//	 
//	       // Damp the height
//	       currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
//	 
//	       // Convert the angle into a rotation
//	       Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
//	 
//	       // Set the position of the camera on the x-z plane to:
//	       // distance meters behind the target
//	       transform.position = target.position;
//	       transform.position -= currentRotation * Vector3.forward * distance;
//	 
//	       // Set the height of the camera
//	       transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
//	 
//	       // Always look at the target
//	       transform.LookAt (target);
		
		//Debug.Log (t );
		//float ztarget=target.position.z;
			
		
		
		if (!AvatarController.LeftTurn && !AvatarController.RightTurn) {
			
			transform.position = new Vector3 (x, 3, target.position.z - z);
			
		} else if (AvatarController.leftthenright) {
			
			
			Vector3 rightpos = new Vector3 (TurnRightAfterLeft.pos, 3, target.position.z - 8.02f);

			
			if (turns) {
				right = Quaternion.Euler (3.520004f, 3f, 0);
				transform.rotation = Quaternion.Lerp (transform.rotation, right, Time.deltaTime * 5f);
				transform.position = Vector3.Lerp (transform.position, rightpos, Time.deltaTime * 5f);
				temp = transform.position;
				temp.z = target.position.z - 8.02f;
				transform.position = temp;
				x = TurnRightAfterLeft.pos;
			}
			if (!turns) {
				transform.position = rightpos;
				x = TurnRightAfterLeft.pos;
			}			
//			if (turns){
//			right = Quaternion.Euler(3.520004f,0f,0);
//			transform.rotation = Quaternion.Lerp(transform.rotation,right,Time.deltaTime*6f);
//			Vector3 rightpos = new Vector3(target.position.x, 3, target.position.z-6.45f);
//			transform.position = Vector3.Lerp(transform.position,rightpos,Time.deltaTime*8f);
//			}
//			else if (!turns){
//			//right = Quaternion.Euler(3.520004f,0f,0);
//			//transform.rotation = Quaternion.Euler(3.520004f,0f,0);
//			transform.position=new Vector3(TurnRightAfterLeft.pos, 3, target.position.z-z);
//			x = TurnRightAfterLeft.pos;
//			}
		} else if (AvatarController.rightthenleft) {
			Vector3 leftpos = new Vector3 (TurnLeftAfterRight.pos, 3, target.position.z - 8.02f);
			
			if (turns) {
				left = Quaternion.Euler (3.520004f, 3f, 0);
				transform.rotation = Quaternion.Lerp (transform.rotation, left, Time.deltaTime * 5f);
				transform.position = Vector3.Lerp (transform.position, leftpos, Time.deltaTime * 5f);
				temp = transform.position;
				temp.z = target.position.z - 8.02f;
				transform.position = temp;
			} 
			if (!turns) {
				transform.position = leftpos;
				x = TurnLeftAfterRight.pos;
			}
//			if (turns){
//			left = Quaternion.Euler(3.520004f,0f,0);
//			transform.rotation = Quaternion.Lerp(transform.rotation,left,Time.deltaTime*6f);
//			Vector3 leftpos = new Vector3(target.position.x, 3, target.position.z-6.45f);
//			transform.position = Vector3.Lerp(transform.position,leftpos,Time.deltaTime*8f);
//			}
//			else if (!turns){
//			//left = Quaternion.Euler(3.520004f,0f,0);
//			//transform.rotation = Quaternion.Lerp(transform.rotation,left,Time.deltaTime*5f);
//			transform.position=new Vector3(TurnLeftAfterRight.pos, 3, target.position.z-z);
//			x = TurnLeftAfterRight.pos;
//			}
//			
		} else if (AvatarController.LeftTurn && !AvatarController.RightTurn) {
			Vector3 leftpos = new Vector3 (target.position.x + 8.02f, 3, TurnLeft.pos);
			
			if (turns) {
				left = Quaternion.Euler (3.520004f, -90f, 0);
				transform.rotation = Quaternion.Lerp (transform.rotation, left, Time.deltaTime * 5f);
				transform.position = Vector3.Lerp (transform.position, leftpos, Time.deltaTime*5f);
				temp = transform.position;
				temp.x = target.position.x + 8.02f;
				transform.position = temp;
			}
			if (!turns) {
				transform.position = leftpos;
			}
//			if (turns){
//			
//			Debug.Log("delay");		
//			}
//			else if (!turns){
//			//left = Quaternion.Euler(3.520004f,-90f,0);
//			//transform.rotation = Quaternion.Euler(3.520004f,-90f,0); 
//			//transform.rotation = Quaternion.Lerp(transform.rotation,left,Time.deltaTime*5f);
//			//transform.position = new Vector3(target.position.x+z, 3,  TurnLeft.pos );
//			//Vector3 leftpos = new Vector3(target.position.x+8.02f, 3, TurnLeft.pos);
//			//transform.position = Vector3.Lerp(transform.position,leftpos,1);
//			Debug.Log("nodelay");
//			}				
		} else if (!AvatarController.LeftTurn && AvatarController.RightTurn) {
			Vector3 rightpos = new Vector3 (target.position.x - 8.02f, 3, TurnRight.pos);
			
			if (turns) {
				right = Quaternion.Euler (3.520004f, 90f, 0);
				transform.rotation = Quaternion.Lerp (transform.rotation, right, Time.deltaTime * 5f);
				transform.position = Vector3.Lerp (transform.position, rightpos, Time.deltaTime * 5f);
				temp = transform.position;
				temp.x = target.position.x - 8.02f;
				transform.position = temp;
			}
			if (!turns) {
				transform.position = rightpos;
			}
			
//			if (turns){			
//			right = Quaternion.Euler(3.520004f,90f,0);
//			transform.rotation = Quaternion.Lerp(transform.rotation,right,Time.deltaTime*6f);
//			Vector3 rightpos = new Vector3(target.position.x-4.65f, 3, target.position.z);
//			transform.position = Vector3.Lerp(transform.position,rightpos,Time.deltaTime*8f);
//			
//			}		
//			else if (!turns){
//			//right = Quaternion.Euler(3.520004f,90f,0);
//			//transform.rotation = Quaternion.Euler(3.520004f,90f,0);
//			transform.position=new Vector3(target.position.x-z, 3, TurnRight.pos);
//						
//			}		
		}
		
		
		
		
		//this.transform.position.z=target.position.z-z;
		//this.transform.position.x=x;
		//this.transform.position.y=3;
		//transform.rotation.x=30;
	}
}