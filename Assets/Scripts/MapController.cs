using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;

public class MapController : MonoBehaviour
{
    public int currentStage;
    public bool[] HouseStates; // HouseStates order: Annie, Scout, Tyler, Walter
    public string currentScene;
    [SerializeField] SpriteRenderer s;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioMixer audioFader;
    [SerializeField] float audioFadeDuration;
    [SerializeField] FadeScript screenFader;
    public static MapController Instance { get; private set; }


// This should be on by default, turn it off if you want to start on a specific scene for testing purposes
    [SerializeField] bool LoadSceneOnStart;
    [SerializeField] string initialScene;
    public SceneChangeEvent m_sceneChangeEvent;

    private bool initialSkip = true;

    
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        currentStage = 0;
        HouseStates = new bool[4] {
            false, false, false, false
        };
        UpdateDay();

        if (m_sceneChangeEvent == null) {
            m_sceneChangeEvent = new SceneChangeEvent();
        }

        if (LoadSceneOnStart) {
            LoadNextScene(initialScene);
            initialSkip = false;
        }
    }

    public void LoadNextScene(string name) {
        StartCoroutine(LoadNextSceneAsync(name));
    }

    /** 
        Loads the next scene additively, waits until the load is finished, and then
        unloads the previous scene
    
        @param n Name of the next scene to load
    **/
    IEnumerator LoadNextSceneAsync(string n) {
        // Debug.Log("MapController: Fading out Music, SFX and Ambience");
        screenFader.ToggleFade();
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "MusicVolume", audioFadeDuration, 0f)); // Fade out sfx, music, ambience
        // StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 0f));

        if (!initialSkip) yield return new WaitForSeconds(1.5f);


        Camera.main.GetComponent<AudioListener>().enabled = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(n, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (currentScene != "-")
            SceneManager.UnloadSceneAsync(currentScene);
        
        s.enabled = false;
        currentScene = n;
        Debug.Log("Scene Loaded: " +currentScene);
        m_sceneChangeEvent.Invoke(n);

        // Debug.Log("MapController: Fading in Music, SFX and Ambience");
        // Start coroutines to fade in sfx and ambience but NOT music
        // Music fade in will be handled by the BaseSceneManagers for character scenes
        // StartCoroutine(FadeMixerGroup.StartFade(audioFader, "SFXVolume", audioFadeDuration, 1f));
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", audioFadeDuration, 1f));
        screenFader.ToggleFade();
    }

    public void UpdateDay() {
        if (!HouseStates[0] && !HouseStates[1] && 
            !HouseStates[2] && !HouseStates[3]) {
            currentStage++;
            Debug.Log("current stage:" + currentStage);

            switch(currentStage) {
                case 1:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 2:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
                case 3:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 4:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
                case 5:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 6:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
        }
        }
    }


}


[System.Serializable]
public class SceneChangeEvent : UnityEvent<string>
{
}
