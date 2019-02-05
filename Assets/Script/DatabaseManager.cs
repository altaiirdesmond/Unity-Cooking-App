using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* For SQLite namespaces */
using Mono.Data.Sqlite;
using System.Data;
using System;

public class DatabaseManager : MonoBehaviour {
    private void Start() {
        string conn = "URI=file:" + Application.dataPath + "/FoodDb.db";
        IDbConnection dbConnection = new SqliteConnection(conn);
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = "SELECT * FROM FoodTable";
        IDataReader dataReader = dbCommand.ExecuteReader();

        while (dataReader.Read()) {
            Debug.Log("id" + dataReader.GetInt32(0) + "\n" +
                "foodname" + dataReader.GetString(1));
        }

        dataReader.Close();
        dbCommand.Dispose();
        dbConnection.Close();
    }
}
