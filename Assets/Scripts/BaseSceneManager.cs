using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BaseSceneManager : MonoBehaviour
{

    public DialogueController d;
    public TextAsset t;
    public bool endOnDialogueEnd;
    public bool nextScene = false;

    [SerializeField] AudioMixer musicFadeMixer;

    public AudioClip sceneMusic; //music for this scene

    //script for fading out the scene to black, then next scene
    //LEAVE NULL IF NOT PLANNING TO FADE/WANT TO USE OTHER SCENE TRANSITIONS
    [SerializeField] FadeScript fadeToScene;

    // Start is called before the first frame update
    void Start()
    {
        if(d != null)
        {
            d.t = t;
            d.runDialogue();
        }
    }

    void Awake()
    {
        PlaySceneMusic();
    }

    private void Update()
    {
        if (nextScene)
        {
            nextScene = false;
            endScene();
        }
    }

    public void endScene()
    {
        if(fadeToScene != null)
        {
            fadeToScene.FadeToNextScene();
            return;
        }
        MapController.Instance.LoadNextScene("Map");
    }

    //Finds MusicPlayer singleton and plays music associated with this scene
    private void PlaySceneMusic()
    {
        if(sceneMusic != null)
        {
            GameObject musicObj = GameObject.Find("MusicPlayer");
            if(musicObj != null)
            {
                //Debug.Log("Attempting to get MusicPlayer script");
                MusicPlayer musicPlayer = musicObj.GetComponent<MusicPlayer>();
                
                if(musicPlayer != null)
                {
                    Debug.Log("BaseSceneManager: Fading in Music");
                    StartCoroutine(FadeMixerGroup.StartFade(musicFadeMixer, "MusicVolume", 1.5f, 1f));
                    musicPlayer.PlayMusic(sceneMusic);
                    //Debug.Log("Attempting to play music");
                }
            }
        }
    }
}
