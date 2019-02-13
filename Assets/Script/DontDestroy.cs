using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().buildIndex > 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
