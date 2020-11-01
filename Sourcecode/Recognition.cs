using OpenCvSharp;
using OpenCvSharp.MachineLearning;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Runtime.InteropServices;
using UnityEngine;

public class Recognition : MonoBehaviour {
	
	//database connection
	static string connectionString = "URI=file://sqlite/ProjAvatarDB.db";
    static IDbConnection dbcon;
	
	//video size
	const int CAPTURE_WIDTH = 640;
    const int CAPTURE_HEIGHT = 480;
	
	//initialize variables (face recogiiton)
	private CvHaarClassifierCascade cascade;
	private CvCapture capture;
	public static IplImage image;
	private CvColor[] colors = new CvColor[]
	{
		new CvColor(0, 0, 255),
		new CvColor(0, 128, 255),
		new CvColor(0, 255, 255),
		new CvColor(0, 255, 0),
		new CvColor(255, 128, 0),
		new CvColor(255, 255, 0),
		new CvColor(255, 0, 0),
		new CvColor(255, 0, 255)
	};
	private int cameraWidth;
	private int cameraHeight;
	private byte[] test;
	private CvRect Face;
	private int faceWidth = 120;	// Default dimensions for faces in the face recognition database. 
	private int faceHeight = 90;	
	// Use this for initialization
	
	private bool save = false;
	private bool recog =false;
	private static List<string> personNames;
	private static IplImage [] faceImgArr ;
	private static IplImage [] eigenVectArr;
	private static IplImage pAvgTrainImg;
	private IplImage processedFaceImg;
	private static CvMat  personNumTruthMat;
	private static CvMat  projectedTrainFaceMat;
	private static CvMat eigenValMat;
	private static CvMat  trainPersonNumMat;
	//private int [] personNum;
	private static int nPersons = 0;
	private static int nTrainFaces = 0;
	private static int nEigens = 0;
	static float[] projectedTestFace;
	int nearest;
	static int picno=0;
	
	private Texture2D usersClrTex;
	private Rect usersClrRect;
	private Color32[] colorImage;
	private string InputName = "Input Name";
	public static string UserName;
	public static bool Userfound = false;
	private bool showImg = false;
	private float[] RecognizeUser;
	private GameObject ProgressText;
	private float time = 0f;
	private float Percent;
	private int[] maxIndex;
	public static string[] NameList;
	
	void Start () 
	{
	
		ProgressText = GameObject.Find("ProgressText");
		usersClrTex = new Texture2D(KinectWrapper.GetDepthWidth(), KinectWrapper.GetDepthHeight(), TextureFormat.RGBA32, false);
		usersClrRect = new Rect((float)Screen.width / 2f- usersClrTex.width/3f, Screen.height/4, (float)(usersClrTex.width/1.5 ), (float)(usersClrTex.height/1.5));
		colorImage = new Color32[KinectWrapper.GetDepthWidth() * KinectWrapper.GetDepthHeight()];
		
		cameraWidth = KinectWrapper.GetDepthWidth();
		cameraHeight = KinectWrapper.GetDepthHeight();
		CvSVM cvSVM = new CvSVM();
		CvTermCriteria term_crit = new CvTermCriteria(CriteriaType.Epsilon, 1000, 4.94065645841247E-324);
		CvSVMParams cvSVMParams = new CvSVMParams(SVMType.CSvc, SVMKernelType.Rbf, 10.0, 8.0, 1.0, 10.0, 0.5, 0.1, null, term_crit);
		test = new byte[921600];
		
		cascade = CvHaarClassifierCascade.FromFile("haarcascade_frontalface_alt.xml");
		image = KinectManager.image;
		
		//for webcam
		//image = Cv.CreateImage(Cv.Size(640, 480), BitDepth.U8, 3);
		//capture = Cv.CreateCameraCapture(0);
        //Cv.SetCaptureProperty(capture, CaptureProperty.FrameWidth, CAPTURE_WIDTH);
        //Cv.SetCaptureProperty(capture, CaptureProperty.FrameHeight, CAPTURE_HEIGHT);
	    //image = Cv.QueryFrame(capture);
       
		personNames = new List<string>();
		trainPersonNumMat=null;
	
		
		if(Application.loadedLevelName=="FaceLogin")
		{
			loadTrainingData( ref trainPersonNumMat );
			RecognizeUser = new float [nPersons];
			recog = true;
			//StartCoroutine (ConfirmUser());
		}
		if(Application.loadedLevelName=="FaceRegister")
		{
			loadTrainingData( ref trainPersonNumMat );
			showImg = true;
			if(InputNameScript.UserName!="")
				InputName = InputNameScript.UserName.ToLower();
			else
				InputName = "Unknown";
			
			Debug.Log(InputName);
			
			StartCoroutine (saveUserImage());
			
		}
	   
		
		projectedTestFace = new float[nEigens];
		Debug.Log("Total number of person: "+nPersons);
		Debug.Log("Total number of train faces "+nTrainFaces);
		Debug.Log("Total number of Eigens "+nEigens);
		
	
	}
	
