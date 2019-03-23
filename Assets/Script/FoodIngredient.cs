/*
 * @author: cdtek
 * 
 * modified by: cdtek
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.DatabaseModel;
using UnityEngine;

namespace Assets.Script {
    public class FoodIngredient : IEnumerator {

        private readonly DatabaseManager databaseManager;
        private readonly Food food;

        private readonly string[] instruction;
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

                    if(words[i] == string.Empty) { // We might get white spaces that could cause error
                        continue;
                    }

                    // Check for WAIT_TIME tag to get time
                    if (words[i].Contains("WAIT_TIME")) {
                        Time = Convert.ToInt32(words[i].Split(':')[1]);

                        Debug.Log("<color=blue>" + Time + "</color> has been added");

                        // To tell whether it is time to tick
                        dictionary.Add("Time" + i.ToString(), "start");
                    }

                    // If the ingredient is not instructed to be used. Checks for tag {skip}
                    if (words[i].Contains("skip")) {
                        foreach (var item in databaseManager.GetIngredient(food.FoodId).
                            Where(x => x.RawName.StartsWith(words[i + 1].ToLower()) && x.RawName.Contains(words[i + 1].ToLower())).Take(1)) {
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
                        Where(x => x.RawName.StartsWith(words[i].ToLower()) && x.RawName.Contains(words[i].ToLower()));

                    Debug.Log("current word:" + words[i]);
                    var ingredients = collection as Ingredient[] ?? collection.ToArray();
                    foreach (var item in ingredients) {
                        Debug.Log("searched word: " + item.RawName);
                    }

                    string fullRawName;
                    if (ingredients.Length > 1) {
                        // First we have to compare the first word to each instance
                        foreach (var item in ingredients) {
                            if (item.RawName != words[i]) {
                                continue;
                            }

                            // Avoid duplication
                            if (!dictionary.ContainsKey(item.RawName)) {
                                // If the word[i] is on Blacklist skip it
                                if (exclude.Contains(words[i])) {
                                    Debug.Log("<color=red>" + words[i] + "</color> cannot be added");
                                    continue;
                                }

                                Debug.Log("<color=green>adding..." + item.RawName + "</color>");
                                dictionary.Add(item.RawName, item.Method);
                            }

                            break;
                        }

                        // If none found we should probably get the second word too
                        fullRawName = words[i] + " " + words[i + 1];
                        fullRawName = fullRawName.Replace(".", string.Empty);
                        fullRawName = fullRawName.Replace(",", string.Empty);
                        fullRawName = fullRawName.Trim();

                        Debug.Log("Getting the two words: " + fullRawName);
                        foreach (var item in databaseManager.GetIngredient(food.FoodId).
                            Where(x => x.RawName == fullRawName.ToLower())) {
                            Debug.Log("<color=green>adding..." + item.RawName + "</color>");
                            // Avoid duplication
                            if (dictionary.ContainsKey(item.RawName)) {
                                continue;
                            }

                            // If the word[i] is on Blacklist skip it
                            if (exclude.Contains(words[i])) {
                                Debug.Log("<color=red>" + words[i] + "</color> cannot be added");
                                continue;
                            }

                            dictionary.Add(item.RawName, item.Method);
                        }
                    } else {
                        // This will only retrieve one item
                        foreach (var item in ingredients) {
                            // Avoid duplication
                            if (dictionary.ContainsKey(item.RawName)) {
                                continue;
                            }

                            // If the word[i] is on Blacklist skip it
                            if (exclude.Contains(words[i].ToLower())) {
                                Debug.Log("<color=red>" + words[i] + "</color> cannot be added");
                                continue;
                            }

                            Debug.Log("<color=green>adding..." + item.RawName + "</color>");
                            dictionary.Add(item.RawName, item.Method);
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

        private void AddToDictionary(string raw) {
            // TODO
        }
    }
}
