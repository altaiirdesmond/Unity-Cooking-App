using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI tmPro;
    [SerializeField] private bool start = false;
    private float hour = 0;
    private float min = 0;
    private float sec = 0;

    public bool HalfWay {
        get {
            return min == Until / 2f && sec == 0; // If halfway to the time limit
        }
    }

    public int Until { get; set; }

    public bool LimitReached {
        get {
            bool state = false;
            if(min >= Until) {
                // Clear all values
                tmPro.SetText("00:00:00");
                start = false;
                min = 0;
                hour = 0;
                sec = 0;

                state = true; // Limit has been reached
            }

            return state;
        }
    }

    public bool Start {
        get {
            return start;
        }

        set {
            start = value;
        }
    }

    public TextMeshProUGUI TmPro {
        get {
            return tmPro;
        }

        set {
            tmPro = value;
        }
    }

    private void Update() {
        if (start) {
            GoTimer();
        } 
    }

    private void GoTimer() {
        sec += Time.deltaTime;

        if(sec >= 59) {
            min++;
            sec = 0;
        }
        if(min > 59) {
            hour++;
            min = 0;
        }

        TmPro.SetText(string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec));
    }
}
