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

    public float testTempoScale = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicSource = GetComponent<AudioSource>();

        SetTempoScale(testTempoScale);
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



    // Update is called once per frame
    void Update()
    {
        
    }
}
