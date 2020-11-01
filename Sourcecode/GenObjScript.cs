using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenObjScript : MonoBehaviour {

	public GameObject ObjClear;
	public GameObject[] ObjNormGen = new GameObject [8];
	public GameObject[] ObjLeftGen = new GameObject [5];
	public GameObject[] ObjRightGen = new GameObject [5];
	public GameObject[] ObjTrunkGen = new GameObject [6];
	public GameObject GenObj;
	float destroyTime = 0.5f;
	float ObjScale = 100.0f;
	float Number=3.0f;
	
	float PositionZ = 0;
	float MulMapScale = 0;
	float MulMapGenObj = 0;
	//float GenMapOBJPositionZ = 0.5f;
	float MapPositionX = 0;
	float MapPositionY = 0;
	float MapPositionZ = 0;
	public static float ProCrossX = 0;
	bool Enter = false;
	public static int countObj = 5;
	public static int CountSide = 9;
	
	public static List<string> BlackList = new List<string>();
	List <GameObject> ObjCanGen = new List<GameObject>();
	
	
	// Use this for initialization
	void Start() {
		bool Enter = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static void SetValue()
	{
		ProCrossX = 0;
		countObj = 5;
		CountSide = 8;
	}
	
	void OnTriggerEnter () {
		if(!Enter)
		{
			Enter = true;
			Quaternion rot   = Quaternion.identity;
			
			//	Debug.Log("Lefturn: " + AvatarController.LeftTurn + " RightTUrn: " + AvatarController.RightTurn + " CountSide: " + CountSide + " Count:" + count);
			
				for(int i = 0; i < 8; i++)
				{
					ObjCanGen.Add(ObjNormGen[i]);
				}
				
				if(AvatarController.LeftArmAllowed)
				{
				for(int i = 0; i < 5; i++){
					if(!BlackList.Contains(ObjLeftGen[i].name))
					ObjCanGen.Add(ObjLeftGen[i]);
				}
					//Debug.Log("NO Fatigue Left");
				}
				
				if(AvatarController.RightArmAllowed)
				{
				for(int i = 0; i < 5; i++){
					if(!BlackList.Contains(ObjRightGen[i].name))
					ObjCanGen.Add(ObjRightGen[i]);
				}
					//Debug.Log("NO Fatigue Right");
				}
				
				
				if(JumpFatigue.JumpAllowed)
				{
				for(int i = 0; i < 6; i++)
				{
					if(!BlackList.Contains(ObjTrunkGen[i].name))
					ObjCanGen.Add(ObjTrunkGen[i]);
				}
					//Debug.Log("NO Fatigue Trunk");
				}

				
				int ran = Random.Range(0,ObjCanGen.Count);
				
			//	Debug.Log("Count is:" + count);
				
				//if(count > 0)
				if(((AvatarController.LeftTurn && AvatarController.RightTurn)||(!AvatarController.LeftTurn && !AvatarController.RightTurn)) && countObj > 0)
				{
					PositionZ = transform.position.z;
					MapPositionZ = PositionZ+ObjScale*(Number+1);
					//Debug.Log("PositionZ Straight:" + MapPositionZ);
					MapPositionX = transform.position.x;
					GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
					int index = theObj.name.IndexOf("(");
					//get rid of the "(clone)" word
					if (index > 0)
  						theObj.name = theObj.name.Substring(0, index);
					MapPositionZ = PositionZ+ObjScale*(Number);
					Instantiate(GenObj,new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
					
			
				}
				//################################TURN LEFT##########################################
				//else if(count <= 0 && (AvatarController.LeftTurn && !AvatarController.RightTurn) && CountSide > 0)
				if(AvatarController.LeftTurn && !AvatarController.RightTurn)
				{
					if(CountSide > 0)
					{
						//Debug.Log("In Here");
						CountSide--;
						MapPositionZ = GenMap.PosCross + 125;
						MapPositionX = transform.position.x;
						GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX - (ObjScale*2) + 100,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						theObj.transform.Rotate(0,-90,0);
						int index = theObj.name.IndexOf("(");
						//get rid of the "(clone)" word
						if (index > 0)
  							theObj.name = theObj.name.Substring(0, index);
						//MapPositionZ = PositionZ+ObjScale*(Number);
						GameObject theGenObj = (GameObject)Instantiate(GenObj,new Vector3(MapPositionX - ObjScale,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						theGenObj.transform.Rotate(0,-90,0);
					}
			
			
					if(CountSide < 5 && CountSide > 0)
					{
						//Debug.Log ("Countside: " + CountSide);
						MapPositionZ = transform.position.z - 25 + ObjScale*(CountSide);
						//Debug.Log("SideObjPosZ: " + MapPositionZ);
						MapPositionX = ProCrossX - 875;
						GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						int index = theObj.name.IndexOf("(");
						//get rid of the "(clone)" word
						if (index > 0)
  							theObj.name = theObj.name.Substring(0, index);
					}
				}
			
				//##############################TURN RIGHT############################################
				if(!AvatarController.LeftTurn && AvatarController.RightTurn)
				{
					if(CountSide > 0)
					{
						//Debug.Log("In Here");
						CountSide--;
						MapPositionZ = GenMap.PosCross + 125;
						MapPositionX = transform.position.x;
						GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX + (ObjScale*2) - 100,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						theObj.transform.Rotate(0,90,0);
						int index = theObj.name.IndexOf("(");
						//get rid of the "(clone)" word
						if (index > 0)
  							theObj.name = theObj.name.Substring(0, index);
							//MapPositionZ = PositionZ+ObjScale*(Number);
						GameObject theGenObj = (GameObject)Instantiate(GenObj,new Vector3(MapPositionX + ObjScale,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						theGenObj.transform.Rotate(0,90,0);
					}
			
					if(CountSide < 5 && CountSide > 0)
					{
						//Debug.Log ("Countside: " + CountSide);
						MapPositionZ = transform.position.z - 25 + ObjScale*(CountSide);
						//Debug.Log("SideObjPosZ: " + MapPositionZ);
						MapPositionX = ProCrossX + 875 + 250;
						GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
						int index = theObj.name.IndexOf("(");
						//get rid of the "(clone)" word
						if (index > 0)
  							theObj.name = theObj.name.Substring(0, index);
					}
				}

				if(GenMap.IntersectionMap)
					countObj--;
			
				Destroy(transform.gameObject,destroyTime);//destroy this map

			
				
			
			
			
		}
		
	}
	
}




					//original
					/*PositionZ = transform.position.z;
					MapPositionZ = PositionZ+MapScale*(Number+1);
					GameObject theObj = (GameObject)Instantiate(ObjCanGen[ ran ],new Vector3(MapPositionX,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
					int index = theObj.name.IndexOf("(");
					//get rid of the "(clone)" word
					if (index > 0)
  						theObj.name = theObj.name.Substring(0, index);
					MapPositionZ = PositionZ+MapScale*(Number);
					Instantiate(GenObj,new Vector3(0,MapPositionY,MapPositionZ), rot);//position which will generate map beforehand
					Destroy(transform.gameObject,destroyTime);//destroy this map*/