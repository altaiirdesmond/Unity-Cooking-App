using Assets.Script.DatabaseModel;
using System.Collections;
using TMPro;
using UnityEngine;

public class CookingManager : MonoBehaviour {

    [SerializeField] private Animator cookingAnimator;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private SpriteRenderer rawImage;

    private Food food;
    private FoodIngredient foodIngredient;
    private bool stop = false;

    private void Start() {
        food = MenuManager.Food;

        // Ready the ingredients after knowing the instruction
        // Pass only translated instruction. Tagalog not supported
        foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

        // We will subscribe to an event to check of the language has been changed
        // For reference:
        // https://www.intertech.com/Blog/c-sharp-tutorial-understanding-c-events/
        Lean.Localization.LeanLocalization.OnLocalizationChanged += CurrentLanguage;

        StartCoroutine(Cook());
    }

    // Subscriber
    public void CurrentLanguage() {
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            instruction.SetText(food.InstructionTranslated);
        } else {
            instruction.SetText(food.Instruction);
        }
    }

    private IEnumerator Cook() {
        while (foodIngredient.MoveNext()) { // We will move to the next instruction
            var contents = foodIngredient.Current;
            foreach (var content in contents) {
                Debug.Log(content.Key[0] + "," + content.Key[1]);
                // Get image from Resources folder
                Texture2D texture2D = Resources.Load("Ingredients/" + content.Key[0]) as Texture2D;
                texture2D.LoadImage(texture2D.EncodeToPNG());
                rawImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2());
                // Get animation key
                cookingAnimator.SetTrigger(content.Key[1]);
                // I don't know what happen but it worked... able to pause
                yield return new WaitForSeconds(1f);
                yield return new WaitUntil(() => cookingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                while (stop) {
                    yield return null;
                }
            }
        }
        yield return null;
    }

    public void StopAnimation() {
        stop = true;
        cookingAnimator.speed = 0f; // This will pause the animation
    }

    public void PlayAnimation() {
        stop = false;
        cookingAnimator.speed = 1f;
    }

    public void PlayBackground() {
        FindObjectOfType<AudioManager>().Playbackground(true);
    }

    public void StopBackground() {
        FindObjectOfType<AudioManager>().Playbackground(false);
    }
}
