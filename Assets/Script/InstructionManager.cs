using Assets.Script.DatabaseModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour {

    private TextMeshProUGUI proUGUI;
    private string instructions;

    private void Start() {
        Food food = MenuManager.Food;

        // Display the instructions on the ScrollRect
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            // Clean instruction before starting CookingScene
            foreach (var item in food.InstructionTranslated.Split('\n')) {
                string newText = item.Replace("{skip}", string.Empty);
                foreach (var waitTime in newText.Split(' ').ToList().Where(i => i.StartsWith("WAIT_TIME:")).Take(1)) {
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
