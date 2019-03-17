using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script {
    public class Back : MonoBehaviour {

        private static Back back;

        private void Awake() {
            if(back != null) {
                Destroy(gameObject);
            } else {
                back = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (SceneManager.GetActiveScene().buildIndex > 0) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                } else if (SceneManager.GetActiveScene().buildIndex == 0) {
                    FindObjectOfType<MenuManager>().ShowBlockingPanel();
                    FindObjectOfType<MenuManager>().ShowQuit();
                }
            }
        }
    }
}
