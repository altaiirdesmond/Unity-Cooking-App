using TMPro;
using UnityEngine;

public class IngredientManager : MonoBehaviour {
	void Start () {
        GetComponent<TextMeshProUGUI>().SetText(MenuManager.FoodIngredients);
    }
}
