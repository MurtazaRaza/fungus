using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Mono.Data;
using Mono.Data.Sqlite;
using System.Data.SqlClient;



[CommandInfo("Database",
             "InitializeAirplane",
             "Initializes Airplane DB")]
public class InitializePlaneDB : Command
{

    public override void OnEnter()
    {
        try
        {

            //Notes:
            //###to execute a query this libs have 2 simple methods:

            //void ExecuteNonQuery(string query)  //for SQL query like UPDATE, DELETE....
            //DataTable ExecuteQuery(string query)  //for SQL query like SELECT ....
            //We only have to work with Execute Non query because it has insert statement


            //Dictionary<int,string> badguys = new Dictionary<int,string>();
            SqliteDatabase sqlDB = new SqliteDatabase("vrlingo.DB");
            DataTable dt = new DataTable();

            //string query = @"select * from user;";

            string query = "delete from level where levelname = 'Airplane'";
            sqlDB.ExecuteNonQuery(query);
           // query = "truncate table level";
            //sqlDB.ExecuteNonQuery(query);
            query = "INSERT into level (levelname, cuser) values ('Airplane','saad@gmail.com')";
            sqlDB.ExecuteNonQuery(query);

            //dt = sqlDB.ExecuteQuery(query);

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            Continue();
        }

    }
}
