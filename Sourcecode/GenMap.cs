using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenMap : MonoBehaviour {

	public GameObject[] nextMap = new GameObject [11];
	//public GameObject endMap;
	//public bool[] store = new bool[4];
	float destroyTime = 2.0f;
	float MapScale=250f;
	float Number=5.0f;
	//float GenMapOBJPositionZ = 0.5f;
	public static float MapPosGenX = 0;
	float MapPosGenY = 0;
	public static float MapPosGenZ = 0;
	public static float PosCross = 0;
	int random = 0;
	public static int count = 0;
	public static bool IntersectionMap = false;
	public static bool Turn = false;
	bool Enter;
	
	public static List<MapData> MapList = new List<MapData>();
	
	public static void SetValue()
	{
		IntersectionMap = false;
		Turn = false;
		count = 0;
		MapPosGenX = 0;
		MapPosGenZ = 0;
		PosCross = 0;
	}
	
	public class MapData
	{
		public string MapName = "";
		public float posX = 0;
		public float posZ = 0;
		public string EggTime = "";
		
		public MapData()
		{
		}
		
		public MapData(string MN, float PX, float PZ, string MT)
		{
			MapName = MN;
			posX = PX;
			posZ = PZ;
			EggTime = MT;
		}
	}
	
	// Use this for initialization
	void Start () {
	
		Enter = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter () {
		
		if(!Enter)
		{
			
			Quaternion rot   = Quaternion.identity;
			
			
			if((AvatarController.LeftTurn && AvatarController.RightTurn)||(!AvatarController.LeftTurn && !AvatarController.RightTurn))
			{
				count = 0;
				random = UnityEngine.Random.Range(0,10);
				
				MapPosGenZ = transform.parent.position.z + MapScale*Number;
				if(!IntersectionMap)
					Instantiate(nextMap[random],new Vector3(MapPosGenX,MapPosGenY,MapPosGenZ), rot);//position which will generate map beforehand
				if(random>=9)
					IntersectionMap = true;
			}
			
			if(((AvatarController.LeftTurn && !AvatarController.RightTurn)||(!AvatarController.LeftTurn && AvatarController.RightTurn)) && count < 4)
			{
				count++;
				//random = Random.Range(0,8);
				//MapPosGenZ = (MapScale*count) + 125;
				//Instantiate(nextMap[random],new Vector3(MapPosGenX,MapPosGenY,(MapScale*((count*2)-1))+ PosCross + 125), rot);
				random = UnityEngine.Random.Range(0,8);
				Instantiate(nextMap[random],new Vector3(MapPosGenX,MapPosGenY,(MapScale*(count)) + PosCross + 125), rot);
				IntersectionMap = false;
			}
			
			
			//Debug.Log("random =" + random);
			if((AvatarController.LeftTurn&&AvatarController.RightTurn) && !Turn)
			{	
				Turn = true;
				destroyTime = 30.0f;
			}
			
			if(KinectManager.StudyMode)
			{
				MapData MapPos = new MapData(transform.parent.gameObject.name, transform.parent.position.x, transform.parent.position.z, DateTime.Now.ToString());
				MapList.Add(MapPos);
			}
			
			Destroy(transform.parent.gameObject,destroyTime);//destroy this map 
			Enter = true;
			
		}
		
	}
//	void OnGUI() {
//		GUI.color = Color.blue;
//		GUI.skin.label.fontSize = 50;
//	}
}
