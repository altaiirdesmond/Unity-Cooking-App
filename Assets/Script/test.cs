using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class test : MonoBehaviour {
    
    private List<string[]> ingredientList;

    private void Start() {
        DatabaseManager databaseManager = new DatabaseManager();
        string foodIngredient = databaseManager.GetFood("ginataang yapyap").Take(1).First().IngredientsTranslated;
        // Get ingredients per line
        ingredientList = new List<string[]>();
        foreach (var item in foodIngredient.Split('\n')) {
            string qty = "", container = "", raw = "", time = "";
            foreach (var word in item.Split(' ')) {
                if(word.Contains("]")) {
                    var w = word.Split(',')[0].ToCharArray();
                    foreach (var i in w) {
                        if (char.IsDigit(i) || i == '.' || i == '/') {
                            // Quantity
                            qty += i.ToString();
                        } else if (i == '&') {
                            qty += ' ';
                        }
                    }

                    var x = word.Split(',')[1].ToCharArray();
                    foreach (var i in x) {
                        if (char.IsLetter(i)) {
                            // Container
                            container += i;
                        } else if (i == '_') {
                            container += ' ';
                        } 
                    }
                } else if (word.Contains(">")) {
                    var w = word.ToCharArray();
                    foreach (var i in w) {
                        if (char.IsLetter(i)) {
                            // Raw
                            raw += i;
                        } else if (i == '_') {
                            raw += ' ';
                        }
                    }
                } else if (word.Contains("}")) {
                    var z = word.ToCharArray();
                    foreach (var i in z) {
                        if (i != '{' || i != '}') {
                            // Time
                            time += i;
                        }
                    }
                }
            }

            ingredientList.Add(new string[] { qty, container, raw, time });
        }

        foreach (var item in ingredientList) {
            Debug.Log(item[0] + "," + item[1] + "," + item[2]);
        }
    }
}
