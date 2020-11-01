using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour
{
	

	public GameObject[] character = new GameObject [3];
	// Use this for initialization
	
//	void Start()
//	{
//		character[KinectManager.CharacterNo].SetActive(true);
//	}

	IEnumerator Start ()
	{
		character[KinectManager.CharacterNo].SetActive(true);
		AsyncOperation async = Application.LoadLevelAsync ("PlayGame");
		yield return async;

		
	}
	
		
}