	void OnGUI()
	{	
		
		
		GUI.DrawTexture(usersClrRect, usersClrTex);
		
		/*	
		if (GUI.Button(new Rect(80f, 70f, 100f, 30f), "SAVE PICTURE"))
			{
			save = true;
				StartCoroutine (saveUserImage());
				
				
				for(int i=0;i<nEigens;i++)
				{
					double minVal, maxVal;
		
					Cv.MinMaxLoc(eigenVectArr[i],out minVal,out maxVal);
				
					if (Cv.IsNaN(minVal) || minVal < -1e30)
						minVal = -1e30;
					if (Cv.IsNaN(maxVal) || maxVal > 1e30)
						maxVal = 1e30;
					if (maxVal-minVal == 0.0f)
						maxVal = minVal + 0.001;
					
					IplImage dstImg = Cv.CreateImage(eigenVectArr[i].Size, BitDepth.U8, 1);
					Cv.ConvertScale(eigenVectArr[i], dstImg, 255.0 / (maxVal - minVal), - minVal * 255.0 / (maxVal-minVal));
					Cv.SaveImage("eigenpic/"+i.ToString()+".bmp",dstImg);
				}
			}*/
		/*
			if (GUI.Button(new Rect(80f, 200f, 100f, 30f), "learn"))
			{
			
				Cv.ReleaseMat(trainPersonNumMat); // Free the previous data before getting new data
				trainPersonNumMat = retrainOnline();
				// Project the test images onto the PCA subspace
				//projectedTestFace = null;	// Free the previous data before getting new data
				projectedTestFace = new float[nEigens];
			
			}
			*/
//			if (GUI.Button(new Rect(80f, 250f, 100f, 30f), "Load data"))
//			{
//		
//				loadTrainingData( ref trainPersonNumMat );
//			}
		
		
//			if (GUI.Button(new Rect(80f, 300f, 100f, 30f), "learn"))
//			{
//				learn();
//			}
//		
		
		
			//InputName = GUI.TextField(new Rect(10, 10, 200, 20), InputName, 25);
		
			
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		
		if(AvatarController.UserNear)
			time += Time.deltaTime;;
		
		Percent = time * 100/7; 
		
		if(Percent<=100&&recog==true)
			ProgressText.GetComponent<TextMesh>().text = Percent.ToString("0.0")+"%";
		
		if(time>=7f)
			ConfirmUser();
		
		
		int iNearest;
	 	IplImage faceImg;
		IplImage sizedImg;
		IplImage equalizedImg;
		IplImage shownImg;
		float confidence=0f;
		
		//image = Cv.QueryFrame(capture);
		
		// detect the faces from video 
		Face = detectFaceInImage(ref image);
		
		// if face is detected
		if(Face.Height>0)
		{	
			faceImg = cropImage(ref image, Face);
			
			sizedImg = resizeImage(ref faceImg,faceWidth,faceHeight);
			
			equalizedImg = Cv.CreateImage(sizedImg.Size, BitDepth.U8, 1);
			
			IplImage grayImage = new IplImage(sizedImg.Size, BitDepth.U8, 1);
			Cv.CvtColor(sizedImg, grayImage, ColorConversion.BgrToGray);
			Cv.EqualizeHist(grayImage, equalizedImg);
			processedFaceImg = equalizedImg;
					
				
			if(save && AvatarController.UserNear)
			{
				picno++;
				if(picno<=20)
				{
					string a = 	string.Format("PicData/{0}{1}.pgm",InputName,picno); 
					Debug.Log(picno);
					processedFaceImg.SaveImage(a);
				
				
				/*using (StreamWriter sw = File.AppendText("markdata/mark.txt")) 
       		    {
					 string s = string.Format("{0} {1} markdata/{2}{3}.pgm",nPersons+1,InputName,InputName,picno);
           			 sw.WriteLine(s);
				}*/
				}
				save=false;
			}
			
			
			if(recog)
			{
				if (nEigens > 0) 
				{
					// project the test image onto the PCA subspace
					Cv.EigenDecomposite(processedFaceImg, eigenVectArr, pAvgTrainImg, projectedTestFace);
					
					// Check which person it is most likely to be.
					iNearest = findNearestNeighbor(projectedTestFace, ref confidence);
					nearest  = (int) trainPersonNumMat[iNearest];
					Debug.Log("Face detect:"+ personNames[nearest-1]+" Confidence = "+confidence);
	
				}//endif nEigens
			//	recog=false;
			}
			
			//Cv.ShowImage("Equalize",equalizedImg);
			
			Cv.ReleaseImage( faceImg );
			Cv.ReleaseImage( sizedImg );
			Cv.ReleaseImage( equalizedImg );
			
		}
		
		if(recog)
		{	
			using (shownImg = Cv.CloneImage(image))
			{
				if (Face.Height > 0) 
				{	// Check if a face was detected.
					// Show the detected face region.
					Cv.Rectangle(shownImg, Cv.Point(Face.X+10, Face.Y-10), Cv.Point(Face.X + Face.Width+10,Face.Y + Face.Height+10), Cv.RGB(0,255,0), 2, LineType.Link8, 0);
					if (nEigens > 0)
					{	
						// Show the name of the recognized person
						CvFont font;
						Cv.InitFont(out font,FontFace.HersheyPlain, 1.5, 1.5, 0, 2,LineType.AntiAlias);
						CvScalar textColor = Cv.RGB(255,51,51);	
						string text="";
							
						if(confidence>=0.80)
						{
							text = personNames[nearest-1];
							if(confidence>=0.90)
								RecognizeUser[nearest-1]+=5;
							
							else if(confidence>=0.85)
								RecognizeUser[nearest-1]+=3;
							
							else if(confidence>=0.80)
								RecognizeUser[nearest-1]++;
							
							//Debug.Log(RecognizeUser[nearest-1]);
								//if(confidence>=0.93)
							//{
								//UserName = personNames[nearest-1];
								//Application.LoadLevel("ConfirmUser");
							
							//Userfound = true;		
							//}
						}
						else if(confidence>=0.60)
							{
								RecognizeUser[nearest-1]+=0.2f;
								//text = "";
							}
						else
							text = "";
					
						//Debug.Log(text);
						Cv.PutText(shownImg, text, Cv.Point(Face.X+10, Face.Y + Face.Height + 30), font, textColor);
						//Cv.PutText(shownImg, text, Cv.Point(Face.x, Face.y + Face.Height + 30), font, textColor);
					}
				}

				// Display the image.
				//Cv.ShowImage("Input", shownImg);
				
				// Convert OpenCV iplimage to Unity color image 
				Marshal.Copy(shownImg.ImageData, test, 0, 921600);
				
				for(int i = 0,k=479; i < 480; i++,k--)
				{
					for(int j = 0; j < 640; j++)
					{
						int index2 = (k*640+j);
						int index = (i*640+j);
						colorImage[index2].r = test[index*3+2];
						colorImage[index2].g = test[index*3+1];
						colorImage[index2].b = test[index*3];
						colorImage[index2].a = 255;
					}				
				}
				
				usersClrTex.SetPixels32(colorImage);
				usersClrTex.Apply();
				
			}
			Cv.ReleaseImage( shownImg );	
		}
		
				
		
	
	}
	void OnDestroy()
    {
		// faceImgArr.Dispose();// array of face images
		//personNumTruthMat.Dispose();// array of person numbers
		personNames.Clear();			// array of person names (indexed by the person number). Added by Shervin.
		nPersons = 0; // the number of people in the training set. Added by Shervin.
		nTrainFaces = 0; // the number of training images
		nEigens = 0; // the number of eigenvalues
		Cv.ReleaseImage( pAvgTrainImg ); // the average image
		for (int i=0; i<nTrainFaces; i++)
		{
			Cv.ReleaseImage( eigenVectArr[i] );
		}
		Cv.ReleaseImage(image);
		//Cv.Free(ref eigenVectArr); // eigenvectors
		//Cv.Free (ref eigenValMat);// eigenvalues
		//projectedTrainFaceMat.Dispose( ); // projected training faces
	    Cv.DestroyAllWindows();
	    Cv.ReleaseHaarClassifierCascade(cascade);
    }
	
