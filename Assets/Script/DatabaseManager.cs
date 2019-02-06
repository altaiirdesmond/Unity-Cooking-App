using System.Collections.Generic;
using UnityEngine;
using Assets.Script.DatabaseModel;
using System.IO;
using SQLite4Unity3d;

public class DatabaseManager {

    private static SQLiteConnection connection;
    private readonly string database = "FoodDb.db";

    public DatabaseManager() {
#if UNITY_EDITOR
        // Set database path to StreamingAssets folder
        var filepath = string.Format(@"Assets/StreamingAssets/{0}", database);
#else
        // Set database path to persistentDataPath within the android device
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, database);
#endif
#if UNITY_ANDROID
        // Check for database file in the persistentDataPath within the android device
        if (!File.Exists(filepath)) {
            // Open StreamingAssets directory and load the db
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + database);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone);

            File.WriteAllBytes(filepath, loadDb.bytes);
        }
#endif
        // Set connection to database
        connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    public IEnumerable<Food> GetFoods() {
        return connection.Table<Food>();
    }

    public IEnumerable<Food> GetFood(string foodName) {
        return connection.Table<Food>().Where(i => i.FoodName.Equals(foodName));
    }

    public IEnumerable<Food> GetFoodsByCategory(string category) {
        return connection.Table<Food>().Where(i => i.Category.Equals(category));
    }
}