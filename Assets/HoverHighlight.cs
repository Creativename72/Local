using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    public SpriteRenderer toHighlight;
    public Color defaultColor;
    public Color highlightColor;


    private void OnMouseEnter()
    {
        Debug.Log("a");
        toHighlight.color = highlightColor;
    }

    private void OnMouseExit()
    {
        Debug.Log("b");
        toHighlight.color = defaultColor;
    }
}
