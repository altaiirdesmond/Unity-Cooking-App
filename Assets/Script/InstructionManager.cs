using Assets.Script.DatabaseModel;
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
                if(item.IndexOf("WAIT_TIME") != -1) {
                    int i = item.IndexOf("WAIT_TIME");
                    newText = newText.Remove(i, 11);
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
