using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {

    private Scene currentScene;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (SceneManager.GetActiveScene().name) {
            case "Categories":
                SceneManager.LoadScene("Main menu");
                break;
            }
        }
    }
}
