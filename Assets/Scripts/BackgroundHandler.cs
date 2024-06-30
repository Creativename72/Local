using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class BackgroundHandler : MonoBehaviour
{
    public SpriteRenderer s;
    [SerializeField]
    public Sprite[] bgs;
    public int currentBg;
    public bool b;
    
    [SerializeField] BackgroundFaderHelper bgFader;
    [SerializeField] AudioMixer audioFader;
    [SerializeField] float audioFadeDuration;

    public BackgroundChangeEvent m_backgroundChangeEvent;

    private void Start()
    {
        currentBg = 0;
        s.sprite = bgs[currentBg];

        if (m_backgroundChangeEvent == null) {
            m_backgroundChangeEvent = new BackgroundChangeEvent();
        }
    }

    private void Update()
    {
        if (b)
        {
            b = false;
            changeBackground();
        }
    }

    public void changeBackground(bool doFade = true)
    {
        StartCoroutine(backgroundChange(doFade));
    }

    private IEnumerator backgroundChange(bool doFade) {
        if (doFade)
        {
            // Debug.Log("BackgroundHandler: Fading out SFX and Ambience");
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 0f));
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));
            bgFader.Fade();

            yield return new WaitForSeconds(0.75f);
        }

        if (currentBg + 1 < bgs.Length)
            m_backgroundChangeEvent.Invoke(bgs[currentBg + 1].name);
        currentBg++;
        if (currentBg >= bgs.Length)
        {
            currentBg = bgs.Length - 1;
        }
        s.sprite = bgs[currentBg];
        if (doFade)
        {
            // Debug.Log("BackgroundHandler: Fading in SFX and Ambience");
            bgFader.Fade();
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 1f));
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
        }
    }
}


[System.Serializable]
public class BackgroundChangeEvent : UnityEvent<string>
{
}
