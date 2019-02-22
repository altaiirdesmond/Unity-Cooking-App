using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI placeHolderText;

    private DatabaseManager databaseManager;
    private Transform[] transforms;

    public static string Filter { get; set; }

    public void SearchFood(TMP_InputField inputField) {
        // resultPrefab will hold the image and text of a food instance
        CategorySceneManager resultPrefab = GameObject.Find("SceneManager").GetComponent<CategorySceneManager>();

        transforms = new Transform[resultPrefab.Panels[0].transform.childCount];

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
            if(Filter == "name") {
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                if (transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.StartsWith(inputField.text)) {
                    if (transform.gameObject.activeSelf == false) {
                        transform.gameObject.SetActive(true);
                    }
                    continue;
                }
                transform.gameObject.SetActive(false);
            } else {
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                if (transform.GetChild(2).GetComponent<TextMeshProUGUI>().text.StartsWith(inputField.text)) {
                    if (transform.gameObject.activeSelf == false) {
                        transform.gameObject.SetActive(true);
                    }
                    continue;
                }
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void FilterBy(string filter) {
        Filter = filter;
        placeHolderText.text = filter;
        Debug.Log("Filter changed to " + filter);
    }

    public void SetDisable(Image image) {
        image.gameObject.SetActive(false);
    }

    public void SetEnable(Image image) {
        image.gameObject.SetActive(true);
    }
}
