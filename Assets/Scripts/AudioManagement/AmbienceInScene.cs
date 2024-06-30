using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AmbienceInScene : MonoBehaviour
{
    [SerializeField] AudioSource ambiencePlayer;
    private AudioSource tempPlayer;
    [SerializeField] AudioMixer audioFader;
    [SerializeField] float audioFadeDuration;

    [SerializeField] List<AudioClip> progressionalAmbience;
    [SerializeField] int ambienceIndex = -1;
    [SerializeField] AudioMixerGroup tempGroup;


    // Start is called before the first frame update
    void Start()
    {
        ambiencePlayer = GetComponent<AudioSource>();
        ambiencePlayer.clip = progressionalAmbience[ambienceIndex];
        ambiencePlayer.Play(0);
    }

    public void ChangeAmbience() {
        StartCoroutine(AmbienceChange());
    }

    private IEnumerator AmbienceChange() {
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));
        yield return new WaitForSeconds(audioFadeDuration);

        ambienceIndex++;
        ambiencePlayer.clip = progressionalAmbience[ambienceIndex];
        ambiencePlayer.Play(0);

        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
    }

    public void ChangeAmbience(string n) {
        StartCoroutine(AmbienceChange(n));
    }

    private IEnumerator AmbienceChange(string name) {
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));
        yield return new WaitForSeconds(audioFadeDuration);

        AudioClip jumpTo = Resources.Load<AudioClip>("SFX/"+name);
        if (progressionalAmbience.Contains(jumpTo)) {
            ambienceIndex = progressionalAmbience.IndexOf(jumpTo);
            ambiencePlayer.clip = progressionalAmbience[ambienceIndex];
            ambiencePlayer.Play(0);
        }

        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
    }

    public void PlayTempAmbience(string n) {
        StartCoroutine(TempAmbiencePlayer(n));
        Debug.Log("temp played");
    }

    private IEnumerator TempAmbiencePlayer(string name) {
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "TempAmbienceVolume", audioFadeDuration, 0f));
        yield return new WaitForSeconds(audioFadeDuration);

        Debug.Log("SFX/" +name);
        tempPlayer = gameObject.AddComponent<AudioSource>();
        AudioClip test = Resources.Load<AudioClip>("SFX/"+name);
        tempPlayer.clip = test;
        tempPlayer.loop = true;
        tempPlayer.outputAudioMixerGroup = tempGroup;
        tempPlayer.Play(0);

        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "TempAmbienceVolume", audioFadeDuration, 1f));
    }

    public void StopTempAmbience() {
        StartCoroutine(TempAmbienceStopper());
    }

    private IEnumerator TempAmbienceStopper() {
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "TempAmbienceVolume", audioFadeDuration, 0f));
        yield return new WaitForSeconds(audioFadeDuration);

        if (tempPlayer != null)
            Destroy(tempPlayer);

        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "TempAmbienceVolume", audioFadeDuration, 1f));
    }
}
