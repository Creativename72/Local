using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnieMinigameChecker : MonoBehaviour
{

    public TextAsset anniePostMinigameText;
    public DialogueController d;

    bool allItemsChecked = false;  //tracks if all items have been checked
    public ClickDialogue[] items;

    bool thisDialoguePlayed = false;  //tracks if the post minigame dialogue has been shown
    public BaseSceneManager b;

    [SerializeField] BackgroundHandler bgHandler; //background handler for the scene

    void Update()
    {
        if (bgHandler != null && bgHandler.currentBackground == 2)
        {
            enableSprites();
        }
        else
        {
            disableSprites();
        }

        if (!allItemsChecked)
        {
            foreach(ClickDialogue item in items)
            {
                if (!item.CheckViewed()) return;
            }
            allItemsChecked = true;
        } else if (!thisDialoguePlayed)
        {
            if(!d.dialogueRunning)
            {
                d.t = anniePostMinigameText;
                d.runDialogue();
                thisDialoguePlayed = true;
                b.endOnDialogueEnd = true;
            }
        }
    }

    public void enableSprites()
    {
        foreach (var item in items)
        {
            item.s.enabled = true;
        }
    }

    void disableSprites()
    {
        foreach(var item in items)
        {
            item.s.enabled = false;
        }
    }
}
