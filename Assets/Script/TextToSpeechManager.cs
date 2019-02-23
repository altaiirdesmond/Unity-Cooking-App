using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class TextToSpeechManager : MonoBehaviour {

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    public int ClipIndex { get; set; }

    public bool IsPlaying {
        get {
            return audioSource.isPlaying;
        }
    }

    public void Init(int foodInstructionLength, string foodName) {
        Debug.Log("Init...");
        audioSource = GetComponent<AudioSource>();

        ClipIndex = 0;

        audioClips = new AudioClip[foodInstructionLength];
        for (int i = 0; i < foodInstructionLength; i++) {
            string path = string.Format("tts/{0}/{1} {2}", foodName, foodName, i);
            audioClips[i] = Resources.Load<AudioClip>(path);
        }
    }

    public void Play() {
        Debug.Log("<color=Blue>" + ClipIndex + "</color>");
        if(ClipIndex > audioClips.Length - 1) {
            return;
        }
        // Assign a clip first
        audioSource.clip = audioClips[ClipIndex++];
        // Then play that clip
        audioSource.Play();
    }

    public void Pause() {
        audioSource.Pause();
    }
    
    public void Resume() {
        audioSource.Play();
    }
}
