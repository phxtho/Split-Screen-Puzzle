using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public Sound music;
    public AudioClip[] musicClips;
    int musicClipNumber;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }

    }

    private void Start()
    {
        // PlaySound("Theme");
        SetUpMusic();
    }

    private void Update()
    {
        if (!music.source.isPlaying)
        {
            ChangeMusicClip();
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found");
            return;
        }

        s.source.Play();
    }

    void SetUpMusic()
    {
        musicClipNumber = 5;
        music.clip = musicClips[musicClipNumber];

        music.source = gameObject.AddComponent<AudioSource>();

        music.source.clip = music.clip;
        music.source.volume = music.volume;
        music.source.pitch = music.pitch;
        music.source.loop = music.loop;
        music.source.Play();
    }

    void ChangeMusicClip()
    {
        musicClipNumber++;

        if (musicClipNumber > musicClips.Length - 1)
            musicClipNumber = 0;

        music.clip = musicClips[musicClipNumber];
        music.source.clip = music.clip;
        music.source.Play();
    }
}
