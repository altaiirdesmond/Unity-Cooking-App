using TMPro;
using UnityEngine;

public class SearchManager : MonoBehaviour {

    private DatabaseManager databaseManager;
    private string filter;

    public void SearchFood(TMP_InputField inputField) {
        // resultPrefab will hold the image and text of a food instance
        CategorySceneManager resultPrefab = GameObject.Find("SceneManager").GetComponent<CategorySceneManager>();

        Transform[] transforms = new Transform[resultPrefab.Panels[0].transform.childCount];

        // Reset visibility
        for (int i = 0; i < resultPrefab.Panels[0].transform.childCount; i++) {
            resultPrefab.Panels[0].transform.GetChild(i).gameObject.SetActive(true);
            transforms[i] = resultPrefab.Panels[0].transform.GetChild(i);
        }

        if (inputField.text == "") {
            return;
        }

        // Show only the queried item.
        foreach (Transform transform in transforms) {
            if (transform.GetComponentInChildren<TextMeshProUGUI>().text.StartsWith(inputField.text)) {
                if(transform.gameObject.activeSelf == false) {
                    transform.gameObject.SetActive(true);
                    Debug.Log("This is only visible");
                }
                continue;
            }
            transform.gameObject.SetActive(false);
        }
    }

    public void FilterBy(string filter) {
        this.filter = filter;
    }
}
