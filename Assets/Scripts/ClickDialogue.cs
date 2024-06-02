using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDialogue : MonoBehaviour
{
    public DialogueController d;
    public TextAsset t;


    private void OnMouseDown()
    {
        if (!d.dialogueRunning)
        {
            d.t = t;
            d.runDialogue();
        } else
        {
            d.simulateMouseClick();
        }
    }
}
