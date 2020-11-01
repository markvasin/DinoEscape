using UnityEngine;
using System.Collections;

public class SmoothFollow2 : MonoBehaviour
{
	
	public Transform followTransform;
	public Vector3 offset = new Vector3 (0f, 2.5f, -5f);
	public float moveSpeed = 1;
	public float turnSpeed = 1;
	Vector3 goalPos;
	public float x = 0.0f;
	public Transform character;
	

	//Quaternion goalRot;

    

	// Use this for initialization

	void Start ()
	{

		if (!followTransform)
		{
			followTransform = GameObject.FindGameObjectWithTag ("target").transform;
			character = GameObject.FindGameObjectWithTag("character").transform;
		}
		if (!followTransform)
			this.enabled = false;
	}

	void LateUpdate ()
	{
		goalPos = character.position + character.TransformDirection (offset);
		goalPos.y = 3f;
		if (!AvatarController.LeftTurn && !AvatarController.RightTurn) {
		
			//if (turns) 
			{
			//transform.position = Vector3.Lerp (transform.position, goalPos, Time.deltaTime * moveSpeed);
			//goalPos.x = x;
			//goalPos.y = 3f;
			transform.position = goalPos;
			//Quaternion temp = Quaternion.Euler(3.520004f,followTransform.transform.rotation.y,followTransform.transform.rotation.z);
			//transform.rotation = Quaternion.Lerp (transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
			//followTransform.rotation = Quaternion.Lerp (followTransform.rotation, character.rotation,1);
			}
			//if (!turns) 
			{
			}
				
		} else if (AvatarController.leftthenright) {
			
			//x = TurnRightAfterLeft.pos;
			//goalPos.x = x;
			transform.position = goalPos;
			//Quaternion temp = Quaternion.Euler(3.520004f,followTransform.transform.rotation.y,followTransform.transform.rotation.z);
			//transform.rotation = Quaternion.Lerp (transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
		} else if (AvatarController.rightthenleft) {
			
			//x = TurnLeftAfterRight.pos;
			//goalPos.x = x;
			transform.position = goalPos;
			//Quaternion temp = Quaternion.Euler(3.520004f,followTransform.transform.rotation.y,followTransform.transform.rotation.z);
			//transform.rotation = Quaternion.Lerp (transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
		} else if (AvatarController.LeftTurn && !AvatarController.RightTurn) {
			
			//goalPos.z = TurnLeft.pos;
			transform.position = goalPos;
			//Quaternion temp = Quaternion.Euler(3.520004f,followTransform.transform.rotation.y,followTransform.transform.rotation.z);
			//transform.rotation = Quaternion.Lerp (transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
		} else if (!AvatarController.LeftTurn && AvatarController.RightTurn) {
			//goalPos.z = TurnRight.pos;
			transform.position = goalPos;
			//Quaternion temp = Quaternion.Euler(3.520004f,followTransform.transform.rotation.y,followTransform.transform.rotation.z);
			//transform.rotation = Quaternion.Lerp (transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
		}
		
		transform.LookAt(followTransform);		

	}
}
