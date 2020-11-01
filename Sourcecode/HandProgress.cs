using UnityEngine;
using System.Collections;

public class HandProgress : MonoBehaviour
{
	
	public Texture[] aTexture = new Texture[20];
	private GameObject mouse;
	// Use this for initialization
	void Awake()
	{
		mouse = GameObject.Find("HandCursor");
	}
	
	void Start ()
	{
	
	}
	
	
	void Update ()
	{
	
	}
	
	public void OnGUI ()
	{

		if (AvatarController.hitplay) {
		
			if (AvatarController.clickProgress > 0.3f && AvatarController.clickProgress < 0.35f) {
				mouse.guiTexture.texture = aTexture[5];
			} else if (AvatarController.clickProgress > 0.35f && AvatarController.clickProgress < 0.4f) {
				mouse.guiTexture.texture = aTexture[6];
			} else if (AvatarController.clickProgress > 0.4f && AvatarController.clickProgress < 0.45f) {
				mouse.guiTexture.texture = aTexture[7];
			} else if (AvatarController.clickProgress > 0.45f && AvatarController.clickProgress < 0.5f) {
				mouse.guiTexture.texture = aTexture[8];
			} else if (AvatarController.clickProgress > 0.5f && AvatarController.clickProgress < 0.55f) {
				mouse.guiTexture.texture = aTexture[9];
			} else if (AvatarController.clickProgress > 0.55f && AvatarController.clickProgress < 0.6f) {
				mouse.guiTexture.texture = aTexture[10];
			} else if (AvatarController.clickProgress > 0.6f && AvatarController.clickProgress < 0.65f) {
				mouse.guiTexture.texture = aTexture[13];
			} else if (AvatarController.clickProgress > 0.65f && AvatarController.clickProgress < 0.7f) {
				mouse.guiTexture.texture = aTexture[14];
			} else if (AvatarController.clickProgress > 0.7f && AvatarController.clickProgress < 0.75f) {
				mouse.guiTexture.texture = aTexture[15];
			} else if (AvatarController.clickProgress > 0.75f && AvatarController.clickProgress < 0.8f) {
				mouse.guiTexture.texture = aTexture[16];
			} else if (AvatarController.clickProgress > 0.8f && AvatarController.clickProgress < 0.85f) {
				mouse.guiTexture.texture = aTexture[17];
			} else if (AvatarController.clickProgress > 0.85f && AvatarController.clickProgress < 0.9f){
				mouse.guiTexture.texture = aTexture[18];
			} else if (AvatarController.clickProgress > 0.9f && AvatarController.clickProgress < 0.95f){
				mouse.guiTexture.texture = aTexture[19];
			}
		}else mouse.guiTexture.texture = aTexture[0];
		
	}





}
