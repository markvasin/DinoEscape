using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EggCollision : MonoBehaviour {
	
	public AudioClip Sound;
	bool Enter = false;
	
	public static List<EggData> EggList = new List<EggData>();
	
	public class EggData
	{
		public float posX = 0;
		public float posY = 0;
		public float posZ = 0;
		public string EggTime = "";
		
		public EggData()
		{
		}
		
		public EggData(float PX, float PY, float PZ, string ET)
		{
			posX = PX;
			posY = PY;
			posZ = PZ;
			EggTime = ET;
		}
	}
	
	
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)//
	{
		if(!Enter)//keep coin
		{
			//ZigSkeleton.score++;
			//ZigSkeleton.GemCount++;
			KinectManager.NumEgg++;
			KinectManager.Score += 10;
			//GameObject.Find("Score").guiText.text = KinectManager.Score.ToString("00000");
			GameObject.Find("Score").GetComponent<TextMesh>().text = KinectManager.Score.ToString("00000");
			GameObject.Find("eggtolife").GetComponent<TextMesh>().text = KinectManager.NumEgg.ToString("");
			
			if(KinectManager.NumEgg>=200){
				KinectManager.NumEgg = KinectManager.NumEgg-200;
				if(KinectManager.NumLife<=6){
					KinectManager.NumLife++;
					//KinectManager.LifeObj[KinectManager.NumLife-1].SetActive(true);
				}
			}
			Enter=true;
			
			AudioSource.PlayClipAtPoint(Sound,transform.position,1f);
			
			if(KinectManager.StudyMode)
			{
				EggData EggPos = new EggData(this.transform.position.x, this.transform.position.y, this.transform.position.z, DateTime.Now.ToString());
				EggList.Add(EggPos);
			}
			
			
		}
		Destroy(this.gameObject,0.0f); //destroy coin after keep
	}
	
}
