using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerniceSceneManager : MonoBehaviour
{

    public BerniceDialogueController d;
    public TextAsset t;
    public bool endOnDialogueEnd;
    public bool nextScene = false;

    public AudioClip sceneMusic; //music for this scene
    // Start is called before the first frame update
    void Start()
    {
        d.t = t;
        d.runDialogue();
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
        GameManager.Instance.MapAndSave();
    }

    //Finds AudioManager singleton and plays music associated with this scene
    private void PlaySceneMusic()
    {
        if(sceneMusic != null)
        {
            GameObject musicObj = GameObject.Find("MusicPlayer");
            if(musicObj != null)
            {
                AudioManager musicPlayer = musicObj.GetComponent<AudioManager>();
                if(musicPlayer != null)
                {
                    musicPlayer.PlayMusic(sceneMusic);
                }
            }
        }
    }
}
