using Assets.Script.DatabaseModel;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using System.Collections;

public class FoodIngredient : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteRenderer;
    private DatabaseManager databaseManager;
    private string foodName;
    private bool cook = false;

    public List<string[]> IngredientList { get; set; }

    private void Start() {
        //StartCoroutine(DoCook());
    }

    public void SetFoodName(string foodName) {
        this.foodName = foodName;

        databaseManager = new DatabaseManager();
        string foodIngredient = databaseManager.GetFood(foodName).Take(1).First().IngredientsTranslated;
        // Get ingredients per line. IMPROVE THIS LATER
        IngredientList = new List<string[]>();
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

            IngredientList.Add(new string[] { qty, container, raw, time });
        }

        foreach (var item in IngredientList) {
            Debug.Log(item[0] + " " + item[1] + " of " + item[2]);
        }
    }
    
    public void SetCookingInstruction(IEnumerable<string> instructionList) {
        for (int i = 0; i < instructionList.Count(); i++) {
            foreach (var ingredient in IngredientList) {
                Debug.Log("we have " + ingredient[2]);
                string firstWord = "", secondWord = "", thirdWord = "";
                if (ingredient[2].Contains(' ')) {
                    if (ingredient[2].Split(' ').Length == 3) {
                        firstWord = ingredient[2].Split(' ')[0];
                        secondWord = ingredient[2].Split(' ')[1];
                        thirdWord = ingredient[2].Split(' ')[2];

                        if (instructionList.ElementAt(i).Contains(firstWord) &&
                            instructionList.ElementAt(i).Contains(secondWord) &&
                            instructionList.ElementAt(i).Contains(thirdWord)) {
                            Debug.Log("we need " + firstWord + " " + secondWord + " " + thirdWord);
                        }
                    } else if (ingredient[2].Split(' ').Length == 2) {
                        firstWord = ingredient[2].Split(' ')[0];
                        secondWord = ingredient[2].Split(' ')[1];
                        if (instructionList.ElementAt(i).Contains(firstWord) &&
                            instructionList.ElementAt(i).Contains(secondWord)) {
                            Debug.Log("we need " + firstWord + " " + secondWord);

                            // Reference for database image(blob) file
                            //Texture2D texture2D = new Texture2D(2, 2);

                            //byte[] rawImage = null;
                            //foreach (var item in databaseManager.GetRawByName(firstWord + " " + secondWord)) {
                            //    rawImage = item.Image;
                            //}
                            //// Load retrieved image(byte)
                            //texture2D.LoadImage(rawImage);
                            //// Set image as sprite for each prefab. Canvas/CategoryResult_Panel/Result/
                            //GameObject.Find("Ingredient/Image").GetComponent<SpriteRenderer>().sprite = 
                            //    Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2());

                            //GameObject.Find("Ingredient/Image").GetComponent<Animator>().SetTrigger("pour"); // Play animation
                        }
                    } else if(ingredient[2].Split(' ').Length == -1) {
                        if (instructionList.ElementAt(i).Contains(ingredient[2])) {
                            Debug.Log("we need " + ingredient[2]);
                        }
                    }
                }
            }
        }
    }

    private void Update() {
        // Check animation state
        //if (GameObject.Find("Ingredient/Image").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pour")) {
        //    Debug.Log("pouring");
        //} else {
        //    Debug.Log("idle");
        //}
    }

    //private IEnumerator DoCook() {
    //    yield return new WaitUntil(() => GameObject.Find("Ingredient/Image").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
    //}
}
