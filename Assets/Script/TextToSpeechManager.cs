using Assets.Script;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// This will handle what instruction will be played through text-to-speech.
/// All ingredients will be fetch from a resource folder to cache into this script.
/// Manipulate text-to-speech here
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TextToSpeechManager : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextToSpeechEngine[] textToSpeechEngines;

    private TextToSpeechEngine currentTextToSpeechEngine;
    private float clipCurrentTime;
    private float temp;

    public string ClipName {
        get {
            return currentTextToSpeechEngine.MainAudioClips[ClipIndex].name;
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
    /// Setup what clips to be played from resources based on food name
    /// </summary>
    /// <param name="foodName"></param>
    public void Init(string foodName) {
        Debug.Log("Init...");

        ClipIndex = 0;

        currentTextToSpeechEngine = Array.Find(textToSpeechEngines, textToSpeechEngine => textToSpeechEngine.Name == foodName);
    }

    public int ClipsForCurrentInstruction(string name) {
        return currentTextToSpeechEngine.MainAudioClips
            .ToList()
            .Where(i => i.name.Contains(name.Substring(0, name.IndexOf('.') + 1)))
            .Count();
    }

    public void Play(int clip) {
        Debug.Log("<color=Blue>" + ClipIndex + "</color>");
        if (ClipIndex > currentTextToSpeechEngine.MainAudioClips.Length - 1) {
            return;
        }
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
        if(ClipMaxLength < 0) {
            return;
        }

        audioSource.Play();
    }
}
