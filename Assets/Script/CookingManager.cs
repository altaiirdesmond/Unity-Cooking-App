using System.Collections;
using System.Linq;
using Assets.Script.DatabaseModel;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script {
    public class CookingManager : MonoBehaviour {
#pragma warning disable 649
        [Header("This will contain all the action within the cooking session")]
        [SerializeField] private Transform bowlImage;
        [SerializeField] private GameObject chickenAdobo;
        [SerializeField] private GameObject bananaLeaf;
        [SerializeField] private Animator cookingAnimator;
        [SerializeField] private TextMeshProUGUI finish;
        [SerializeField] private GameObject foodResult;
        [SerializeField] private GameObject foodResultBackPanel;
        [SerializeField] private GameObject foodResultName;
        [SerializeField] private TextMeshProUGUI instruction;
        [SerializeField] private GameObject palitaw;
        [SerializeField] private Transform panImage;
        [SerializeField] private SpriteRenderer plateImage;
        [SerializeField] private Transform potImage;
        [SerializeField] private SpriteRenderer rawImage;
        [SerializeField] private Transform fryPanImage;
        [SerializeField] private Transform jarImage;
        [SerializeField] private Transform leafImage;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI trivia;

        private int clipCountForCurInstruction;
        private Food food;
        private FoodIngredient foodIngredient;
        private TextToSpeechManager speechManager;
        private bool stop;
#pragma warning restore 649

        public string Trivia { get; set; }

        private void Start() {
            food = MenuManager.Food;

            if (food.FoodName == "kinilaw na guso" ||
                food.FoodName == "kutsinta" ||
                food.FoodName == "palitaw" ||
                food.FoodName == "sapin sapin" ||
                food.FoodName == "crispy hito") {
                // Display bowl at start
                potImage.gameObject.SetActive(false);
                bowlImage.gameObject.SetActive(true);
                panImage.gameObject.SetActive(false);
                fryPanImage.gameObject.SetActive(false);
                jarImage.gameObject.SetActive(false);
                leafImage.gameObject.SetActive(false);
            } else if (food.FoodName == "lunis" ||
                       food.FoodName == "pickled balut" ||
                       food.FoodName == "ginataang tilapia" ||
                       food.FoodName == "chicken adobo sa gata" ||
                       food.FoodName == "ginataang hipon, sitaw, kalabasa") {
                // Display frypan at start
                potImage.gameObject.SetActive(false);
                bowlImage.gameObject.SetActive(false);
                panImage.gameObject.SetActive(false);
                fryPanImage.gameObject.SetActive(true);
                jarImage.gameObject.SetActive(false);
                leafImage.gameObject.SetActive(false);
            }

            // Ready the ingredients after knowing the instruction
            // Pass only translated instruction. Tagalog not supported
            foodIngredient = new FoodIngredient(food, food.InstructionTranslated);

            trivia.SetText(LeanLocalization.CurrentLanguage.Equals("English") ? food.TriviaTranslated : food.Trivia);

            // We will subscribe to an event to check of the language has been changed
            LeanLocalization.OnLocalizationChanged += CurrentLanguage;

            speechManager = FindObjectOfType<TextToSpeechManager>();
            speechManager.Init(food.FoodName);

            StartCoroutine(Cook());
        }

        // Subscriber
        public void CurrentLanguage() {
            if (LeanLocalization.CurrentLanguage.Equals("English")) {
                trivia.SetText(food.TriviaTranslated);

                // Clean instruction before starting CookingScene
                var cleanedInstruction = string.Empty;
                foreach (var item in food.InstructionTranslated.Split('\n')) {
                    var newText = item.Replace("{skip}", string.Empty);
                    foreach (var waitTime in newText.Split(' ').ToList().Where(i => i.StartsWith("WAIT_TIME:"))) {
                        newText = newText.Replace(waitTime, string.Empty);
                    }

                    cleanedInstruction += newText + "\n";
                }

                instruction.SetText(cleanedInstruction);
            }
            else {
                trivia.SetText(food.Trivia);
                instruction.SetText(food.Instruction);
            }
        }

        // TODO: Compress this function
        private IEnumerator BananaLeaf() {
            bananaLeaf.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitUntil(() =>
                bananaLeaf.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BananaLeafIdle"));
            while (stop) {
                yield return null;
            }
        }
        // TODO: Compress this function
        private IEnumerator ChickenAdobo() {
            chickenAdobo.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitUntil(() =>
                chickenAdobo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ChickenAdoboIdle"));
            while (stop) {
                yield return null;
            }

            yield return null;
        }
        // TODO: Compress this function
        private IEnumerator Palitaw() {
            palitaw.GetComponent<Animator>().SetTrigger("show");
            yield return new WaitUntil(() =>
                palitaw.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PalitawIdle"));
            while (stop) {
                yield return null;
            }

            yield return null;
        }

        // TODO: Compress this function
        private IEnumerator Cook() {
            var idx = 0;
            // We will move to the next instruction
            while (foodIngredient.MoveNext()) {
                if (food.FoodName == "chicken adobo sa gata" && idx == 1 ||
                    food.FoodName == "kutsinta" && idx == 3 ||
                    food.FoodName == "palitaw" && idx == 3 ||
                    food.FoodName == "sapin sapin" && idx == 5) {
                    // Display pot
                    potImage.gameObject.SetActive(true);
                    bowlImage.gameObject.SetActive(false);
                    panImage.gameObject.SetActive(false);
                    fryPanImage.gameObject.SetActive(false);
                    jarImage.gameObject.SetActive(false);
                    leafImage.gameObject.SetActive(false);
                }
                else if (food.FoodName == "sapin sapin" && idx == 6) {
                    // Display pan
                    panImage.gameObject.SetActive(true);
                    potImage.gameObject.SetActive(false);
                    bowlImage.gameObject.SetActive(false);
                    fryPanImage.gameObject.SetActive(false);
                    jarImage.gameObject.SetActive(false);
                    leafImage.gameObject.SetActive(false);
                }
                else if (food.FoodName == "crispy hito" && idx == 2) {
                    // Display fry pan
                    panImage.gameObject.SetActive(false);
                    potImage.gameObject.SetActive(false);
                    bowlImage.gameObject.SetActive(false);
                    fryPanImage.gameObject.SetActive(true);
                    jarImage.gameObject.SetActive(false);
                    leafImage.gameObject.SetActive(false);
                }
                else if (food.FoodName == "pickled balut" && idx == 1) {
                    // Display jar
                    panImage.gameObject.SetActive(false);
                    potImage.gameObject.SetActive(false);
                    bowlImage.gameObject.SetActive(false);
                    fryPanImage.gameObject.SetActive(false);
                    jarImage.gameObject.SetActive(true);
                    leafImage.gameObject.SetActive(false);
                }
                else if (food.FoodName == "sapin sapin" && idx == 10) {
                    // Display leaf
                    panImage.gameObject.SetActive(false);
                    potImage.gameObject.SetActive(false);
                    bowlImage.gameObject.SetActive(false);
                    fryPanImage.gameObject.SetActive(false);
                    jarImage.gameObject.SetActive(false);
                    leafImage.gameObject.SetActive(true);
                }

                clipCountForCurInstruction = speechManager.ClipsForCurrentInstruction(speechManager.ClipName);

                FindObjectOfType<AudioManager>().BackgroundTheme(0.2f);

                var contents =
                    foodIngredient.Current; // It contains all existing(non-skip) ingredient within an instruction

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
                        foodIngredient.Time = 0f; // Clear the time from foodIngredient
                        timer.Start = true;

                        while (!timer.NormalLimitReached) {
                            if (Mathf.RoundToInt(timer.Min) == Mathf.RoundToInt(timer.Until) - 2 && 
                                Mathf.RoundToInt(timer.Sec) == 0) {
                                // Start animation of timer
                                timer.GetComponent<Animator>().SetTrigger("show");

                                FindObjectOfType<AudioManager>().TimerNearingSFXPlay();
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
                        foodIngredient.Time = 0f; // Clear the time from foodIngredient
                        timer.Start = true;

                        while (!timer.NormalLimitReached) {
                            if (Mathf.RoundToInt(timer.Min) == Mathf.RoundToInt(timer.Until) - 2 && 
                                Mathf.RoundToInt(timer.Sec) == 0) {
                                // Start nearing signal animation of timer 
                                timer.GetComponent<Animator>().SetTrigger("show");

                                FindObjectOfType<AudioManager>().TimerNearingSFXPlay();
                            }

                            if (timer.TriviaLimit) {
                                // Listen to a random limit if the WAIT_TIME is more than 3 mins
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
                    }
                    else {
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

                        if (food.FoodName == "chicken adobo sa gata" && idx == 3 && content.Key == "chicken") {
                            // Do animation for chicken adobo
                            StartCoroutine(ChickenAdobo());

                            clipCountForCurInstruction--;

                            continue;
                        }

                        Debug.Log(content.Key + "," + content.Value);
                        // Get image from Resources folder
                        var texture2D = Resources.Load("Ingredients/" + content.Key) as Texture2D;
                        rawImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                            new Vector2(0.5f, 0.2f));
                        // Get plate image from Resources folder
                        texture2D = Resources.Load("Ingredients/plate_image") as Texture2D;
                        plateImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                            new Vector2(0.5f, 0.1f));

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

                if (food.FoodName == "palitaw" && idx == 3) {
                    StartCoroutine(Palitaw());
                } else if (food.FoodName == "sapin sapin" && idx == 10) {
                    StartCoroutine(BananaLeaf());
                }

                idx++;
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
            var foodSprite = Resources.Load("FoodResult/" + food.FoodName) as Texture2D;
            foodResult.GetComponent<Image>().sprite = Sprite.Create(foodSprite,
                new Rect(0, 0, foodSprite.width, foodSprite.height), new Vector2(0.5f, 0.1f));

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
}