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

        /// <summary>
        /// Defines the start
        /// </summary>
        [SerializeField] private bool start;

        /// <summary>
        /// Defines the hour
        /// </summary>
        private float hour;

        /// <summary>
        /// Gets or sets the Until
        /// </summary>
        public float Until { get; set; }

        /// <summary>
        /// Gets a value indicating whether TriviaLimit
        /// <para>TriviaLimit needs, for example 60 minutes, 60 - 1 value to be invoked</para>
        /// </summary>
        public bool TriviaLimit {
            get {
                return Mathf.RoundToInt(Min) == 2 && Mathf.RoundToInt(Sec) == 0;
            }
        }

        /// <summary>
        /// Gets the Min
        /// </summary>
        public float Min { get; private set; }

        /// <summary>
        /// Gets the Sec
        /// </summary>
        public float Sec { get; private set; }

        /// <summary>
        /// <para>Gets a value indicating whether NormalLimitReached</para> 
        /// <para>NormalLimitReached needs, for example 60 minutes, 60 - 1 value to be invoked</para>
        /// </summary>
        public bool NormalLimitReached {
            get {
                bool state = false;
                if (Min >= Until - 1 && Sec >= 59) {
                    // Clear all values
                    timerDisplay.SetText("00:00:00");
                    start = false;
                    Min = 0;
                    hour = 0;
                    Sec = 0;

                    state = true; // Limit has been reached
                }

                return state;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Start
        /// </summary>
        public bool Start {
            get {
                return start;
            }

            set {
                start = value;
            }
        }

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
            if (start) {
                GoTimer();
            }
        }

        /// <summary>
        /// Starts timer
        /// </summary>
        private void GoTimer() {
            Sec += Time.deltaTime;

            if (Sec >= 59) {
                Min++;
                Sec = 0;
            }
            if (Min >= 59) {
                hour++;
                Min = 0;
            }

            TmPro.SetText(string.Format("{0:00}:{1:00}:{2:00}", hour, Min, Sec));
        }
    }
}
