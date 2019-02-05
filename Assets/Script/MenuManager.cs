using UnityEngine;
using UnityEngine.SceneManagement;
/* For SQLite namespaces */
using Mono.Data.Sqlite;
using System.Data;
using System;
using Assets.Script.DatabaseModel;

public class MenuManager : MonoBehaviour {

    public void GoToSetting() {
        SceneManager.LoadScene("Setting");
    }

    public void SetLanguage(string language) {
        
    }

    public void ApplySetting() {
        SceneManager.LoadScene("Main menu");
    }

    public void GotoCategories() {
        SceneManager.LoadScene("Categories");
    }

    public void PopulateCategoryResult(string category) {
        DatabaseManager databaseManager = new DatabaseManager(
            "SELECT * FROM FoodTable WHERE Category = @category", 
            new string[] { category });

        foreach (var item in databaseManager.GetAll()) {
            Debug.Log(item.FoodName);
        }
    }
}
