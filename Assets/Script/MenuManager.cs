using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour {

    private string sceneOrigin;
    private readonly bool show = true;
    private bool dropped = true;

    /// <summary>
    /// Set origin to what scene comes first before going to setting
    /// </summary>
    /// <param name="origin">scene origin</param>
    public void GoToSetting(string origin) {
        sceneOrigin = origin;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Setting");
    }

    /// <summary>
    /// The scene that will be loaded is the sceneOrigin after apply
    /// </summary>
    public void ApplySetting() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneOrigin);
    }

    public void Exit() {
        Application.Quit();
    }

    public void GotoCategories() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Categories");
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main menu");
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
            Instantiate(dataObserver.Result, dataObserver.Panels[1].transform);
        }
    }

    public void ShowOptions() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].GetComponent<UIAnimation>().Animator.SetBool("show", show);
    }

    public void HideOptions() {
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Panels[2].GetComponent<UIAnimation>().Animator.SetBool("show", !show);
    }

    public void ShowLanguageDropdown() {
        // Invert active state
        var isActive = !(GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Buttons[0].activeSelf && dropped);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Buttons[0].SetActive(isActive);
        GameObject.Find("SceneManager").GetComponent<CategorySceneManager>().Buttons[1].SetActive(isActive);

        dropped = isActive;
    }
}
