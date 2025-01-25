using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loopy : MonoBehaviour
{
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioClip musicStart;

    void Start()

    {
        if(!musicStart)
        {
            musicSource.PlayOneShot(musicStart);
        }
        musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
    }

}
