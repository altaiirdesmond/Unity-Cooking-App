using Assets.Script.DatabaseModel;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour {
    [Header("This will contain all the action within the cooking session")]
    [SerializeField] private Animator cookingAnimator;
    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private SpriteRenderer rawImage;
    [SerializeField] private SpriteRenderer plateImage;
    [SerializeField] private Transform bowlImage;
    [SerializeField] private Transform potImage;
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI trivia;
    [SerializeField] private TextMeshProUGUI finish;
    [SerializeField] private GameObject foodResultName;
    [SerializeField] private GameObject foodResult;
    [SerializeField] private GameObject foodResultBackPanel;

    private TextToSpeechManager speechManager;
    private Food food;
    private FoodIngredient foodIngredient;
    private bool stop = false;
    private int clipCountForCurInstruction = 0;

    public string Trivia { get; set; }

    private void Start() {
        food = MenuManager.Food;

        if(food.FoodName == "kinilaw na guso") {
            potImage.gameObject.SetActive(false);
            bowlImage.gameObject.SetActive(true);
        } else {
            potImage.gameObject.SetActive(true);
            bowlImage.gameObject.SetActive(false);
        }

        // Ready the ingredients after knowing the instruction
        // Pass only translated instruction. Tagalog not supported
        foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

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

            // Clean instruction before starting CookingScene
            string cleanedInstruction = string.Empty;
            foreach (var item in food.InstructionTranslated.Split('\n')) {
                string newText = item.Replace("{skip}", string.Empty);
                foreach (var waitTime in newText.Split(' ').ToList().Where(i => i.StartsWith("WAIT_TIME:"))) {
                    newText = newText.Replace(waitTime, string.Empty);
                }

                cleanedInstruction += newText + "\n";
            }

            instruction.SetText(cleanedInstruction);
        } else {
            trivia.SetText(food.Trivia);
            instruction.SetText(food.Instruction);
        }
    }

    // TODO: Compress this function
    private IEnumerator Cook() {
        while (foodIngredient.MoveNext()) { // We will move to the next instruction
            clipCountForCurInstruction = speechManager.ClipsForCurrentInstruction(speechManager.ClipName);

            FindObjectOfType<AudioManager>().BackgroundTheme(0.2f);

            var contents = foodIngredient.Current; // It contains all existing(non-skip) ingredient within an instruction
            foreach (var content in contents) {

                if (content.Key.Contains("Time") && clipCountForCurInstruction == 1) {

                    // Play clip
                    speechManager.Play(speechManager.NextClip);

                    while (speechManager.ClipMaxLength > 0) {
                        while (stop) {
                            yield return null;
                        }

                        speechManager.ClipMaxLength -= Time.deltaTime;
                        FindObjectOfType<AudioManager>().BackgroundTheme(0.1f);
                        yield return null;
                    }

                    clipCountForCurInstruction--;

                    // And since time is only the content immediately start timer
                    FindObjectOfType<AudioManager>().TimerStartSFXPlay();
                    timer.Until = foodIngredient.Time;
                    Debug.Log("<color=green>Starting time and until " + timer.Until + "</color>");
                    foodIngredient.Time = 0; // Clear the time from foodIngredient
                    timer.Start = true;

                    while (!timer.NormalLimitReached) {
                        if (timer.Min == timer.Until - 1 && timer.Sec == 0) {
                            // Start animation of timer
                            timer.GetComponent<Animator>().SetTrigger("show");
                        }

                        if (timer.TriviaLimit) {
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

                    // There's nothing else in the content just string no image so continue to the next iteration
                    break;
                }

                // If the content key contains Time
                if (content.Key.Contains("Time")) {
                    FindObjectOfType<AudioManager>().TimerStartSFXPlay();
                    timer.Until = foodIngredient.Time;
                    Debug.Log("<color=green>Starting time and until " + timer.Until + "</color>");
                    foodIngredient.Time = 0; // Clear the time from foodIngredient
                    timer.Start = true;

                    while (!timer.NormalLimitReached) {
                        if (timer.Min == timer.Until - 1 && timer.Sec == 0) {
                            // Start animation of timer
                            timer.GetComponent<Animator>().SetTrigger("show");
                        }

                        if (timer.TriviaLimit) { // Listen to a random limit if the WAIT_TIME is more than 3 mins
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

                    // Hide trivia by script
                    FindObjectOfType<CategorySceneManager>()
                        .Panels[7]
                        .GetComponent<UIAnimation>()
                        .Animator
                        .SetBool("show", false);

                    FindObjectOfType<AudioManager>().TimerDoneSFXPlay();

                    // There's nothing else in the content just string no image so continue to the next iteration
                    continue;
                } else {
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
                    rawImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.2f));
                    // Get plate image from Resources folder
                    texture2D = Resources.Load("Ingredients/plate_image") as Texture2D;
                    plateImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.1f));

                    // Get animation key
                    cookingAnimator.SetTrigger(content.Value);
                    // I don't know what happen but it worked... able to pause
                    yield return new WaitForSeconds(1f);
                    yield return new WaitUntil(() => cookingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                    while (stop) {
                        // yield null when using while loops within coroutine to prevent freezing
                        yield return null;
                    }

                    clipCountForCurInstruction--;
                }
            }

            // If still has clips to play for an instruction
            while (clipCountForCurInstruction > 0) {
                speechManager.Play(speechManager.NextClip);
                while (speechManager.ClipMaxLength > 0) {
                    while (stop) {
                        yield return null;
                    }

                    speechManager.ClipMaxLength -= Time.deltaTime;
                    FindObjectOfType<AudioManager>().BackgroundTheme(0.1f);
                    yield return null;
                }

                clipCountForCurInstruction--;
            }
        }
        // We'll gonna use the same sfx of trivia on done cooking
        FindObjectOfType<AudioManager>().TriviaSFXPlay();
        // Show "COOKING DONE!" text
        finish.GetComponent<Animator>().SetBool("done", true);
        // Enable white background
        foodResultBackPanel.SetActive(true);
        // Enable foodresult gameobject first
        foodResult.SetActive(true);
        // Enable foodname gameobject first
        foodResultName.SetActive(true);
        // Set what food name to be displayed
        foodResultName.GetComponent<TextMeshProUGUI>().SetText(food.FoodName);
        // Get the food sprite to use by the gameobject
        Texture2D foodSprite = Resources.Load("FoodResult/" + food.FoodName) as Texture2D;
        foodResult.GetComponent<Image>().sprite = Sprite.Create(foodSprite, new Rect(0, 0, foodSprite.width, foodSprite.height), new Vector2(0.5f, 0.1f));

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