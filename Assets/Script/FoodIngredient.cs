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
        return (position <= instruction.Length - 1);
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

                // We clean the word
                if (words[i].Contains(",")) {
                    words[i] = words[i].Split(',')[0];
                } else if (words[i].Contains(".")) {
                    words[i] = words[i].Split('.')[0];
                }

                // Get the instances of the word in the ingredient
                var collection = databaseManager.GetIngredient(food.FoodId).
                    Where(x => x.RawName.StartsWith(words[i]) && x.RawName.Contains(words[i]));

                Debug.Log("current word:" + words[i]);
                foreach (var item in collection) {
                    Debug.Log("searched word: " + item.RawName);
                }

                // We are checking for multiple instance of that ingredient with 
                // word that starts with word[i]
                if(collection.Count() > 1) {
                    // If there are two or more instances found from the database
                    // then we should get not only the first
                    // word but the whole word to be accurate on what we are getting in the 
                    // database
                    string fullRawName = words[i] + " " + words[i + 1];
                    fullRawName = fullRawName.Replace(".", string.Empty);
                    fullRawName = fullRawName.Trim();

                    Debug.Log("Getting the two words: " + fullRawName);

                    foreach (var item in databaseManager.GetIngredient(food.FoodId).
                        Where(x => x.RawName == fullRawName)) {
                        Debug.Log("<color=green>adding..." + item.RawName + "</color>");
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
                } else {
                    // This will only retrieve one item
                    foreach (var item in collection) {
                        // Avoid duplication
                        if (!dictionary.ContainsKey(item.RawName)) {
                            // If the word[i] is on Blacklist skip it
                            if (exclude.Contains(words[i])) {
                                Debug.Log("<color=red>" + words[i] + "</color> cannot be added");
                                continue;
                            } else {
                                Debug.Log(words[i] + "has been added");
                                dictionary.Add(item.RawName, item.Method);
                            }
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