	public static void RemoveUSER()
	{
		Cv.ReleaseMat(trainPersonNumMat); // Free the previous data before getting new data
		trainPersonNumMat = retrainOnline();
		// Project the test images onto the PCA subspace
		//projectedTestFace = null;	// Free the previous data before getting new data
		projectedTestFace = new float[nEigens];
		picno=0;
	}
	
	IEnumerator saveUserImage()
	{
		yield return new WaitForSeconds(2f);
		while(picno<20)
		{
			Percent = picno * 100/20;
			if (Percent>=95)
				Percent =100;
			if(Percent<=100)
				ProgressText.GetComponent<TextMesh>().text = Percent.ToString("0.0")+"%";
			save = true;
			yield return new WaitForSeconds(0.1f);
			
		}
				
		ProjAvatarDB.UserRegister();
		Cv.ReleaseMat(trainPersonNumMat); // Free the previous data before getting new data
		trainPersonNumMat = retrainOnline();
		// Free the previous data before getting new data
		projectedTestFace = new float[nEigens];
		picno=0;
		Application.LoadLevel("Menu");
		
	}
	
	void ConfirmUser()
	{
		//yield return new WaitForSeconds(8f);
		maxIndex = new int[4];
		NameList = new string[3]; 
		//Debug.Log("confirm");
		
		for(int j=0;j<4;j++)
		{
			//for (int i=0; i<nPersons;i++)
			//Debug.Log(i+" "+RecognizeUser[i]);
			
			float maxValue = RecognizeUser.Max();
			
			if (maxValue<=0)
				maxIndex[j] = -1;
			else
			{
				maxIndex[j] = RecognizeUser.ToList().IndexOf(maxValue);
				RecognizeUser[maxIndex[j]] =  0;
			}
//			if(maxValue==0)
//				UserName = null;
//			else
//			{	
//				int maxIndex = RecognizeUser.ToList().IndexOf(maxValue);
//				UserName = personNames[maxIndex];
//			}
		}
		
		if(maxIndex[0]==-1)
			UserName = null;
		else
			UserName = personNames[maxIndex[0]];
		
		for(int j=1;j<4;j++)
		{
			if(maxIndex[j]>=0)
				NameList[j-1] = personNames[maxIndex[j]];
			else
				NameList[j-1] = null;	
		}
			//Debug.Log(maxIndex[j]);
		
		Application.LoadLevel("ConfirmUser");
	}
	
	
	static CvMat retrainOnline()
	{
		CvMat trainPersonNumMat=null;
		int i;
	
		Cv.ReleaseMat( personNumTruthMat ); 
		personNames.Clear();			
		nPersons = 0; 
		nTrainFaces = 0;
		nEigens = 0; 
		Cv.ReleaseImage( pAvgTrainImg ); 
	
		for ( i=0; i<nTrainFaces; i++)
		{
			Cv.ReleaseImage( eigenVectArr[i] );
		}
				
		// Retrain from the data in the files
		learn();
			
		// Load the previously saved training data
		if( loadTrainingData( ref trainPersonNumMat ) == 0) 
		{
			Debug.Log("ERROR : Couldn't load the training data!");
		}

		return trainPersonNumMat;
	}
	
	
	public CvRect detectFaceInImage(ref IplImage inputImg )
	{
		CvRect facerect;
	
		using (IplImage detectImage = Cv.CloneImage(inputImg))
		{
			using (IplImage iplImage2 = new IplImage(new CvSize(Cv.Round((double)detectImage.Width / 1.04), Cv.Round((double)detectImage.Height / 1.04)), BitDepth.U8, 1))
			{
				using (IplImage grayImage = new IplImage(detectImage.Size, BitDepth.U8, 1))
				{
					Cv.CvtColor(detectImage, grayImage, ColorConversion.BgrToGray);
					Cv.Resize(grayImage, iplImage2, Interpolation.Linear);
					Cv.EqualizeHist(iplImage2, iplImage2);
				}
				using (CvMemStorage cvMemStorage = new CvMemStorage())
				{
					cvMemStorage.Clear();
					CvSeq<CvAvgComp> cvSeq = Cv.HaarDetectObjects(iplImage2, cascade, cvMemStorage, 1.1, 3, HaarDetectionType.Zero, new CvSize(64, 64));
					
					for (int k = 0; k < cvSeq.Total; k++)
					{
						CvRect rect = cvSeq[k].Value.Rect;
						CvPoint center = new CvPoint
						{
							X = Cv.Round(((double)rect.X + (double)rect.Width * 0.5) * 1.04),
							Y = Cv.Round(((double)rect.Y + (double)rect.Height * 0.5) * 1.04)
						};
						int radius = Cv.Round((double)(rect.Width + rect.Height) * 0.25 * 1.04);
						detectImage.Circle(center, radius, colors[k % 8], 3, LineType.AntiAlias, 0);
					}
					if (cvSeq.Total > 0)
					{
						//facerect = cvSeq[0].Value.Rect;
						facerect = (CvRect)Cv.GetSeqElem( cvSeq, 0 );
					
					}
					else
					facerect = new CvRect (-1,-1,-1,-1);
				}
		
			}
			
			if(showImg)
			{
				// convert OpenCV Iplimage to Unity color image
				Marshal.Copy(detectImage.ImageData, test, 0, 921600);
		
				for(int i = 0,k=479; i < 480; i++,k--)
				{
					for(int j = 0; j < 640; j++)
					{
						int index2 = (k*640+j);
						int index = (i*640+j);
						colorImage[index2].r = test[index*3+2];
						colorImage[index2].g = test[index*3+1];
						colorImage[index2].b = test[index*3];
						colorImage[index2].a = 255;
					}				
				}
				
				usersClrTex.SetPixels32(colorImage);
				usersClrTex.Apply();
				
				
			}
			
		}
	
	return facerect;
	
	}
	
	
	//crop image function
	IplImage cropImage(ref IplImage img,  CvRect region)
	{
		IplImage imageTmp;
		IplImage imageRGB;
		
		// First create a new (color or greyscale) IPL Image and copy contents of img into it.
		imageTmp = Cv.CreateImage(new CvSize(img.Width,img.Height), img.Depth, img.NChannels);
		Cv.Copy(img, imageTmp,null);
			
		// Create a new image of the detected region
		// Set region of interest to that surrounding the face
		Cv.SetImageROI(imageTmp, region);
		
		// Copy face region into a new iplImage (imageRGB) and return it
		imageRGB = Cv.CreateImage(new CvSize(region.Width,region.Height), imageTmp.Depth,imageTmp.NChannels);
		Cv.Copy(imageTmp, imageRGB, null);	// Copy just the region.
	
	    Cv.ReleaseImage( imageTmp );
		
		return imageRGB;		
	}
	
	
	//resizing image function
	IplImage resizeImage(ref IplImage origImg, int newWidth, int newHeight)
	{
		IplImage outImg;
		
		// Scale the image to the new dimensions
		outImg = Cv.CreateImage(new CvSize(newWidth, newHeight), origImg.Depth, origImg.NChannels);
		
		if (newWidth > origImg.Width && newHeight > origImg.Height)
		{
			// Make the image larger
			Cv.ResetImageROI((IplImage)origImg);
			Cv.Resize(origImg, outImg, Interpolation.Linear);	//LINEAR is good for enlarging
		}
		else 
		{
			// Make the image smaller
			Cv.ResetImageROI((IplImage)origImg);
			Cv.Resize(origImg, outImg, Interpolation.Area);	//AREA is good for shrinking.
		}
			
		return outImg;
	}
	
	
	// load images from file
	static int loadFaceImgArray()
	{
		//File imgListFile;
		string imgFilename;
		int iFace, nFaces=0;
		int i, countRow = 0;
		int countNumberPlayer = 0;
		//StreamReader reader;
		// int count = 0;  
	   	/*using ( reader = File.OpenText(filename))  
	     {  
	      	string line; 
				
             while ((line = reader.ReadLine()) != null)  
			{
				++nFaces;
				
			}
		 }
		reader.Close();*/
			
		// allocate the face-image array and person number matrix
		
		//nPersons = ProjAvatarDB.getNumPlayer();
		nFaces = ProjAvatarDB.getNumPlayer()*20;
		
		faceImgArr  = new IplImage [ nFaces];
		personNumTruthMat = Cv.CreateMat( 1, nFaces, MatrixType.S32C1 );	
		personNames.Clear();	// Make sure it starts as empty.
		nPersons = 0;
				
		//using ( reader = File.OpenText(filename))  
	  //  {
			//string []parts;
			
			//for(iFace=0; iFace<nFaces; iFace++)
			//{
			   /* parts = reader.ReadLine().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
				
				string personName;
				string sPersonName;
				int personNumber;
				personNumber = Int32.Parse(parts[0]); 
				personName = parts[1]; 
				imgFilename = parts[2];	
				sPersonName = personName;
				*/
					
		
		//load user name from database
		 dbcon = (IDbConnection) new SqliteConnection(connectionString);
		 dbcon.Open();
         IDbCommand dbcmd = dbcon.CreateCommand();

		 string sqlSELECT = "SELECT * FROM PLAYER_INFO";
		 dbcmd.CommandText = sqlSELECT;
		 IDataReader reader = dbcmd.ExecuteReader();
		
		 while(reader.Read())
		 {
			countNumberPlayer++;
   		 	string UserStringID = reader.GetString (0);
   			string UserNameDB = reader.GetString (1);
			int UserID = Convert.ToInt32(UserStringID);
        	Debug.Log("UserID: " + UserID + " UserName" + UserNameDB );
			if (countNumberPlayer > nPersons)
		 	{
				for (i=nPersons; i < countNumberPlayer; i++)
				{
					personNames.Add(UserNameDB);
				}
				nPersons = countNumberPlayer;
		 	}
			
			for(int k = 0; k < 20; k++)
			{
				personNumTruthMat[(countRow*20)+k] = countNumberPlayer;
				imgFilename = string.Format("PicData/{0}{1}.pgm",UserNameDB ,k+1);
				Debug.Log(imgFilename);
				faceImgArr[(countRow*20)+k] = Cv.LoadImage(imgFilename, LoadMode.GrayScale);
		    }
			countRow++;
		 }

	       
	      			
					
		//personNames = ProjAvatarDB.getPlayerList();
		//	string myString = String.Format("{0} {1} {2}",personNumber, personName, imgFilename);
		//	Debug.Log(myString);
		//personNumTruthMat[iFace] = personNumber;
		// load the face image
		//Debug.Log("load face "+iFace);
		//faceImgArr[iFace] = Cv.LoadImage(imgFilename, LoadMode.GrayScale);
		//Cv.ShowImage(iFace.ToString(),faceImgArr[iFace]);
		//}
 //printf("Got %d: %d, <%s>, <%s>.\n", iFace, personNumber, personName, imgFilename);												
	//}
	   
		// clean up
	   reader.Close();
       reader = null;
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
		
		//Debug.Log("Data loaded from "+filename+" ("+ nFaces +"images of "+nPersons+" people).\n");
		Debug.Log("People: ");
		if (nPersons > 0)
			Debug.Log(personNames[0]);
		for (i=1; i<nPersons; i++) {
			Debug.Log(personNames[i]);
		}
		
		return nFaces;
	
	}
	
