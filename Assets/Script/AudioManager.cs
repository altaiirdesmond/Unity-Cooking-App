using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    private static AudioManager dontDestroy;
    private AudioSource[] sfxAudioSources;
    private GameObject[] buttons;

    [Header("Misc. fx")]
    public Sound[] sounds;

    [Header("Background theme")]
    [SerializeField] private AudioSource backgroundClipAudioSource;

    public AudioSource BackgroundClipAudioSource {
        get {
            return backgroundClipAudioSource;
        }
    }

    private void Awake() {
        // Singleton. This prevents multiple instances
        if (dontDestroy != null) {
            Destroy(gameObject);
        } else {
            dontDestroy = this;
        }

        DontDestroyOnLoad(gameObject);
        
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button")) {
            // Ready buttons to listen for clicks
            button.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }

        foreach (var sound in sounds) {
            // We will setup each clip
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.AudioClip;
            sound.AudioSource.clip.name = sound.AudioClip.name;
            sound.AudioSource.volume = sound.Volume;
        }

        // We monitor if the scene changes
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void TaskOnClick() {
        // This is where we assign what clip should be played on button click
        Array.Find(sounds, sounds => sounds.Name == "ButtonClip").AudioSource.Play();
    }

    public void TimerStartSFXPlay() {
        // This is where we assign what clip should be played on Timer SFX
        Array.Find(sounds, sounds => sounds.Name == "TimerStart").AudioSource.Play();
    }

    public void TimerDoneSFXPlay() {
        // This is where we assign what clip should be played on Timer SFX
        Array.Find(sounds, sounds => sounds.Name == "TimerDone").AudioSource.Play();
    }

    private void ChangedActiveScene(Scene current, Scene next) {
        string currentName = current.name;

        if (currentName == null) {
            // Scene1 has been removed
            currentName = "Replaced";
        }

        // We get the buttons for each scene loaded
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button")) {
            // Ready buttons to listen for clicks
            button.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }
    }

    public void Playbackground(bool play) {
        if (play) {
            BackgroundClipAudioSource.Play();
        } else {
            BackgroundClipAudioSource.Pause();
        }
    }
}
