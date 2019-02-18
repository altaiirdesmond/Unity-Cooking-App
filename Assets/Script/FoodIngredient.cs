/*
 * @author: cdtek
 * 
 * modified by: cdtek
 */

using Assets.Script.DatabaseModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class FoodIngredient : IEnumerator {

    private DatabaseManager databaseManager;
    private Food food;

    private string[] instruction;
    private int position = -1;

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

    public Dictionary<string[], byte[]> Current { // Will contain image and animation
        get {
            Dictionary<string[], byte[]> dictionary = new Dictionary<string[], byte[]>();
            string[] words = instruction[position].Split(' '); // We're gonna assign manually
            for (int i = 0; i < words.Length; i++) {
                // We remove the noise
                if(words[i].Contains(",")) {
                    words[i] = words[i].Split(',')[0];
                } else if (words[i].Contains(".")) {
                    words[i] = words[i].Split('.')[0];
                }

                // This will only retrieve one item
                foreach (var item in databaseManager.GetIngredient(food.FoodId).
                    Where(x => x.RawName.StartsWith(words[i]) && x.RawName.Contains(words[i])).Take(1)) {

                    // Avoid duplication
                    if(!dictionary.ContainsKey(new string[] { item.RawName, item.Method })) {
                        // We get the image of that ingredient
                        byte[] rawImage = null;
                        foreach (var raw in databaseManager.GetRawByName(item.RawName)) {
                            rawImage = raw.Image;
                        }

                        dictionary.Add(new string[] { item.RawName, item.Method }, rawImage);
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
