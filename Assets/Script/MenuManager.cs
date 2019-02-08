using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MenuManager : MonoBehaviour {

    [SerializeField] private GameObject masked_image;

    public static string FoodIngredients { get; set; }

    public void Exit() {
        Application.Quit();
    }

    public void GotoCategories() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Categories");
    }

    public void GoToHome() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }

    public void GoToInstruction(GameObject gameObject) {
        // Query food ingredients before proceeding
        DatabaseManager databaseManager = new DatabaseManager();
        foreach (var item in databaseManager.GetFood(gameObject.GetComponent<TextMeshProUGUI>().text)) {
            FoodIngredients = item.Ingredients;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Ingredients");
    }

    public void PopulateCategoryResult(string category) {
        CategorySceneManager dataObserver = GameObject.Find("SceneManager").GetComponent<CategorySceneManager>();
        dataObserver.Panels[1].SetActive(true);
        dataObserver.Panels[0].SetActive(false);

        DatabaseManager databaseManager = new DatabaseManager();
        // Fetch data from database
        foreach (var item in databaseManager.GetFoodsByCategory(category)) {
            // Instantiate the result gameobject containing the database result
            dataObserver.Result.GetComponentInChildren<TextMeshProUGUI>().text = item.FoodName;

            //MemoryStream stream = new MemoryStream(item.Image);
            //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            //Texture2D texture2D = new Texture2D(2, 2);
            //texture2D.LoadImage(item.Image);
            //dataObserver.Result.GetComponentInChildren<Image>().sprite =
            //    Sprite.Create(texture2D, masked_image.GetComponent<Rect>(), new Vector2());

            Instantiate(dataObserver.Result, dataObserver.Panels[1].transform);
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
