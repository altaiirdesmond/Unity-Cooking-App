using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageListener : MonoBehaviour {

    private Transform tagalogCheckboxImage;
    private Transform englishCheckboxImage;

    private LanguageListener languageListener;

    private void Start() {
        // Singleton. This prevents multiple instances
        if (languageListener != null) {
            Destroy(gameObject);
        } else {
            languageListener = this;
        }

        DontDestroyOnLoad(gameObject);

        tagalogCheckboxImage = GameObject.FindGameObjectWithTag("Tagalog").GetComponent<Transform>();
        englishCheckboxImage = GameObject.FindGameObjectWithTag("English").GetComponent<Transform>();

        CurrentLanguage();

        // We monitor if the scene changes
        SceneManager.activeSceneChanged += ChangedActiveScene;

        // We will subscribe to an event to check of the language has been changed
        Lean.Localization.LeanLocalization.OnLocalizationChanged += CurrentLanguage;
    }

    private void CurrentLanguage() {
        if (Lean.Localization.LeanLocalization.CurrentLanguage.Equals("English")) {
            tagalogCheckboxImage.gameObject.SetActive(false);
            englishCheckboxImage.gameObject.SetActive(true);
        } else {
            tagalogCheckboxImage.gameObject.SetActive(true);
            englishCheckboxImage.gameObject.SetActive(false);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next) {
        // We get the buttons for each scene loaded
        tagalogCheckboxImage = GameObject.FindGameObjectWithTag("Tagalog").GetComponent<Transform>();
        englishCheckboxImage = GameObject.FindGameObjectWithTag("English").GetComponent<Transform>();

        CurrentLanguage();
    }
}
