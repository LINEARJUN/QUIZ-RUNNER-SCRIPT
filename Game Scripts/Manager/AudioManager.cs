using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    private float masterVolume;
    private float musicVolume;

    [Header("Music Section")]
    public Sound[] music;
    public Sound[] environment;
    public SFX[] sfx;
    private int musicIndex = -1;

    //After creation
    [HideInInspector] public AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        //Create
        musicSource = gameObject.AddComponent<AudioSource>();
        foreach(Sound s in environment)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.playOnAwake = false;
        }
        sfxSource = gameObject.AddComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Volume"))
        {
            string[] tmp = PlayerPrefs.GetString("Volume").Split('|');
            masterVolume = float.Parse(tmp[0]);
            musicVolume = float.Parse(tmp[1]);
        }
        else
        {
            masterVolume = 0.75f;
            musicVolume = 0.5f;
            PlayerPrefs.SetString("Volume", masterVolume.ToString() + "|" + musicVolume.ToString());
        }

        AudioListener.volume = masterVolume;
        musicSource.volume = musicVolume;

        //We need to play music on awake
        PlayMusic();

        //나중에 맵별로 배경음을 바꿔야 합니다.
        PlayEnvironment("cricket_crying");
    }

    private void PlayMusic()
    {
        musicIndex = Random.Range(0, music.Length);

        musicSource.Stop();

        //Set
        musicSource.clip = music[musicIndex].clip;
        musicSource.volume = music[musicIndex].volume;
        musicSource.pitch = music[musicIndex].volume;
        musicSource.loop = music[musicIndex].loop;

        musicSource.Play();

        Invoke("PlayMusic", musicSource.clip.length);
    }
    public void PlayEnvironment(string name)
    {
        Sound s = Array.Find(environment, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Environment: " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void PlaySFX(string name, float volume = -1)
    {
        SFX s = Array.Find(sfx, sfx => sfx.name == name);
        if (s == null)
        {
            Debug.Log("SFX: " + name + " not found!");
            return;
        }
        if (volume == -1)
        {
            sfxSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            sfxSource.PlayOneShot(s.clip, volume);
        }
    }

    public float GetMasterVolume()
    {
        AudioListener.volume = masterVolume;
        PlayerPrefs.SetString("Volume", masterVolume.ToString() + "|" + musicVolume.ToString());
        return masterVolume;
    }
    public float GetMusicVolume()
    {
        musicSource.volume = musicVolume;
        PlayerPrefs.SetString("Volume", masterVolume.ToString() + "|" + musicVolume.ToString());
        return musicVolume;
    }
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetString("Volume", masterVolume.ToString() + "|" + musicVolume.ToString());
        AudioListener.volume = masterVolume;
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetString("Volume", masterVolume.ToString() + "|" + musicVolume.ToString());
        musicSource.volume = musicVolume;
    }
}
