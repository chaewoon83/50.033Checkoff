using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapShotControl : MonoBehaviour
{
    public AudioMixerSnapshot Paused;
    public AudioMixerSnapshot UnPaused;
    public AudioSource SystemAudioSource;
    public AudioClip PauseAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SystemAudioSource.PlayOneShot(PauseAudio);
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            Pause();
        }
    }

    void Pause()
    {
        if (Time.timeScale == 0)
        {
            Paused.TransitionTo(0.01f);
        }
        else
        {
            UnPaused.TransitionTo(0.01f);
        }
    }
}
