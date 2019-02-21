using Assets.Script.DatabaseModel;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour {

    private TextMeshProUGUI proUGUI;
    private string instructions;

    private void Start() {
        Food food = MenuManager.Food;

        foreach (var item in food.InstructionTranslated.Split('\n')) {
            instructions += item + "\n";
        }

        // Display the instructions on the ScrollRect
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            foreach (var item in food.InstructionTranslated.Split('\n')) {
                instructions += item + "\n";
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
