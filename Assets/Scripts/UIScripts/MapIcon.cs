using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : HighlightableObject
{
    [Header("Map Icon Stuff")]
    public string day1Scene;
    public string day2Scene;
    public string day3Scene;

    [SerializeField] private Sprite selectableIcon;
    [SerializeField] private Sprite unselectableIcon;

    public bool HasVisited()
    {
        return enableClick;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableClick)
        {
            spriteRenderer.sprite = selectableIcon;
        }
        else
        {
            spriteRenderer.sprite = unselectableIcon;
        }
    }

    public override void OnMouseDown()
    {
        if (enableClick && onClick != null)
        {
            onClick.Invoke();
            enableHighlight = enableClick = false;
        }
    }
    public void EnableIcon(bool value)
    {
        this.spriteRenderer.enabled = value;
    }
}
