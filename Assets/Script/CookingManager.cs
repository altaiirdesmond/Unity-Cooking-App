using Assets.Script.DatabaseModel;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour {

    [SerializeField] private Animator cookingAnimator;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private SpriteRenderer rawImage;

    private Food food;
    private FoodIngredient foodIngredient;

    private void Start() {
        food = MenuManager.Food;

        // Ready the ingredients after knowing the instruction
        // Pass only translated instruction. Tagalog not supported
        foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

        StartCoroutine(Cook());
    }

    private IEnumerator Cook() {
        while (foodIngredient.MoveNext()) {
            var contents = foodIngredient.Current;
            foreach (var content in contents) {
                Debug.Log(content.Key[0] + "," + content.Key[1]);
                // Get image from Resources folder
                Texture2D texture2D = Resources.Load("Ingredients/" + content.Key[0]) as Texture2D;
                texture2D.LoadImage(texture2D.EncodeToPNG());
                rawImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2());
                // Get animation key
                cookingAnimator.SetTrigger(content.Key[1]);
                yield return new WaitForSeconds(5f);
            }
        }
        yield return null;
    }
}
