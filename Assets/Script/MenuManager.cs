using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void GoToSetting() {
        SceneManager.LoadScene("Setting");
    }

    public void SetLanguage(string language) {
        
    }

    public void ApplySetting() {
        SceneManager.LoadScene("Main menu");
    }

    public void GotoCategories() {
        SceneManager.LoadScene("Categories");
    }
}
