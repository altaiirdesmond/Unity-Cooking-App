using System.Linq;
using Assets.Script.DatabaseModel;
using TMPro;
using UnityEngine;

namespace Assets.Script {
    public class InstructionManager : MonoBehaviour {

        private TextMeshProUGUI ingredientText;
        private string instructions;

        private void Start() {
            Food food = MenuManager.Food;

            // Display the instructions on the ScrollRect
            if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
                // Clean instruction before starting CookingScene
                foreach (var item in food.InstructionTranslated.Split('\n')) {
                    string newText = item.Replace("{skip}", string.Empty);
                    foreach (var waitTime in newText.Split(' ').ToList().Where(i => i.StartsWith("WAIT_TIME:"))) {
                        newText = newText.Replace(waitTime, string.Empty);
                    }

                    instructions += newText + "\n";
                }
                GetComponent<TextMeshProUGUI>().SetText(instructions);
            } else {
                foreach (var item in food.Instruction.Split('\n')) {
                    instructions += item + "\n";
                }
                GetComponent<TextMeshProUGUI>().SetText(instructions);
            }
        }
    }
}
