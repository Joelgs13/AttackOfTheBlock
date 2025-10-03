using UnityEngine;

/*
 * This script manages all game sounds.
 * Add it to a GameObject in the scene (e.g. "AudioManager").
 * Assign the sound clips in the Inspector.
 */
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource; // For sound effects

    [Header("Clips")]
    public AudioClip bounceClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;
    public AudioClip startClip;

    void Awake()
    {
        // Singleton: ensures only one AudioManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // persists between scenes
    }

    // Play a one-shot sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
    
}