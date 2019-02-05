using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* For SQLite namespaces */
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Script.DatabaseModel;

public class DatabaseManager {

    private readonly string conString = "URI=file:" + Application.dataPath + "/FoodDb.db";

    private List<Food> result;

    public DatabaseManager(string query, string[] values) {
        // Get parameters within the query
        string[] param = new string[values.Length];
        int idx = 0;
        foreach (var item in query.Split(' ')) {
            if (item.StartsWith("@")) {
                param[idx++] = item;
            }
        }

        // Set connection to database
        using (SqliteConnection conn = new SqliteConnection(conString)) {
            // Open connection
            conn.Open();

            SqliteCommand command = new SqliteCommand(query, conn);
            SqliteParameter[] p = new SqliteParameter[param.Length];

            for (int i = 0; i < p.Length; i++) {
                Debug.Log(param[i]);
                // Set parameter properties
                p[i] = command.CreateParameter();
                p[i].ParameterName = param[i];
                p[i].Value = values[i];

                // Add the parameter
                command.Parameters.Add(p[i]);
            }

            SqliteDataReader dataReader = command.ExecuteReader();
            result = new List<Food>();
            while (dataReader.Read()) {
                result.Add(new Food() {
                    FoodId = dataReader.GetInt32(0),
                    FoodName = dataReader.GetString(1),
                    Region = dataReader.GetString(2),
                    RecipeId = dataReader.GetInt32(3),
                    Category = dataReader.GetString(4),
                    Trivia = dataReader.GetString(5)
                });
            }
        }
    }

    // Get collection
    public IEnumerable<Food> GetAll() {
        return result;
    }

    // Get specific
    public Food GetOne(string foodName) {
        Food tmpFood = new Food();
        foreach (var item in result) {
            if(item.FoodName == foodName) {
                tmpFood = item;
            }
        }

        return tmpFood;
    }
}
