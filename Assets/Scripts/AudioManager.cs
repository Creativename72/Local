using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //PLACE THE PREFAB OF THE MUSIC PLAYER IN THE FIRST SCENE OF THE GAME
    //singleton Instance of the music player
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource; //persistent musicAudioSource
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    // Start is called before the first frame update
    void Awake()
    {
        //verify that this object is the singleton Instance, otherwise delete
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        musicSlider.onValueChanged.AddListener((volume) =>
        {
            musicSlider.value = volume;
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        });

        sfxSlider.onValueChanged.AddListener((volume) =>
        {
            sfxSlider.value = volume;
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        });   
    }

    public void UpdateSliders()
    {
        audioMixer.GetFloat("MusicVolume", out var musicVal);
        audioMixer.GetFloat("SFXVolume", out var sfxVal);
        musicSlider.value = Mathf.Pow(10, (musicVal / 20));
        sfxSlider.value = Mathf.Pow(10, (sfxVal / 20));
    }

    public void SetMusicVolume(float volume)
    {
        musicSlider.value = volume;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSlider.value = volume;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVolume(float volume, string group)
    {
        if (group == "MusicVolume")
        {
            //musicSlider.value = volume;
        }
        else if (group == "SFXVolume")
        {
            //sfxSlider.value = volume;
        }
        audioMixer.SetFloat(group, Mathf.Log10(volume) * 20);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(musicAudioSource == null)
        {
            return;
        }
        
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    /// <summary>
    /// Fades in / out the given group over duration invoking the specified callback function
    /// at the end of the call
    /// </summary>
    /// <param name="target">Target Volume</param>
    /// <param name="duration">duration of fade</param>
    /// <param name="callback">function to be invoked</param>
    /// <param name="group">AudioMixer group to target</param>
    public void FadeTo(float target, float duration, Action callback = null, string group = "MasterVolume")
    {
        if (target <= 0)
            target = 0.0001f;
        StartCoroutine(ChangeVolume(target, duration, callback, group));
    }

    private IEnumerator ChangeVolume(float target, float duration, Action callback, string group = "MasterVolume")
    {

        // can't change volume on nothing
        if (!audioMixer.GetFloat(group, out float mixerVol)) {
            Debug.LogWarning("Attempting to Change volume of unexposed parameter.");
            yield break;
        }
        float currentTime = 0;
        float start = Mathf.Pow(10, mixerVol / 20);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float dt = currentTime / duration;
            float fracVolume = Mathf.Lerp(start, target, dt);
            SetVolume(fracVolume, group);

            yield return null;
        }

        callback?.Invoke();

        yield break;
    }
    public void PlaySFX(string n)
    {
        // Debug.Log("Play SFX: "+n);
        sfxAudioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/" + n), 1f);
    }

    public void PlayButtonSFX()
    {
        sfxAudioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/s_nextbutton"));
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
