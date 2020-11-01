using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using Mono.Data.SqliteClient;

public class FatigueData{
	
	//the path for connecting database
	static string connectionString = "URI=file://sqlite/ProjAvatarDB.db";
    static IDbConnection dbcon;
	
	public static void GenMapPosition()
	{

		dbcon = (IDbConnection) new SqliteConnection(connectionString);
        dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		for(int i = 0; i < GenMap.MapList.Count; i++)
		{
			string sqlGenMapCOLLECT = "INSERT INTO MAPPOSITION (USERID,MAP,PositionX,PositionZ,TIMERGEN) VALUES('"+ ProjAvatarDB.UserPlayerID +"','"+ GenMap.MapList[i].MapName +"', '"+ GenMap.MapList[i].posX +"','"+ GenMap.MapList[i].posZ +"','"+ GenMap.MapList[i].EggTime +"')";
      		dbcmd.CommandText = sqlGenMapCOLLECT;
       		dbcmd.ExecuteNonQuery();
		}
		// clean up
       	dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
	}
	
	public static void CollectEggData()
	{
		
			
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
        dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		int CountEggList = EggCollision.EggList.Count;
		
		for(int i =0; i < CountEggList; i++)
		{
		
			//string sqlGenMapCOLLECT = "INSERT INTO EGGCOLLECTION (USERID, PositionX, PositionY, PositionZ) VALUES('"+ ProjAvatarDB.UserPlayerID +"','"+ PosX + "','"+ PosY +"','"+ PosZ +"')";
      		string sqlGenMapCOLLECT = "INSERT INTO EGGCOLLECTION (USERID, PositionX, PositionY, PositionZ, TIMERGEN) VALUES('"+ ProjAvatarDB.UserPlayerID +"','"+ EggCollision.EggList[i].posX + "','"+ EggCollision.EggList[i].posY +"','"+ EggCollision.EggList[i].posZ +"','" + EggCollision.EggList[i].EggTime +"')";
			dbcmd.CommandText = sqlGenMapCOLLECT;
       		dbcmd.ExecuteNonQuery();
		
		}
		// clean up
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
		
	}
	
}
