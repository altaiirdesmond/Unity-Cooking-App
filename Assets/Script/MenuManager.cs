using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static string FoodIngredients { get; set; }

    public void Exit() {
        Application.Quit();
    }

    public void GoToCooking() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Cooking");
    }

    public void GotoCategories() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Categories");
    }

    public void GoToHome() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

    public void GoToInstruction(GameObject gameObject) {
        if (GameObject.Find("LeanLocalization").GetComponent<Lean.Localization.LeanLocalization>().CurrentSetLanguage.Equals("English")) {
            // Query food ingredients before proceeding
            DatabaseManager databaseManager = new DatabaseManager();
            foreach (var item in databaseManager.GetFood(gameObject.GetComponent<TextMeshProUGUI>().text)) {
                // English
                FoodIngredients = item.IngredientsTranslated;
            }
        } else {
            // Query food ingredients before proceeding
            DatabaseManager databaseManager = new DatabaseManager();
            foreach (var item in databaseManager.GetFood(gameObject.GetComponent<TextMeshProUGUI>().text)) {
                // Tagalog
                FoodIngredients = item.Ingredients;
            }
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ingredients");
    }

    public void PopulateCategoryResult(string category) {
        CategorySceneManager dataObserver = GameObject.Find("SceneManager").GetComponent<CategorySceneManager>();
        dataObserver.Buttons[0].SetActive(true);
        dataObserver.Buttons[1].SetActive(false);
        dataObserver.Panels[1].SetActive(true);
        dataObserver.Panels[0].SetActive(false);

        DatabaseManager databaseManager = new DatabaseManager();
        int i = 0;
        // Fetch data from database
        foreach (var item in databaseManager.GetFoodsByCategory(category)) {
            dataObserver.Result.GetComponentInChildren<TextMeshProUGUI>().text = item.FoodName;
            // Instantiate first before setting the image
            Instantiate(dataObserver.Result, dataObserver.Panels[1].transform);
            // Reference for database image(blob) file
            Texture2D texture2D = new Texture2D(2, 2);
            // Load retrieved image(byte)
            texture2D.LoadImage(item.Image);
            // Set image as sprite for each prefab. Canvas/CategoryResult_Panel/Result/
            GameObject.Find("Canvas/CategoryResult_Panel").transform.GetChild(i++).GetChild(0).GetChild(0).GetComponent<Image>().sprite = 
                Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2());
        }
    }

    public void ShowOptions() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].GetComponent<UIAnimation>().Animator.SetBool("show", true);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[3].SetActive(true);
    }

    public void HideOptions() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].GetComponent<UIAnimation>().Animator.SetBool("show", false);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[3].SetActive(false);
    }

    public void ShowQuit() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[3].GetComponent<UIAnimation>().Animator.SetBool("show", true);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].SetActive(true);
    }

    public void HideQuit() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[3].GetComponent<UIAnimation>().Animator.SetBool("show", false);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].SetActive(false);
    }

    public void ShowSettings() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[1].GetComponent<UIAnimation>().Animator.SetBool("show", true);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].SetActive(true);
    }

    public void HideSetting() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[1].GetComponent<UIAnimation>().Animator.SetBool("show", false);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].SetActive(false);
    }
}
