using UnityEngine;

namespace Assets.Script {
    /// <summary>
    /// This will contain all the panels or any other UI element to handle. 
    /// Access only this for UI manipulation
    /// </summary>
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
}