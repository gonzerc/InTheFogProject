using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip[] musicClips;
    public AudioSource[] sources;

    public AudioClip buttonClick;
    public AudioClip gunShot;
    public AudioClip reloadSFX;
    public AudioClip emptyMag;
    public AudioClip cash;

    private AudioSource music;
    private AudioSource sfx;
    private int clipPlaying;

    void Awake()
    {
        music = sources[0];
        sfx = sources[1];

        music.loop = false;
        sfx.loop = false;
        clipPlaying = Random.Range(0, musicClips.Length);
    }

    void Update()
    {
        if (!music.isPlaying)
        {
            music.clip = musicClips[clipPlaying];
            music.Play();

            int nextIndex = clipPlaying;
            while (nextIndex == clipPlaying)
            {
                nextIndex = Random.Range(0, musicClips.Length);
            }
            clipPlaying = nextIndex;
        }
    }

    public void ButtonClick()
    {
        sfx.clip = buttonClick;
        sfx.Play();
    }

    public void PlayGunshot()
    {
        sfx.clip = gunShot;
        sfx.Play();
    }

    public void PlayReload()
    {
        if (!sfx.isPlaying)
        {
            sfx.clip = reloadSFX;
            sfx.Play();
        }
    }

    public void PlayEmptyMag()
    {
        if (!sfx.isPlaying)
        {
            sfx.clip = emptyMag;
            sfx.Play();
        }
    }

    public void PlayCash()
    {
        if (!sfx.isPlaying)
        {
            sfx.clip = cash;
            sfx.Play();
        }
    }
}
