using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Mono.Data;
using Mono.Data.Sqlite;
using System.Data.SqlClient;


/// <summary>
/// Writes text in a dialog box.
/// </summary>
[CommandInfo("Database",
             "DeductFromAirplane",
             "Deducts From Airplane DB")]
public class DeductFromAirplane : Command {

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
            string query = "INSERT into question (Qscore, level) values (0, 1)";
            sqlDB.ExecuteNonQuery(query);

            dt = sqlDB.ExecuteQuery(query);


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
