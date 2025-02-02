using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TylerDay3Manager : MonoBehaviour
{
    [SerializeField] private TextAsset text;
    [SerializeField] private DialogueController dialogueController;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject game;

    public PlayDialogueSFX sfxPlayer;

    private State currentState;
    private int harvestCount;
    private int winCount;

    // public AudioClip sceneMusic; //music for this scene

    private enum State
    {
        IntroDialogue,
        Minigame,
        PostDialogue,
    }
    void Start()
    {
        // set up the world
        background.SetActive(true);
        game.SetActive(false);

        // PlaySceneMusic();//play scene music

        int childCount = game.transform.childCount;
        // go through each child and if their name is one of the selectable ones add a function to increase score on click
        for (int i = 0; i < childCount; i++)
        {
            GameObject go = game.transform.GetChild(i).gameObject;
            HighlightableObject highlight = go.GetComponent<HighlightableObject>();
            // disable all clicks and highlights to start
            highlight.EnableClick(false);
            highlight.EnableHighlight(false);
            if (go.name.Contains("Potato") || go.name.Contains("Carrots") || go.name.Contains("Onions"))
            {
                // count needeed to win
                winCount++;
                highlight.OnClick(() =>
                {
                    // increase count and disable object click don't know yet how we want to handle the harvesting of vegetables visually
                    harvestCount++;
                    highlight.EnableClick(false);
                    highlight.EnableHighlight(false);
                    highlight.spriteRenderer.enabled = false;
                    sfxPlayer.PlaySFX("s_harvest");
                });
            }
            else
            {
                highlight.OnClick(() =>
                {
                    // disable highlighting if can't harvest it
                    highlight.EnableHighlight(false);
                });
            }
        }

        dialogueController.t = text;

        // add all the functions
        dialogueController.AddFunction("pause_3", () =>
        {
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(3, () => dialogueController.ResumeDialogue()));
        });
        dialogueController.AddFunction("begin_game", () =>
        {
            background.SetActive(false);
            game.SetActive(true);
        });
        dialogueController.AddFunction("begin_harvest", () =>
        {
            currentState = State.Minigame;
            dialogueController.PauseDialogue();
            // enable all the vegetables to be clicked
            int childCount = game.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                HighlightableObject highlight = game.transform.GetChild(i).gameObject.GetComponent<HighlightableObject>();
                // disable all clicks and highlights to start
                highlight.EnableClick(true);
                highlight.EnableHighlight(true);
            }
        });
        // can decide how to implement later
        dialogueController.AddFunction("focus_left_plot", () =>
        {
            Debug.Log("Looking left plot");
        });
        dialogueController.AddFunction("focus_right_plot", () =>
        {
            Debug.Log("Looking right plot");
        });
        dialogueController.AddFunction("unfocus_plot", () =>
        {
            Debug.Log("Unfocusing plot");
        });
        dialogueController.AddFunction("exit_garden", () =>
        {
            background.SetActive(true);
            game.SetActive(false);
        });
        dialogueController.runDialogue();
        currentState = State.IntroDialogue;
    }
    private IEnumerator Wait(float time, Action after)
    {
        yield return new WaitForSeconds(time);
        after.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.Minigame && winCount == harvestCount)
        {
            currentState = State.PostDialogue;
            StartCoroutine(Wait(2, () => dialogueController.ResumeDialogue()));
        }
    }


    /*
    //Only for scene music
    //Finds MusicPlayer singleton and plays music associated with this scene
    private void PlaySceneMusic()
    {
        if (sceneMusic != null)
        {
            GameObject musicObj = GameObject.Find("MusicPlayer");
            if (musicObj != null)
            {
                MusicPlayer musicPlayer = musicObj.GetComponent<MusicPlayer>();
                if (musicPlayer != null)
                {
                    musicPlayer.PlayMusic(sceneMusic);
                }
            }
        }
    }
    */
}
