using Assets.Script.DatabaseModel;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CookingManager : MonoBehaviour {

    [SerializeField] private Animator cookingAnimator;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private SpriteRenderer rawImage;
    [SerializeField] private TextToSpeechManager speechManager;
    [SerializeField] private Timer timer;

    private Food food;
    private FoodIngredient foodIngredient;
    private bool stop = false;

    public string Trivia { get; set; }

    private void Start() {
        food = MenuManager.Food;

        // Ready the ingredients after knowing the instruction
        // Pass only translated instruction. Tagalog not supported
        foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

        // Ready trivia for Trivia panel to fetch
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            Trivia = food.TriviaTranslated;
        } else {
            Trivia = food.Trivia;
        }
            
        // We will subscribe to an event to check of the language has been changed
        // For reference:
        // https://www.intertech.com/Blog/c-sharp-tutorial-understanding-c-events/
        Lean.Localization.LeanLocalization.OnLocalizationChanged += CurrentLanguage;

        speechManager = FindObjectOfType<TextToSpeechManager>();
        speechManager.Init(food.InstructionTranslated.Split('\n').Length, food.FoodName);
        speechManager.Play();

        StartCoroutine(Cook());
    }

    // Subscriber
    public void CurrentLanguage() {
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            Trivia = food.TriviaTranslated;

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
            Trivia = food.Trivia;
            instruction.SetText(food.Instruction);
        }
    }

    private IEnumerator Cook() {
        while (foodIngredient.MoveNext()) { // We will move to the next instruction
            while (speechManager.IsPlaying) {
                FindObjectOfType<AudioManager>().BackgroundClipAudioSource.volume = 0.05f;
                Debug.Log("<color=green>Playing</color>");
                yield return null;
            }

            FindObjectOfType<AudioManager>().BackgroundClipAudioSource.volume = 0.2f;

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
                    // yield null when using while loops within coroutine to prevent freezing
                    yield return null;
                }
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
                            .Panels[8]
                            .GetComponent<UIAnimation>()
                            .Animator
                            .SetBool("show", true);
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

            Debug.Log("<color=orange>Changing index</color>");
            speechManager.Play();
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