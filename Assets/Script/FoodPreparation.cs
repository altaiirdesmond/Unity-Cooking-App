using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Script {
    public class FoodPreparation : MonoBehaviour {
        private void Start () {
            if (transform.name.Equals("IngredientsText")) {
                GetComponent<TextMeshProUGUI>().SetText(Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")
                    ? MenuManager.Food.IngredientsTranslated
                    : MenuManager.Food.Ingredients);
            } else {
                // Clear WAIT_TIME tags that appear on the game
                GetComponent<TextMeshProUGUI>().SetText(Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")
                    ? RemoveTags(MenuManager.Food.InstructionTranslated)
                    : MenuManager.Food.Instruction);
            }
        }

        // Removes WAIT_TIME tag from translated instructions
        private string RemoveTags(string input) {
            string instructions = string.Empty;

            // Clean instruction before starting CookingScene
            foreach (var item in input.Split('\n')) {
                string newText = item.Replace("{skip}", string.Empty);
                foreach (var waitTime in newText.Split(' ').ToList().Where(i => i.StartsWith("WAIT_TIME:"))) {
                    newText = newText.Replace(waitTime, string.Empty);
                }

                instructions += newText + "\n";
            }

            return instructions;
        }
    }
}