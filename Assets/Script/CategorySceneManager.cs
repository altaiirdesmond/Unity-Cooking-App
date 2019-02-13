using UnityEngine;

public class CategorySceneManager : MonoBehaviour {

    [SerializeField] private GameObject result;
    [SerializeField] private GameObject[] panels; //Panel assignment should be all the same across the scenes
    [SerializeField] private GameObject[] buttons;

    public GameObject Result {
        get {
            return result;
        }

        set {
            result = value;
        }
    }

    public GameObject[] Panels {
        get {
            return panels;
        }

        set {
            panels = value;
        }
    }

    public GameObject[] Buttons {
        get {
            return buttons;
        }

        set {
            buttons = value;
        }
    }
}