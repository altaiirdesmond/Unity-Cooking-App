using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {

    private static DontDestroy dontDestroy;

    private void Awake() {
        // Singleton. This prevents multiple instances
        if(dontDestroy != null) {
            Destroy(gameObject);
        } else {
            dontDestroy = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().buildIndex > 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 0) {
                FindObjectOfType<MenuManager>().ShowBlockingPanel();
                FindObjectOfType<MenuManager>().ShowQuit();
            }
        }
    }
}
