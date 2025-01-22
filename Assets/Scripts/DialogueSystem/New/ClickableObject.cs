using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : HighlightableObject
{
    [SerializeField] private DC controller;
    [SerializeField] private TextAsset clickText;

    private bool clicked;
    public override void OnMouseDown()
    {
        // only allows one click
        if (enableClick && !clicked)
        {
            onClick?.Invoke();

            controller.ClearDialogue();
            controller.AddDialogue(clickText);
            controller.StartDialogue();
            this.enableHighlight = false;
            this.enableClick = false;

            this.clicked = true;
        }
    }

    public void Update()
    {
        // if controller is running disable clicks and highlights
        bool done = controller.IsDone();
        this.enableClick = done && !clicked;
        this.enableHighlight = done && !clicked;
    }
}
