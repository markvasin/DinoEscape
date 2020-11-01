using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class ProjAvatarDB{
	
	//the path for connecting database
	static string connectionString = "URI=file://sqlite/ProjAvatarDB.db";
    static IDbConnection dbcon;
	//initial value
	public static int CountBlackList = 0;
	public static int UserPlayerID = 0;
	
	//count the number of player in database(PLAYER_INFO table)
	public static int getNumPlayer()
	{
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
	    string sqlCOUNT = "SELECT COUNT(*) FROM PLAYER_INFO";
		dbcmd.CommandText = sqlCOUNT;
      	int NumPlayer = Convert.ToInt32(dbcmd.ExecuteScalar());
		
       // clean up
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
	   return NumPlayer;
	}
	//Get the blacklist of the player from BLACKLIST table
	public static void GetBlackList()
	{
	   
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
	   string sqlSELECT = "SELECT * FROM BLACKLIST WHERE USERID='"+ UserPlayerID +"'";
       dbcmd.CommandText = sqlSELECT;
       IDataReader reader = dbcmd.ExecuteReader();
       while(reader.Read()) {
            string USERID = reader.GetString (0);
			string BlackList = reader.GetString (1);
			GenObjScript.BlackList.Add(BlackList);
       }
		
	   CountBlackList = GenObjScript.BlackList.Count;
       
		// clean up
       reader.Close();
       reader = null;
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
		
	}
	//Get Collect more Blacklist (if any) after game over
	public static void CollectBlackList()
	{
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
	   for(int i = CountBlackList; i<GenObjScript.BlackList.Count; i++)
	   {
			string sqlCOLLECT = "INSERT INTO BLACKLIST(USERID,BLIST) VALUES('"+ UserPlayerID +"','"+ GenObjScript.BlackList[i] +"')";
      		dbcmd.CommandText = sqlCOLLECT;
       		dbcmd.ExecuteNonQuery();
	   }
		
		// clean up
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
	}
	//Register New User
	public static bool CheckUserName()
	{
	   List<string> PlayerList = new List<string>();
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
	   string playername = InputNameScript.UserName.ToLower();
		
	 
			string sqlNAME = "SELECT USERNAME FROM PLAYER_INFO";
       		dbcmd.CommandText = sqlNAME;
       		IDataReader reader = dbcmd.ExecuteReader();
       		while(reader.Read()) {
            	string UserName = reader.GetString(0);
				PlayerList.Add(UserName.ToLower());
       		}
		//prevent the redundant name in database
			
		
		// clean up
		reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
		
		if(PlayerList.Contains(playername))
			{
				
				return false;
			}
	
		return true;
	}
	
	
	public static void UserRegister()
	{
		
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
	   string playername = InputNameScript.UserName.ToLower();
		
	   string sqlINSERT = "INSERT INTO PLAYER_INFO(USERNAME) VALUES('"+ playername +"')";
	   dbcmd.CommandText = sqlINSERT;
	   dbcmd.ExecuteNonQuery();	
	
		
		// clean up
		dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
		
		
	}
	
	//User Log In
	public static bool UserLogIn()
	{
		
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
		//Get PlayerID	
			string sqlID = "SELECT USERID FROM PLAYER_INFO WHERE USERNAME = '"+ Recognition.UserName.ToLower() +"'";
     		dbcmd.CommandText = sqlID;
			object userObjID = dbcmd.ExecuteScalar();
			if(userObjID == null)
			{
			 	dbcmd.Dispose();
      		 	dbcmd = null;
        		dbcon.Close();
        		dbcon = null;
				return false;
			}
			else
				UserPlayerID = (Int32)userObjID;
		
		// clean up
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
		return true;
	}
	
	
	//set score after game over
	public static void SetScore()
	{
	   int NewScore = KinectManager.Score;
	   int CurScore = 0;
	   int NumRow = 0;
	
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
		//get the old bestscore in database
			string sqlSELECT = "SELECT BESTSCORE FROM PLAYER_INFO WHERE USERID = '"+ UserPlayerID +"'";
     		dbcmd.CommandText = sqlSELECT;
			object CurrentScore = dbcmd.ExecuteScalar();
		
		 	if(CurrentScore != null)
				CurScore = (Int32)CurrentScore;
		
		//If the New score is Greater than the Old score
				if(NewScore > CurScore)
				{
				
					string sqlUPDATE = "UPDATE PLAYER_INFO SET LatestDate = '"+ DateTime.Today.ToString("yyyy-MM-dd") +"', BESTSCORE = '"+ NewScore +"' WHERE USERID = '"+ UserPlayerID +"'";
					dbcmd.CommandText = sqlUPDATE;
					dbcmd.ExecuteNonQuery();
				
				}
		//add the number of round the user has ever played
			string sqlNumPlay = "SELECT NumPlay FROM PLAYER_INFO WHERE USERID = '"+ UserPlayerID +"'";
     		dbcmd.CommandText = sqlNumPlay;
			object PlayerNum = dbcmd.ExecuteScalar();
			if(PlayerNum != null)
			{
				int PlayNum = (Int32)PlayerNum + 1;
				string sqlUPDATENumPlay = "UPDATE PLAYER_INFO SET NumPlay = '"+ PlayNum +"' WHERE USERID = '"+ UserPlayerID +"'";
				dbcmd.CommandText = sqlUPDATENumPlay;
				dbcmd.ExecuteNonQuery();
			}
			else
			{
				string sqlUPDATENumPlay = "UPDATE PLAYER_INFO SET NumPlay = '1' WHERE USERID = '"+ UserPlayerID +"'";
				dbcmd.CommandText = sqlUPDATENumPlay;
				dbcmd.ExecuteNonQuery();
			}
		
		//UPDATE HIGHSCORE TABLE
		//Check the Number of Rows in HIGHSCORE table
		string sqlCOUNT = "SELECT COUNT(*) FROM HIGHSCORE";
		dbcmd.CommandText = sqlCOUNT;
      	NumRow = Convert.ToInt32(dbcmd.ExecuteScalar());
      	
		//Update High Score Table
		if(NumRow>=5)
		{
			string sqlMIN = "SELECT MIN(SCORE) FROM HIGHSCORE";
     		dbcmd.CommandText = sqlMIN;
			int MinScore = Convert.ToInt32(dbcmd.ExecuteScalar());
			
			if(NewScore>MinScore)
			{
				string sqlUDHIGHSCORE = "UPDATE HIGHSCORE SET USERID = '"+ UserPlayerID +"', SCORE = '"+ NewScore +"', DATEDAY = '"+ DateTime.Today.ToString("yyyy-MM-dd") +"' WHERE SCORE = (SELECT MIN(SCORE) FROM HIGHSCORE)";
				dbcmd.CommandText = sqlUDHIGHSCORE;
				dbcmd.ExecuteNonQuery();
			}
			
		}
		else //Row less than 5 then insert
		{
			string sqlISRTHIGHSCORE = "INSERT INTO HIGHSCORE(USERID,SCORE,DATEDAY) VALUES('"+ UserPlayerID +"','"+ NewScore +"','"+ DateTime.Today.ToString("yyyy-MM-dd") +"')";
			dbcmd.CommandText = sqlISRTHIGHSCORE;
			dbcmd.ExecuteNonQuery();
		}
		
       // clean up
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
		
	}
	//Get the Top 5 HighScore
	public static void GetHighScore()
	{
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
       string UserName = "";
	   string Score = "";
       string DateData = "";
	   int count = 0;
		
	   string sqlH = "SELECT USERNAME,SCORE,DATEDAY FROM PLAYER_INFO,HIGHSCORE WHERE HIGHSCORE.USERID=PLAYER_INFO.USERID ORDER BY SCORE DESC";
       dbcmd.CommandText = sqlH;
       IDataReader reader = dbcmd.ExecuteReader();
       while(reader.Read())
	   {
            string USERID = reader.GetString (0);
			string SCORE = reader.GetString (1);
			DateTime DateT = reader.GetDateTime (2);
			string DDATE = DateT.ToString("dd-MM-yyyy");
			UserName = UserName + USERID;
			Score = Score + SCORE;
			DateData = DateData+ DDATE;
			if(count<=3){
				UserName = UserName + "\n";
				Score = Score + "\n";
				DateData = DateData + "\n";
			}
			count++;
       }
		//display on HighScore scene
		GameObject.Find("UserHighScore").GetComponent<TextMesh>().text = UserName;
		GameObject.Find("HighScore").GetComponent<TextMesh>().text = Score;
		GameObject.Find("DateHighScore").GetComponent<TextMesh>().text = DateData;
		
		// clean up
       reader.Close();
       reader = null;
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
	}
	//Get Player Info
	public static void GetUserInfo()
	{
		
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
	   string Score = "No Data";
	   string latestDate = "No Data";
	   string numplay = "No Data";
		
	   string sqlSELECT = "SELECT * FROM PLAYER_INFO WHERE USERID = '"+ UserPlayerID +"'";
       dbcmd.CommandText = sqlSELECT;
       IDataReader reader = dbcmd.ExecuteReader();
       while(reader.Read()) {
			if(!reader.IsDBNull (3))
			{
				DateTime DateT = reader.GetDateTime (2);
				latestDate = DateT.ToString("dd-MM-yyyy");
				Score = reader.GetString (3);
				numplay = reader.GetString (4);
			}
       }
		//Display on UserInfo Scene
		GameObject.Find("InfoText").GetComponent<TextMesh>().text = Score + "\n\n\n" + latestDate + "\n\n\n" + numplay;
		
       // clean up
       reader.Close();
       reader = null;
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
		
		
	}
	
	public static void deactivateUser()
	{
	   int NumRow = 0;
	   int MinScore = 0;	 
	   int LeftRow = 0;
	   dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
		
			string sqlNAME = "SELECT USERNAME FROM PLAYER_INFO WHERE USERID = '"+ UserPlayerID +"'";
     		dbcmd.CommandText = sqlNAME;
			string Name = (string)dbcmd.ExecuteScalar();
		
		//Debug.Log("Your name is " + Name);
			for(int i = 1; i <= 20; i++)
			{
				string DeleteFile = string.Format("PicData/{0}{1}.pgm",Name,i); 
				File.Delete(DeleteFile);
			}
		
			//Delete User from PLAYER_INFO table
			string sqlDELETE = "DELETE FROM PLAYER_INFO WHERE USERID = '"+ UserPlayerID +"'";
      		dbcmd.CommandText = sqlDELETE;
       		dbcmd.ExecuteNonQuery();
			//NEWONE
			string sqlDELETEBL = "DELETE FROM BLACKLIST WHERE USERID = '"+ UserPlayerID +"'";
      		dbcmd.CommandText = sqlDELETEBL;
       		dbcmd.ExecuteNonQuery();
			//Delete the User from HIGHSCORE table (if any)
			string sqlDELETEHighScore = "DELETE FROM HIGHSCORE WHERE USERID = '"+ UserPlayerID +"'";
      		dbcmd.CommandText = sqlDELETEHighScore;
       		dbcmd.ExecuteNonQuery();
		
			//UPDATE HIGHSCORE TABLE
			//Check the Number of Rows in HIGHSCORE table
			string sqlCOUNT = "SELECT COUNT(*) FROM HIGHSCORE";
			dbcmd.CommandText = sqlCOUNT;
      		NumRow = Convert.ToInt32(dbcmd.ExecuteScalar());
      	
			LeftRow = 5-NumRow;
		
		//Update High Score Table
		if(NumRow<5)
		{
			if(NumRow > 0)
			{
			
				string sqlTOP = "SELECT USERID,BESTSCORE,LatestDATE FROM PLAYER_INFO WHERE BESTSCORE <> '' EXCEPT SELECT * FROM HIGHSCORE ORDER BY BESTSCORE DESC LIMIT "+ LeftRow +"";
       			dbcmd.CommandText = sqlTOP;
       			IDataReader reader = dbcmd.ExecuteReader();
				
       			while(reader.Read())
	  			{	
	           		 string USERID = reader.GetString (0);
					 string SCORE = reader.GetString (1);
					 DateTime DateT = reader.GetDateTime (2);
					 string DDATE = DateT.ToString("yyyy-MM-dd");
					 
					 string sqlINSERTHighScore = "INSERT INTO HIGHSCORE(USERID,SCORE,DATEDAY) VALUES('"+ USERID +"','"+ SCORE +"','"+ DDATE +"')";
					 dbcmd.CommandText = sqlINSERTHighScore;
					 dbcmd.ExecuteNonQuery();
				}
				 reader.Close();
     			 reader = null;
			}
			else if (NumRow == 0)
			{
				string sqlTOP = "SELECT USERID,BESTSCORE,LatestDATE FROM PLAYER_INFO ORDER BY BESTSCORE DESC LIMIT 5";
       			dbcmd.CommandText = sqlTOP;
       			IDataReader reader = dbcmd.ExecuteReader();
				
       			while(reader.Read())
	  			{
           		 string USERID = reader.GetString (0);
				 string SCORE = reader.GetString (1);
				 DateTime DateT = reader.GetDateTime (2);
				 string DDATE = DateT.ToString("yyyy-MM-dd");
				
				 string sqlINSERTHighScore = "INSERT INTO HIGHSCORE(USERID,SCORE,DATEDAY) VALUES('"+ USERID +"','"+ SCORE +"','"+ DDATE +"')";
				 dbcmd.CommandText = sqlINSERTHighScore;
				 dbcmd.ExecuteNonQuery();
				}
			
				 reader.Close();
       			 reader = null;
       		}
			
			
		}
			
		
		// clean up
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
		if(getNumPlayer()>0)
	   		Recognition.RemoveUSER();
	}
	

}
