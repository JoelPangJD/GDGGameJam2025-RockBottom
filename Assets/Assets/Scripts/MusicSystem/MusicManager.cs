using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public float beatTempo = 120.0f; // Beats per minute
    //The starting delay before the music starts playing to let the beats sync up
    [SerializeField]
    private float startingDelay;

    private AudioSource musicSource;
    [SerializeField]
    public AudioClip musicClip;
    //[SerializeField]
    //private AudioMixer audioMixer;

    //A small buffer time account for when the song starts to play and the first beat is hit
    public float firstBeatBufferTime = 0.1f;

    private float tempoScale = 1.0f;

    public float startingTempoScale = 0.6f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        musicSource = GetComponent<AudioSource>();

        StartCoroutine(CheckMusicEnd());
        SetTempoScale(startingTempoScale);
    }

    void SetTempoScale(float tempoScale)
    {
        this.tempoScale = tempoScale;
        musicSource.pitch = tempoScale;
    }

    public float GetBeatInterval()
    {
        return 60.0f / (beatTempo * tempoScale);
    }

    IEnumerator CheckMusicEnd()
    {
        // Wait until the audio is playing to prevent starting before it ends
        yield return new WaitUntil(() => musicSource.isPlaying);

        // Wait until the audio is no longer playing
        yield return new WaitUntil(() => !musicSource.isPlaying);

        GameManager.instance.player1RhythmManager.StopSpawning();
        GameManager.instance.player2RhythmManager.StopSpawning();
        RampMusicUp();
        // Delay before playing the music again 
        yield return new WaitForSeconds(5.0f); // Optional delay before action
        GameManager.instance.player1RhythmManager.StartSpawning();
        GameManager.instance.player2RhythmManager.StartSpawning();
        StartCoroutine(CheckMusicEnd());
        PlayMusic();
    }


    public void PlayMusic()
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music Source or Music Clip is not assigned.");
        }
    }

    public void RampMusicUp()
    {
        tempoScale += 0.5f;
        SetTempoScale(tempoScale);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
