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

    public void TriviaSFXPlay() {
        // This is where we assign what clip should be played on Trivia SFX
        Array.Find(sounds, sounds => sounds.Name == "Trivia").AudioSource.Play();
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

        Debug.Log("<Color=green>" + SceneManager.GetActiveScene().name + "</Color>");
        if (SceneManager.GetActiveScene().name.Equals("Cooking")) {
            // Play background music only on cooking
            BackgroundTheme().loop = true;
            BackgroundTheme(0.2f).Play();

            // If cooking scene has been loaded get the food name to fetch the file and put to list

        } else {
            BackgroundTheme().Stop();
        }

        // We get the buttons for each scene loaded
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button")) {
            // Ready buttons to listen for clicks
            button.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }
    }

    public AudioSource BackgroundTheme(float volume) {
        // This is where we assign what clip should be played on Timer SFX
        AudioSource audioSource = Array.Find(sounds, sounds => sounds.Name == "Background").AudioSource;
        audioSource.volume = volume;

        return audioSource;
    }

    public AudioSource BackgroundTheme() {
        // This is where we assign what clip should be played on Timer SFX
        return Array.Find(sounds, sounds => sounds.Name == "Background").AudioSource;
    }
}
