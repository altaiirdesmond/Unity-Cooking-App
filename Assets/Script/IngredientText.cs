using TMPro;
using UnityEngine;

public class IngredientText : MonoBehaviour {
	void Start () {
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            GetComponent<TextMeshProUGUI>().SetText(MenuManager.Food.IngredientsTranslated);
        } else {
            GetComponent<TextMeshProUGUI>().SetText(MenuManager.Food.Ingredients);
        }
    }
}
