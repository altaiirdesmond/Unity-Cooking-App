using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI tmPro;
    [SerializeField] private bool start = false;
    private float hour = 0;
    private float min = 0;
    private float sec = 0;
    
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
