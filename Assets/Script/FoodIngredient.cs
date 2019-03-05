/*
 * @author: cdtek
 * 
 * modified by: cdtek
 */

using Assets.Script.DatabaseModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using System;

public class FoodIngredient : IEnumerator {

    private DatabaseManager databaseManager;
    private Food food;

    private string[] instruction;
    private int position = -1;

    public int Time { get; set; }

    public FoodIngredient(Food food, string instruction) {
        this.food = food; // Let see what food we get
        this.instruction = instruction.Split('\n'); // Let see the instruction of this food... but per line

        databaseManager = new DatabaseManager(); // Yes the database... we need it to check the information about the food
    }

    public bool MoveNext() { // Will be using this to manually iterate through the instructions
        position++;
        return (position < instruction.Length);
    }

    public void Reset() {
        position = -1;
    }

    public Dictionary<string, string> Current { // Will contain image and animation
        get {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string exclude = "";
            string[] words = instruction[position].Split(' '); // We're gonna assign manually
            for (int i = 0; i < words.Length; i++) {

                // Check for WAIT_TIME tag to get time
                if (words[i].Contains("WAIT_TIME")) {
                    Time = Convert.ToInt32(words[i].Split(':')[1]);

                    // To tell whether it is time to tick
                    dictionary.Add("Time", "start");
                }

                // If the ingredient is not instructed to be used. Checks for tag {skip}
                if (words[i].Contains("skip")) {
                    foreach (var item in databaseManager.GetIngredient(food.FoodId).
                    Where(x => x.RawName.StartsWith(words[i + 1]) && x.RawName.Contains(words[i + 1])).Take(1)) {
                        // Blacklist that ingredient
                        exclude = item.RawName + " " + "{skip}";

                        Debug.Log("<color=blue>" + exclude + "</color> has been blacklisted");
                    }
                }

                // We remove the noise
                if (words[i].Contains(",")) {
                    words[i] = words[i].Split(',')[0];
                } else if (words[i].Contains(".")) {
                    words[i] = words[i].Split('.')[0];
                }

                // This will only retrieve one item
                foreach (var item in databaseManager.GetIngredient(food.FoodId).
                    Where(x => x.RawName.StartsWith(words[i]) && x.RawName.Contains(words[i])).Take(1)) {

                    // Avoid duplication
                    if (!dictionary.ContainsKey(item.RawName)) {
                        // If the word[i] is on Blacklist skip it
                        if (exclude.Contains(words[i])) {
                            Debug.Log("<color=red>" + words[i] + "</color> cannot be added");
                            continue;
                        } else {
                            dictionary.Add(item.RawName, item.Method);
                        }
                    }
                }
            }

            return dictionary;
        }
    }

    object IEnumerator.Current {
        get {
            return Current;
        }
    }
}
