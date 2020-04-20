using System;
using System.Collections;
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
    public AudioMixer masterMixer;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup musicMixerGroup2;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup engineMixerGroup;

    // Use this to mute game during production
    public bool mute;
    public float musicVolume;

    private AudioSource musicChannel;
    private AudioSource musicChannel2; //used for crossfades between music
    private AudioSource sfxChannel;
    private AudioSource engineChannel1;
    private AudioSource engineChannel2;
    private AudioSource swivelChannel;
    [HideInInspector] public Dictionary<string, AudioClip> soundMap;
    //Tracks whether intro in coroutine has finished playing or not.
    private bool introCompleted = true;

    //Holds a reference to a coroutine when one starts
    private Coroutine introCoroutine = null;

    float maxVol = 1f;
    float minVol = 0f;
    private float ratio = 1f;

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
        musicChannel.loop = false;

        musicChannel2 = new GameObject().AddComponent<AudioSource>();
        musicChannel2.transform.SetParent(transform);
        musicChannel2.name = "MusicChannel2";
        musicChannel.outputAudioMixerGroup = musicMixerGroup2;
        musicChannel2.loop = true;

        sfxChannel = new GameObject().AddComponent<AudioSource>();
        sfxChannel.transform.SetParent(transform);
        sfxChannel.outputAudioMixerGroup = sfxMixerGroup;
        sfxChannel.name = "SoundChannel";

        engineChannel1 = new GameObject().AddComponent<AudioSource>();
        engineChannel1.transform.SetParent(transform);
        engineChannel1.outputAudioMixerGroup = engineMixerGroup;
        engineChannel1.name = "EngineChannel1";

        engineChannel2 = new GameObject().AddComponent<AudioSource>();
        engineChannel2.transform.SetParent(transform);
        engineChannel2.outputAudioMixerGroup = engineMixerGroup;
        engineChannel2.name = "EngineChannel2";

        swivelChannel = new GameObject().AddComponent<AudioSource>();
        swivelChannel.transform.SetParent(transform);
        swivelChannel.outputAudioMixerGroup = sfxMixerGroup;
        swivelChannel.name = "RotationChannel";

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            soundMap.Add(clip.name, clip);
        }

        ToggleMute(mute);

        if (SceneManager.GetActiveScene().name.Equals("Title"))
            StartCoroutine(PlayIntroMusicUntilDone());
        else if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            StartGameSounds();
        }
    }

    public IEnumerator PlayIntroMusicUntilDone()
    {
        PlayMusic("Menu_Music_Loop");
        bool isNextScene = false;
        musicChannel.loop = false;

        while (!isNextScene)
        {
            yield return null;

            if (!musicChannel.isPlaying)
            {
                if (SceneManager.GetActiveScene().name.Equals("Game"))
                {
                    isNextScene = true;
                }
                else
                {
                    PlayMusic("Menu_Music_Loop");
                }
            }
        }

        PlayMusic("Gameplay_Music_Loop");
        musicChannel.loop = true;
    }

    public void StartGameSounds()
    {
        // Play engine sounds
        engineChannel1.clip = soundMap["Food_Truck_Rolling_Engine_Loop_No_Gears2D_Update"];
        engineChannel1.volume = maxVol;
        engineChannel1.loop = true;
        engineChannel1.Play();
        //masterMixer.SetFloat("Engine1Vol", maxVol);

        engineChannel2.clip = soundMap["Food_Truck_Rolling_Engine_Loop_Almost_Out_Of_Gas_2D"];
        engineChannel2.volume = minVol;
        engineChannel2.loop = true;
        engineChannel2.Play();
        //masterMixer.SetFloat("Engine2Vol", minVol);

        //swivelChannel.clip = soundMap["Food_Truck_Cannon_Turn_Turret_Move_Swivel_2D"];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mute = !mute;
            ToggleMute(mute);
        }

        // Fuel tester
        // if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ratio -= 0.1f;
        //    TransitionToEngine2(ratio);
        // }
    }

    public void TransitionToEngine2(float engine1Ratio)
    {
        engineChannel1.volume = Mathf.Sqrt(engine1Ratio);
        engineChannel2.volume = Mathf.Sqrt(1 - engine1Ratio);

        //float vol = Mathf.Log10(Mathf.Lerp(maxVol, minVol, engine1Ratio) * 20);
        //masterMixer.SetFloat("Engine1Vol", Mathf.Log10(Mathf.Lerp(maxVol, minVol, engine1Ratio) * 20));
        //masterMixer.SetFloat("Engine2Vol", Mathf.Log10(Mathf.Lerp(minVol, maxVol, engine1Ratio) * 20));
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

    // Careful, this code is duplicated
    private int lastCrashPlayed = -1;
    public void PlayRandomCrash()
    {
        int roll = UnityEngine.Random.Range(0, 3);

        while (roll == lastCrashPlayed)
        {
            roll = UnityEngine.Random.Range(0, 3);
        }

        if (roll == 0)
            PlaySound("Just_Car_Crash");
        if (roll == 1)
            PlaySound("Just_Car_Crash-001");
        if (roll == 2)
            PlaySound("Just_Car_Crash-002");

        lastCrashPlayed = roll;
    }

    private int lastShotPlayed = -1;
    public void PlayRandomShot()
    {
        int roll = UnityEngine.Random.Range(0, 4);

        while (roll == lastShotPlayed)
        {
            roll = UnityEngine.Random.Range(0, 4);
        }

        if (roll == 0)
            PlaySound("Food_Truck_Cannon_Shoot_Updated_2D");
        if (roll == 1)
            PlaySound("Food_Truck_Cannon_Shoot_Updated-001_2D");
        if (roll == 2)
            PlaySound("Food_Truck_Cannon_Shoot_Updated-002_2D");
        if (roll == 3)
            PlaySound("Food_Truck_Cannon_Shoot_Updated-003_2D");

        lastShotPlayed = roll;
    }

    public void PlayRandomSpotInSwivel()
    {
        float playbackTime = UnityEngine.Random.Range(0, swivelChannel.clip.length - 1);
        swivelChannel.time = playbackTime;
        swivelChannel.volume = 1;
        swivelChannel.Play();

        //Invoke("StopSwiveling", 1f);
        //StartCoroutine(LowerSwivelVolume(0.5f));
    }

    public void StopSwiveling()
    {
        swivelChannel.Stop();
    }

    //private IEnumerator LowerSwivelVolume(float delay)
    //{
    //    float timeElapsed = 0;

    //    while (timeElapsed)
    //}

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
