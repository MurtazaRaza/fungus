using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Mono.Data;
using Mono.Data.Sqlite;
using System.Data.SqlClient;



[CommandInfo("Database",
             "EndAirplane",
             "End Airplane DB")]
public class EndAccuracyAirplane : Command
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

            string query = @"select sum(qscore)/CAST(max(id) as float) * 100 as coun from Question where level = 1;";

           

            dt = sqlDB.ExecuteQuery(query);

            foreach (DataRow dr in dt.Rows)
            {
                Debug.Log(dr["coun"].ToString());
            }

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
