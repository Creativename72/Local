using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDialogue : MonoBehaviour
{
    public DialogueController d;
    public TextAsset t;
    
    bool dialogueViewed = false; //tracks if this item's dialogue was viewed/clicked on


    private void OnMouseDown()
    {
        if (!d.dialogueRunning)
        {
            d.t = t;
            d.runDialogue();
            dialogueViewed = true;
            
        } else
        {
            d.simulateMouseClick();
        }
    }

    public bool CheckViewed()
    {
        return dialogueViewed;
    }
}
