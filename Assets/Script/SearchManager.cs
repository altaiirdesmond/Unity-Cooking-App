namespace Assets.Script {
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the <see cref="SearchManager" />
    /// </summary>
    public class SearchManager : MonoBehaviour {
        /// <summary>
        /// Defines the databaseManager
        /// </summary>
        private DatabaseManager databaseManager;

        /// <summary>
        /// Defines the prefabHolder which holds result prefabs
        /// </summary>
        private Transform[] prefabHolder;

        /// <summary>
        /// What food to be search
        /// </summary>
        /// <param name="inputField">The food to search</param>
        public void SearchFood(TMP_InputField inputField) {
            // resultPrefab will hold the image and text of a food instance
            CategorySceneManager resultPrefab = GameObject.Find("SceneManager").GetComponent<CategorySceneManager>();

            prefabHolder = new Transform[resultPrefab.Panels[0].transform.childCount];

            // Reset visibility
            for (var i = 0; i < resultPrefab.Panels[0].transform.childCount; i++) {
                resultPrefab.Panels[0].transform.GetChild(i).gameObject.SetActive(true);
                prefabHolder[i] = resultPrefab.Panels[0].transform.GetChild(i);
            }

            if (inputField.text == "") {
                return;
            }

            // Show only the queried item.
            foreach (var tf in prefabHolder) {
                tf.GetChild(1).gameObject.SetActive(true);
                tf.GetChild(2).gameObject.SetActive(false);
                if (tf.GetChild(1).GetComponent<TextMeshProUGUI>().text.StartsWith(inputField.text) ||
                    tf.GetChild(2).GetComponent<TextMeshProUGUI>().text.StartsWith(inputField.text)) {
                    if (tf.gameObject.activeSelf == false) {
                        tf.gameObject.SetActive(true);
                    }
                    continue;
                }

                if (tf.GetChild(2).GetComponent<TextMeshProUGUI>().text.StartsWith(inputField.text)) {

                }
                tf.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Set which check image should be disabled.
        /// </summary>
        /// <param name="image">The image<see cref="Image"/></param>
        public void SetDisable(Image image) {
            image.gameObject.SetActive(false);
        }

        /// <summary>
        /// Set which check image should be enabled.
        /// </summary>
        /// <param name="image">The image<see cref="Image"/></param>
        public void SetEnable(Image image) {
            image.gameObject.SetActive(true);
        }
    }
}