	//learning process
	static void learn()
	{
		// load training data
		nTrainFaces = loadFaceImgArray();
		Debug.Log("Got "+ nTrainFaces+" training images");
		if( nTrainFaces < 2 )
		{
			Debug.LogError("Need 2 or more training faces\n Input file contains only "+nTrainFaces);
			return;
		}
		
		// do PCA on the training faces
		doPCA();
			
		List<float> projectionList = new List<float>();
	
		// project the training images onto the PCA subspace
		for(int i=0; i<nTrainFaces; i++)
		{
				
			float[] projectionArray = new float[nEigens];
			try
			{ 
				Cv.EigenDecomposite(faceImgArr[i], eigenVectArr, pAvgTrainImg, projectionArray);
			}
			
			catch(Exception e)
			{
				
				Debug.LogError(e.Message);
			}
				
			projectionList.AddRange(projectionArray);
				
				
		}
		
		projectedTrainFaceMat = new CvMat( nTrainFaces, nEigens, MatrixType.F32C1,projectionList.ToArray());
		
		// store the recognition data as an xml file
		storeTrainingData();
	
		Cv.SaveImage("out_averageImage.bmp", pAvgTrainImg);
	
	}
	
	//calculating PCA (eigenface)
	static void doPCA()
	{
		CvTermCriteria calcLimit;
		CvSize faceImgSize = new CvSize(120,90);
	
		// set the number of eigenvalues to use
		nEigens = nTrainFaces-1;
		Debug.Log(nEigens);
		
		eigenVectArr = new IplImage[ nEigens ];
	
		for(int i=0; i<nEigens; i++)
			eigenVectArr[i] =  new IplImage(120, 90, BitDepth.F32, 1);
								//Cv.CreateImage(faceImgSize, BitDepth.F32, 1);
	
		// allocate the eigenvalue array
		float[] eigenvalueArray = new float[nEigens];
		// allocate the averaged image
		pAvgTrainImg = new IplImage(120, 90, BitDepth.F32, 1);//Cv.CreateImage(faceImgSize,BitDepth.F32 , 1);
		
		// set the PCA termination criterion
		calcLimit = new CvTermCriteria(CriteriaType.Iteration, nEigens, 1);
			
		Cv.CalcEigenObjects(faceImgArr, eigenVectArr, 0, calcLimit,  pAvgTrainImg, eigenvalueArray);
		eigenValMat = new CvMat( 1, nEigens,MatrixType.F32C1,eigenvalueArray );
		
		//Cv.Normalize(eigenValMat, eigenValMat, 1, 0,NormType.L1, null);
	}
	
