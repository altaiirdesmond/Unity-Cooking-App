using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodIngredient : MonoBehaviour {

    /// <summary>
    /// Listed ingredients of the food by ParseIngredient
    /// </summary>
    public List<string[]> IngredientList { get; set; }

    /// <summary>
    /// This resolves every line of Ingredient to parse the quantity and required raw material of the Food
    /// </summary>
    /// <param name="foodName">The food name</param>
    public void SetFoodName(string foodName) {
        DatabaseManager databaseManager = new DatabaseManager();
        string foodIngredient = databaseManager.GetFood(foodName).Take(1).First().IngredientsTranslated;
        // Get ingredients per line
        IngredientList = new List<string[]>();
        foreach (var item in foodIngredient.Split('\n')) {
            //Debug.Log(item);
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

            IngredientList.Add(new string[] { qty, container, raw, time });
        }

        //foreach (var item in IngredientList) {
        //    Debug.Log(item[0] + "," + item[1] + "," + item[2]);
        //}
    }
    
    public void SetCookingInstruction(IEnumerable<string> instructionList) {
        for (int i = 0; i < instructionList.Count(); i++) {
            Debug.Log(instructionList.ElementAt(i));
            foreach (var ingredient in IngredientList) {
                string firstWord, secondWord;
                if (ingredient[2].Contains(' ')) {
                    firstWord = ingredient[2].Split(' ')[0];
                    secondWord = ingredient[2].Split(' ')[1];

                    if (instructionList.ElementAt(i).Contains(firstWord) && instructionList.ElementAt(i).Contains(secondWord)) {
                        Debug.Log("We need " + firstWord + " " + secondWord);
                    }
                } else {
                    if (instructionList.ElementAt(i).Contains(ingredient[2])) {
                        Debug.Log("We need " + ingredient[2]);
                    }
                }
            }
        }
    }
}
