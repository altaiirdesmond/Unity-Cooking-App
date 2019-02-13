using Assets.Script.DatabaseModel;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InstructionManager : MonoBehaviour {

    private Food food;
    private List<string> instructionList;
    private string instructions;
    private int minStep = 0;
    private int maxStep;
    private TextMeshProUGUI proUGUI;

    private void Start() {
        food = MenuManager.Food;

        instructionList = new List<string>();
        foreach (var item in food.InstructionTranslated.Split('\n')) {
            instructionList.Add(item);
            instructions += item + "\n";
        }

        GetComponent<TextMeshProUGUI>().SetText(instructions); // Display the instructions on the ScrollRect

        maxStep = instructionList.Count - 1; // Count is non-zero based index
    }

    private void Update() {
        if(minStep < maxStep) {
            Debug.Log(instructionList.ElementAt(minStep++)); // ElementAt is a zero-based index

        }
    }
}
