using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    public SpriteRenderer toHighlight;
    public Color defaultColor;
    public Color highlightColor;
    public int optionNumber;
    public DialogueController parent;


    private void OnMouseEnter()
    {
        toHighlight.color = highlightColor;
    }

    private void OnMouseExit()
    {
        toHighlight.color = defaultColor;
    }

    private void OnMouseDown()
    {
        parent.currentScene.chooseOption(optionNumber);
        OnMouseExit();
    }
}
