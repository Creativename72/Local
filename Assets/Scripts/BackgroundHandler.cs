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

    private bool onBlack;
    private bool hasChangedBackground;

    private void Start()
    {
        currentBackground = 0;
        spriteRenderer.sprite = backgrounds[currentBackground];
        onBlack = false;
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

    /// <summary>
    /// Fades the screen to black and back 0.75 seconds for each phase
    /// </summary>
    public void FadeInOut()
    {
        if (onBlack)
        {
            StopAllCoroutines();
            bgFader.SetVisible();
        }
        
        StartCoroutine(ScreenChange());
    }
    public IEnumerator ScreenChange()
    {
        bgFader.SetVisible();
        onBlack = true;
        AudioManager.Instance.FadeTo(0, 0.75f, null, "SFXAutomate");
        bgFader.FadeBlack(true);
        yield return new WaitForSeconds(0.75f);
        AudioManager.Instance.FadeTo(1, 0.75f, null, "SFXAutomate");
        bgFader.FadeBlack(false);
        yield return new WaitForSeconds(0.75f);
        onBlack = false;
    }

    public void changeBackground(bool doFade = true)
    {
        if (onBlack)
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
            onBlack = true;
            // Debug.Log("BackgroundHandler: Fading out SFX and Ambience");
            AudioManager.Instance.FadeTo(0, 0.75f, null, "SFXAutomate");
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
            AudioManager.Instance.FadeTo(1, 0.75f, null, "SFXAutomate");
            yield return new WaitForSeconds(0.75f);
            onBlack = false;
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
