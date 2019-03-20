using System;
using System.Linq;
using UnityEngine;

namespace Assets.Script {
    /// <summary>
    /// This will handle what instruction will be played through text-to-speech.
    /// All ingredients will be fetch from a resource folder to cache into this script.
    /// Manipulate text-to-speech here
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class TextToSpeechManager : MonoBehaviour {
#pragma warning disable 649
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TextToSpeechEngine[] textToSpeechEngines;
#pragma warning restore 649

        private TextToSpeechEngine currentTextToSpeechEngine;
        private float clipCurrentTime;
        private string clipName;
        private float temp;
        private string food;

        public string ClipName {
            get {
                return ClipIndex > currentTextToSpeechEngine.MainAudioClips.Length - 1 ? audioSource.clip.name : currentTextToSpeechEngine.MainAudioClips[ClipIndex].name;
            }
        }

        /// <summary>
        /// Iterate to the next clip
        /// </summary>
        public int NextClip {
            get {
                return ClipIndex++;
            }
        }

        public bool IsPlaying {
            get {
                return audioSource.isPlaying;
            }
        }

        public float ClipMaxLength { get; set; }

        /// <summary>
        /// Get current clip
        /// </summary>
        public int ClipIndex { get; private set; }

        /// <summary>
        /// Setup what clips to be played from resources based on food cName
        /// </summary>
        public void Init(string foodName) {
            Debug.Log("Init...");

            food = Lean.Localization.LeanLocalization.CurrentLanguage == "English" ? foodName : foodName + " tagalog";

            ClipIndex = 0;

            currentTextToSpeechEngine = Array.Find(textToSpeechEngines, textToSpeechEngine => textToSpeechEngine.Name == food);
        }

        public int ClipsForCurrentInstruction(string clipsName) {
            return currentTextToSpeechEngine.MainAudioClips
                .Count(i => i.name.Contains(clipsName.Substring(0, clipsName.IndexOf('.') + 1)));
        }

        public void Play(int clip) {
            if (clip > currentTextToSpeechEngine.MainAudioClips.Length - 1) {
                return;
            }
            Debug.Log("<color=Blue>clip:" + currentTextToSpeechEngine.MainAudioClips[clip].name + "</color>");
            // Assign a clip first
            audioSource.clip = currentTextToSpeechEngine.MainAudioClips[clip];
            // Then play that clip
            audioSource.Play();
            // Able to know whether the clip has finished playing. Set new length for every play
            ClipMaxLength = audioSource.clip.length;
        }

        public void Pause() {
            audioSource.Pause();
        }

        public void Resume() {
            if (ClipMaxLength < 0) {
                return;
            }

            audioSource.Play();
        }
    }
}