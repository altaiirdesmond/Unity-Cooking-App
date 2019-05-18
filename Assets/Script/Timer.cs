namespace Assets.Script {
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Timer" />
    /// </summary>
    public class Timer : MonoBehaviour {

        /// <summary>
        /// Defines the timerDisplay
        /// </summary>
        [SerializeField] private TextMeshProUGUI timerDisplay;

        private float t;

        public bool CountDecreasing { get; set; }

        /// <summary>
        /// Gets or sets the Until
        /// </summary>
        public float Until { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Start
        /// </summary>
        public bool Start { get; set; }

        /// <summary>
        /// Defines the Hour
        /// </summary>
        public float Hour { get; set; }

        /// <summary>
        /// Gets the Min
        /// </summary>
        public float Min { get; private set; }

        /// <summary>
        /// Gets the Sec
        /// </summary>
        public float Sec { get; private set; }

        /// <summary>
        /// Gets or sets the timerDisplay
        /// </summary>
        public TextMeshProUGUI TmPro {
            get {
                return timerDisplay;
            }

            set {
                timerDisplay = value;
            }
        }

        private void Update() {
            if (Start && !CountDecreasing) {
                CountUp();
            }else if (Start && CountDecreasing) {
                CountDown();
            }
        }

        private void CountDown() {
            t -= Time.deltaTime;
            
            Sec = (int)(t % 60);
            Min = (int)(t / 60) % 60;

            TmPro.SetText(string.Format("{0:0}:{1:00}:{2:00}", Hour, Min, Sec));
        }

        /// <summary>
        /// Starts timer
        /// </summary>
        private void CountUp() {
            t += Time.deltaTime;

            Min = (int)(t / 60) % 60;
            Sec = (int) (t % 60);

            TmPro.SetText(string.Format("{0:0}:{1:00}:{2:00}", Hour, Min, Sec));
        }

        /// <summary>
        /// <para>Gets a value indicating whether TimerUp</para> 
        /// </summary>
        public bool TimerUp() {
            if (Mathf.RoundToInt(Min) <= 0 && Mathf.RoundToInt(Sec) <= 0) {
                // Clear all values
                timerDisplay.SetText("00:00:00");
                Start = false;
                Min = 0;
                Hour = 0;
                Sec = 0;

                t = 0;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether TriviaLimit
        /// </summary>
        public bool TriviaLimit() {
            return Mathf.RoundToInt(Min) == Mathf.RoundToInt(2f) && Mathf.RoundToInt(Sec) == 0;
        }

        /// <summary>
        /// Where to start counting down
        /// </summary>
        /// <param name="minutes">Minute to start from</param>
        public void StartTimerAt(int minutes) {
            t = 60 * minutes;
            Min = minutes;
        }
    }
}
