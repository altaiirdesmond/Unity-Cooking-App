using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class TextToSpeechManager : MonoBehaviour {

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;

    private float clipCurrentTime;
    private int clipIndex;
    private float temp;

    public bool IsPlaying {
        get {
            return audioSource.isPlaying;
        }
    }

    public float ClipMaxLength { get; set; }

    public void Init(int foodInstructionLength, string foodName) {
        Debug.Log("Init...");
        audioSource = GetComponent<AudioSource>();

        clipIndex = 0;

        audioClips = new AudioClip[foodInstructionLength];
        for (int i = 0; i < foodInstructionLength; i++) {
            string path = string.Format("tts/{0}/{1} {2}", foodName, foodName, i);
            audioClips[i] = Resources.Load<AudioClip>(path);
        }
    }

    public void Play() {
        Debug.Log("<color=Blue>" + clipIndex + "</color>");
        if(clipIndex > audioClips.Length - 1) {
            return;
        }
        // Assign a clip first
        audioSource.clip = audioClips[clipIndex++];
        // Then play that clip
        audioSource.Play();
        // Able to know whether the clip has finished playing
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