	//store training data to xml file
	public static void storeTrainingData()
	{
        CvFileStorage saveFile = new CvFileStorage("facedata.xml", null, FileStorageMode.Write);
        int i;
	    saveFile.WriteInt("nPersons", nPersons );
			
		for (i=0; i<nPersons; i++) 
		{
			byte[] stringBytes = Encoding.ASCII.GetBytes(personNames[i]);
			CvMat charArr = new CvMat(1,stringBytes.Length, MatrixType.U8C1, stringBytes);
			saveFile.Write(  "personName_"+(i+1),charArr);
		}	
		
		saveFile.WriteInt("nEigens", nEigens);
        saveFile.WriteInt("nTrainFaces", nTrainFaces);
        saveFile.Write( "trainPersonNumMat", personNumTruthMat);
		saveFile.Write("eigenValMat", eigenValMat);
        saveFile.Write("projectedTrainFaceMat", projectedTrainFaceMat);
        saveFile.Write("avgTrainImg", pAvgTrainImg);

        for (int eigenIndex = 0; eigenIndex < nEigens;eigenIndex++)
        {
            saveFile.Write("eigenVect_" + eigenIndex,eigenVectArr[eigenIndex]);
        }

        saveFile.Dispose();
		
	}

	
	// Open the training data from the file 'facedata.xml'.
	static int loadTrainingData(ref CvMat pTrainPersonNumMat)
	{
		CvFileStorage  fileStorage;
		
		// create a file-storage interface
		fileStorage = new CvFileStorage("facedata.xml", null, FileStorageMode.Read);
		
		if( fileStorage==null ) {
			Debug.Log("Can't open training database file 'facedata.xml' ");
			return 0;
		}
	
		// Load the person names.
		personNames.Clear();	// Make sure it starts as empty.
		nPersons = fileStorage.ReadIntByName(null, "nPersons", 0);
		if (nPersons == 0)
		{
			Debug.Log("No people found in the training database 'facedata.xml'.");
			return 0;
		}
		// Load each person's name.
		for (int i=0; i<nPersons; i++) 
		{
			CvMat charArr = fileStorage.ReadByName<CvMat>(null,   "personName_"+(i+1));
	        byte[] charBytes = new byte[charArr.Cols];
	        for(int j = 0; j<charBytes.Length; j++)
			{
				charBytes[j] = charArr.DataArrayByte[j];
			}
	        string sPersonName = Encoding.ASCII.GetString(charBytes);
			
			personNames.Add( sPersonName );
		}
	
		// Load the data
		nEigens = fileStorage.ReadIntByName( null, "nEigens", 0);
		nTrainFaces = fileStorage.ReadIntByName(null, "nTrainFaces", 0);
		pTrainPersonNumMat = fileStorage.ReadByName<CvMat>( null, "trainPersonNumMat");
		eigenValMat  = fileStorage.ReadByName<CvMat>(null, "eigenValMat");
		projectedTrainFaceMat = fileStorage.ReadByName<CvMat>(null, "projectedTrainFaceMat");
		pAvgTrainImg = fileStorage.ReadByName<IplImage>( null, "avgTrainImg");
		eigenVectArr = new IplImage[ nEigens ];
				
		for(int i=0; i<nEigens; i++)
		{
			eigenVectArr[i] = fileStorage.ReadByName<IplImage>(null, "eigenVect_" + i);
		}
	
		// release the file-storage interface
		fileStorage.Dispose();
	
		Debug.Log("Training data loaded ("+ nTrainFaces +" training images of " + nPersons + " people): ");
		Debug.Log("People: ");
		if (nPersons > 0)
			Debug.Log(personNames[0]);
		for (int i=1; i<nPersons; i++) {
			Debug.Log(personNames[i]);
		}
	
	
		return 1;
	}
	
	//find the nearest match of user
	int findNearestNeighbor(float[] projectedFaceArray,ref float pConfidence)
	{
         double bestDistance = double.MaxValue;
         int bestMatch = 0;

         for (int sampleIndex = 0; sampleIndex < nTrainFaces; sampleIndex++)
         {
            double distance = 0;

            for (int eigenIndex = 0; eigenIndex < nEigens; eigenIndex++)
            {
                double componentDistance = projectedFaceArray[eigenIndex] - projectedTrainFaceMat[sampleIndex , eigenIndex];
                distance += componentDistance * componentDistance;//eigenValMat[eigenIndex];
				
            }
			
            if(distance < bestDistance)
            {
                bestDistance = distance;
                bestMatch = sampleIndex;
            }
        }
		//find confidence value
		pConfidence = 1.0f- ((float)Math.Sqrt( bestDistance / (nTrainFaces * nEigens) )/255.0f) ;
	
        return bestMatch;
	}
	
	
}