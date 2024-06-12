using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAllItemsDialogue : MonoBehaviour
{
    public TextAsset postItemsDialogue;
    public DialogueController d;

    bool allItemsChecked = false;  //tracks if all items have been checked
    public ClickDialogue[] items;

    bool thisDialoguePlayed = false;  //tracks if the post minigame dialogue has been shown

    void Update()
    {
        if (!allItemsChecked)
        {
            foreach (ClickDialogue item in items)
            {
                if (!item.CheckViewed()) return;
            }
            allItemsChecked = true;
        }
        else if (!thisDialoguePlayed)
        {
            if (!d.dialogueRunning)
            {
                d.t = postItemsDialogue;
                d.runDialogue();
                thisDialoguePlayed = true;

            }
        }
    }
}
