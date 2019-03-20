using TMPro;
using UnityEngine;

namespace Assets.Script {
    public class IngredientText : MonoBehaviour {
        private void Start () {
            GetComponent<TextMeshProUGUI>().SetText(Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")
                ? MenuManager.Food.IngredientsTranslated
                : MenuManager.Food.Ingredients);
        }
    }
}