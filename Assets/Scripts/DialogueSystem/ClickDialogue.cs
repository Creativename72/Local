using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDialogue : MonoBehaviour
{
    public DialogueController d;
    public TextAsset t;
    public SpriteRenderer s;
    
    bool dialogueViewed = false; //tracks if this item's dialogue was viewed/clicked on
    public bool hideOnClick = false;

    private void OnMouseDown()
    {
        if (!d.dialogueRunning)
        {
            d.t = t;
            d.runDialogue();
            dialogueViewed = true;
            if (hideOnClick)
            {
                this.gameObject.SetActive(false);
                s.enabled = false;
            }
            
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
