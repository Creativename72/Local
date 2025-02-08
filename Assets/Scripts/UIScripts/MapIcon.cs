using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapIcon : HighlightableObject
{
    [Header("Map Icon Stuff")]
    public string day1Scene;
    public string day2Scene;
    public string day3Scene;

    [SerializeField] private Sprite visitedIcon;
    [SerializeField] private Sprite unvisitedIcon;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisited(bool visited)
    {
        if (visited)
        {
            spriteRenderer.sprite = visitedIcon;
        }
        else
        {
            spriteRenderer.sprite = unvisitedIcon;
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
