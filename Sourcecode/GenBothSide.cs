using UnityEngine;
using System.Collections;

public class GenBothSide : MonoBehaviour {

	public GameObject[] LRMap = new GameObject [5];
	
	float MapScale=249.8f;
	float HalfMap = 125;
	float Number=9.0f;
	
	float MapPositionX = 0;
	float MapPositionY = 0;
	float MapPositionZ = 0;

	int random = 0;
	bool Enter = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter () {
		
		if(!Enter)
		{
			Enter=true;
			AvatarController.LeftTurn = false;
			AvatarController.RightTurn = false;
			AvatarController.leftthenright = false;
			AvatarController.rightthenleft = false;
			
			GenMap.Turn = false;
			GenObjScript.CountSide = 8;
			
			Quaternion rot   = Quaternion.identity;
			MapPositionZ = transform.parent.position.z + 125;
			
			//Gen three maps for each side
			for(int i = 1; i < 4; i++)
			{
					
					MapPositionX = transform.parent.position.x + 125 - (250*i);
					GameObject LeftMap = (GameObject)Instantiate(LRMap[i-1],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
					LeftMap.tag = "LeftT";
				
					random = Random.Range(5,7);
					MapPositionX = transform.parent.position.x + 125 + (250*i);
					GameObject RightMap = (GameObject)Instantiate(LRMap[random],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);
					RightMap.tag = "RightT";
			}
			
			//gen turning map for the fouth map for each side
			LRMap[3].tag = "LeftT";
			LRMap[4].tag = "RightT";
				
			MapPositionX = transform.parent.position.x + 125 -(250*4);
			Instantiate(LRMap[3],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);
			
			MapPositionX = transform.parent.position.x + 125 +(250*4);
			Instantiate(LRMap[4],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);
				
			
		}
		
	}

}
