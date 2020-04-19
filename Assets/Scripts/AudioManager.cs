﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Audio manager that loads in all sounds from the Audio folder. Use the file names as arguments to play.
/// </summary>
//[RequireComponent(typeof(AudioListener))]
public class AudioManager : Singleton<AudioManager>
{
    // Inspector set
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup musicMixerGroup2;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup engineMixerGroup1;
    public AudioMixerGroup engineMixerGroup2;

    // Use this to mute game during production
    public bool mute;
    public float musicVolume;

    private AudioSource musicChannel;
    private AudioSource musicChannel2; //used for crossfades between music
    private AudioSource sfxChannel;
    private AudioSource engineChannel1;
    private AudioSource engineChannel2;
    private Dictionary<string, AudioClip> soundMap;
    //Tracks whether intro in coroutine has finished playing or not.
    private bool introCompleted = true;

    //Holds a reference to a coroutine when one starts
    private Coroutine introCoroutine = null;

    protected override void Awake()
    {
        base.Awake();

        SetDontDestroy();
    }

    void Start()
    {
        soundMap = new Dictionary<string, AudioClip>();

        musicChannel = new GameObject().AddComponent<AudioSource>();
        musicChannel.transform.SetParent(transform);
        musicChannel.name = "MusicChannel";
        musicChannel.outputAudioMixerGroup = musicMixerGroup;
        musicChannel.loop = true;

        musicChannel2 = new GameObject().AddComponent<AudioSource>();
        musicChannel2.transform.SetParent(transform);
        musicChannel2.name = "MusicChannel2";
        musicChannel.outputAudioMixerGroup = musicMixerGroup2;
        musicChannel2.loop = true;

        sfxChannel = new GameObject().AddComponent<AudioSource>();
        sfxChannel.transform.SetParent(transform);
        sfxChannel.outputAudioMixerGroup = sfxMixerGroup;
        sfxChannel.name = "SoundChannel";

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            soundMap.Add(clip.name, clip);
        }

        ToggleMute(mute);

        if (SceneManager.GetActiveScene().name.Equals("Title"))
            PlayMusic("Menu_Music_Loop");
        else if (SceneManager.GetActiveScene().name.Equals("Game"))
            PlayMusic("Gameplay_Music_Loop");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mute = !mute;
            ToggleMute(mute);
        }
    }

    public void UpdateMusicVolume()
    {
        musicVolume = VolumeListener.volumeLevel;
        musicChannel.volume = VolumeListener.volumeLevel;
        sfxChannel.volume = VolumeListener.volumeLevel;
    }

    public float GetMusicVolume()
    {
        return musicChannel.volume;
    }

    public void PlayMusic(string name)
    {
        musicChannel.clip = soundMap[name];
        musicChannel.volume = musicVolume;
        musicChannel.loop = true;
        musicChannel.Play();
    }

    public void PlayMusicFromTime(string name, float time)
    {
        musicChannel.time = time;
        PlayMusicAndCrossfade(name, 2);
    }


    public void PlayMusicAndCrossfade(string name, float crossfadeDuration)
    {
        float oldMusicTime = musicChannel.time;

        //load music channel1 into channel2
        musicChannel2.clip = musicChannel.clip;
        musicChannel2.volume = musicChannel.volume;
        musicChannel2.loop = true;
        musicChannel2.time = oldMusicTime;
        musicChannel.Stop();
        musicChannel2.Play();

        musicChannel.clip = soundMap[name];
        musicChannel.volume = 0f;
        musicChannel.loop = true;
        musicChannel.time = oldMusicTime;

        musicChannel.Play();

        Debug.Log("Starting crossfade.");
        introCoroutine = StartCoroutine(CrossfadeFromChannel2To1(crossfadeDuration));
    }
    private IEnumerator CrossfadeFromChannel2To1(float crossfadeDuration)
    {
        float timer = 0;

        float volumeMax = musicChannel2.volume;

        while (timer <= crossfadeDuration)
        {
            timer += Time.deltaTime;

            musicChannel2.volume = volumeMax - volumeMax * (timer / crossfadeDuration);
            musicChannel.volume = volumeMax * (timer / crossfadeDuration);
            yield return null;
        }

        musicChannel2.Stop();

    }

    public void PlayMusicOnce(string name)
    {
        musicChannel.clip = soundMap[name];
        musicChannel.volume = musicVolume;
        musicChannel.loop = false;
        musicChannel.Play();
    }

    public void PlayMusicWithIntro(string introName, string loopName)
    {
        if (!introCompleted && introCoroutine != null)
        {

            //cancel the existing coroutine before starting another.
            StopCoroutine(introCoroutine);
        }
        Debug.Log("Starting intro.");
        PlayMusic(introName);
        introCompleted = false;
        introCoroutine = StartCoroutine(PlayMusicDelayed(loopName, musicChannel.clip.length));
    }

    private IEnumerator PlayMusicDelayed(string name, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        introCompleted = true;
        Debug.Log("starting loop");
        PlayMusic(name);
    }

    //Allows seamless transition of Music that are time and tempo aligned.
    public void PlayMusicWithIntroResumingTime(string introName, string loopName)
    {
        float oldMusicTime = musicChannel.time;

        if (!introCompleted && introCoroutine != null)
        {
            //cancel the existing coroutine before starting another.

            StopCoroutine(introCoroutine);

            //need to start from same place in the new intro if existing song was in its intro
            //TODO was a little bit off, decided to start new intro from beginning.
            //PlayMusicFromTime(introName,oldMusicTime);
            PlayMusic(introName);
            float newMusicTime = musicChannel.time;
            //introCoroutine = StartCoroutine(PlayMusicDelayed(loopName,musicChannel.clip.length - newMusicTime));
            introCoroutine = StartCoroutine(PlayMusicDelayed(loopName, musicChannel.clip.length));
        }
        else
        {
            //Intro is complete, so just play from the loop.
            Debug.Log("intro already completed, starting from middle");
            PlayMusicFromTime(loopName, oldMusicTime);
        }
    }

    public void PlaySound(string name)
    {
        AudioClip clip = soundMap[name];
        sfxChannel.PlayOneShot(soundMap[name]);
    }

    public void PlaySound(string name, float volume)
    {
        sfxChannel.PlayOneShot(soundMap[name], volume * VolumeListener.volumeLevel);
    }

    public void ToggleMute(bool mute)
    {
        if (mute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
