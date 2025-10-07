using UnityEngine;

/*
 * This script manages all game sounds.
 */
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource; // SudioSource for the sound effects described on the excersise

    [Header("Clips")]
    public AudioClip bounceClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;
    public AudioClip startClip;

    void Awake()
    {
        // Singleton: ensures only one AudioManager exists as Iñigo asked us to do
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // persists between scenes
    }

    // Play a sfx
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
    
}