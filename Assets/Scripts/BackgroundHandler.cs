using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using System;

public class BackgroundHandler : MonoBehaviour
{
    [FormerlySerializedAs("s")] public SpriteRenderer spriteRenderer;
    //[SerializeField] public Sprite[] backgrounds;
    [FormerlySerializedAs("bgs")] [SerializeField] public Sprite[] backgrounds;
    public int currentBackground;
    public bool change;
    
    [SerializeField] BackgroundFaderHelper bgFader;
    [SerializeField] AudioMixer audioFader;
    [SerializeField] float audioFadeDuration;

    public BackgroundChangeEvent m_backgroundChangeEvent;

    private bool transitioning;
    private bool hasChangedBackground;

    private void Start()
    {
        currentBackground = 0;
        spriteRenderer.sprite = backgrounds[currentBackground];
        transitioning = false;
        hasChangedBackground = false;

        if (m_backgroundChangeEvent == null) {
            m_backgroundChangeEvent = new BackgroundChangeEvent();
        }
    }

    private void Update()
    {
        if (change)
        {
            change = false;
            changeBackground();
        }
    }

    public void changeBackground(bool doFade = true)
    {
        if (transitioning)
        {
            StopAllCoroutines();
            bgFader.SetVisible();
            if (!hasChangedBackground)
            {
                currentBackground++;
            }
        }
        StartCoroutine(backgroundChange(doFade));
    }

    private IEnumerator backgroundChange(bool doFade) {
        hasChangedBackground = false;
        if (doFade)
        {
            transitioning = true;
            // Debug.Log("BackgroundHandler: Fading out SFX and Ambience");
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 0f));
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));
            bgFader.FadeBlack(true);

            yield return new WaitForSeconds(0.75f);
        }

        if (currentBackground + 1 < backgrounds.Length)
            m_backgroundChangeEvent.Invoke(backgrounds[currentBackground + 1].name);

        currentBackground++;
        if (currentBackground >= backgrounds.Length)
        {
            currentBackground = backgrounds.Length - 1;
        }
        spriteRenderer.sprite = backgrounds[currentBackground];
        hasChangedBackground = true;
        if (doFade)
        {
            // Debug.Log("BackgroundHandler: Fading in SFX and Ambience");
            bgFader.FadeBlack(false);
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 1f));
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
            yield return new WaitForSeconds(0.75f);
            transitioning = false;
        }
    }

    public void EnableChange(bool value)
    {
        this.change = value;
    }
}


[System.Serializable]
public class BackgroundChangeEvent : UnityEvent<string>
{
}
