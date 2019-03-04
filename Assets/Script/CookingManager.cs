using Assets.Script.DatabaseModel;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CookingManager : MonoBehaviour {

    [SerializeField] private Animator cookingAnimator;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private SpriteRenderer rawImage;
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI trivia;
    [SerializeField] private TextMeshProUGUI finish;

    private TextToSpeechManager speechManager;
    private Food food;
    private FoodIngredient foodIngredient;
    private bool stop = false;
    private int temp = 0;

    public string Trivia { get; set; }

    private void Start() {
        food = MenuManager.Food;

        // Ready the ingredients after knowing the instruction
        // Pass only translated instruction. Tagalog not supported
        foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

        // ...
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            trivia.SetText(food.TriviaTranslated);
        } else {
            trivia.SetText(food.Trivia);
        }
            
        // We will subscribe to an event to check of the language has been changed
        Lean.Localization.LeanLocalization.OnLocalizationChanged += CurrentLanguage;

        speechManager = FindObjectOfType<TextToSpeechManager>();
        speechManager.Init(food.FoodName);

        StartCoroutine(Cook());
    }

    // Subscriber
    public void CurrentLanguage() {
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            trivia.SetText(food.TriviaTranslated);

            // Clean instruction for every language change
            string cleanedInstruction = string.Empty;
            foreach (var item in food.InstructionTranslated.Split('\n')) {
                string newText = item.Replace("{skip}", string.Empty);
                if (item.IndexOf("WAIT_TIME") != -1) {
                    int i = item.IndexOf("WAIT_TIME");
                    newText = newText.Remove(i, 11);
                }
                cleanedInstruction += newText + "\n";
            }

            instruction.SetText(cleanedInstruction);
        } else {
            trivia.SetText(food.Trivia);
            instruction.SetText(food.Instruction);
        }
    }

    private IEnumerator Cook() {
        while (foodIngredient.MoveNext()) { // We will move to the next instruction
            temp = speechManager.ClipsForCurrentInstruction(speechManager.ClipName);

            FindObjectOfType<AudioManager>().BackgroundTheme(0.2f);

            var contents = foodIngredient.Current;
            foreach (var content in contents) { // It contains all existing(non-skip) ingredient within an instruction
                Debug.Log("<color=orange>Changing index</color>");
                speechManager.Play(speechManager.NextClip);

                while (speechManager.ClipMaxLength > 0) {
                    while (stop) {
                        yield return null;
                    }

                    speechManager.ClipMaxLength -= Time.deltaTime;
                    //Debug.Log("<color=Blue>" + speechManager.ClipMaxLength + "</color>");
                    FindObjectOfType<AudioManager>().BackgroundTheme(0.1f);
                    //Debug.Log("<color=green>Playing</color>");
                    yield return null;
                }

                Debug.Log(content.Key + "," + content.Value);
                // Get image from Resources folder
                Texture2D texture2D = Resources.Load("Ingredients/" + content.Key) as Texture2D;
                texture2D.LoadImage(texture2D.EncodeToPNG());
                rawImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2());
                // Get animation key
                cookingAnimator.SetTrigger(content.Value);
                // I don't know what happen but it worked... able to pause
                yield return new WaitForSeconds(1f);
                yield return new WaitUntil(() => cookingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                while (stop) {
                    // yield null when using while loops within coroutine to prevent freezing
                    yield return null;
                }

                temp--;
            }

            // If still has clips to play for an instruction
            while(temp > 0) {
                speechManager.Play(speechManager.NextClip);
                while (speechManager.ClipMaxLength > 0) {
                    while (stop) {
                        yield return null;
                    }

                    speechManager.ClipMaxLength -= Time.deltaTime;
                    FindObjectOfType<AudioManager>().BackgroundTheme(0.1f);
                    yield return null;
                }

                temp--;
            }

            if(foodIngredient.Time > 0) { // If a time has been set in an instruction
                Debug.Log("<color=green>Starting time</color>");
                FindObjectOfType<AudioManager>().TimerStartSFXPlay();
                timer.Until = foodIngredient.Time;
                foodIngredient.Time = 0; // Clear the time from foodIngredient
                timer.Start = true;
                while (!timer.LimitReached) {
                    if (timer.HalfWay) {
                        // Show trivia by script
                        FindObjectOfType<CategorySceneManager>()
                            .Panels[7]
                            .GetComponent<UIAnimation>()
                            .Animator
                            .SetBool("show", true);
                        FindObjectOfType<AudioManager>().TriviaSFXPlay();
                    }
                    while (stop) {
                        // Pause timer
                        if (timer.Start) {
                            timer.Start = false;
                        }

                        yield return null;
                    }

                    timer.Start = true;
                    yield return null;
                }

                FindObjectOfType<AudioManager>().TimerDoneSFXPlay();
            }
        }
        // We'll gonna use the same sfx of trivia on done cooking
        FindObjectOfType<AudioManager>().TriviaSFXPlay();

        // Show "COOKING DONE!" text
        finish.GetComponent<Animator>().SetBool("done", true);

        yield return new WaitForSeconds(3f);

        finish.GetComponent<Animator>().SetBool("done", false);

        yield return null;
    }

    public void StopAnimation() {
        stop = true;
        cookingAnimator.speed = 0f; // This will pause the animation
        FindObjectOfType<AudioManager>().BackgroundTheme().Pause();
    }

    public void PlayAnimation() {
        stop = false;
        cookingAnimator.speed = 1f;
        FindObjectOfType<AudioManager>().BackgroundTheme().Play();
    }
}