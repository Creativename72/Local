using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HighlightableObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] private Color highlightColor = Color.white;
    private Action onClick;
    private bool enableHighlight;
    private bool enableClick;

    public void OnClick(Action a)
    {
        this.onClick = a;
    }

    private void OnMouseEnter()
    {
        if (enableHighlight)
            sprite.color = highlightColor;
    }

    private void OnMouseExit()
    {
        sprite.color = defaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite.color = defaultColor;
        enableHighlight = true;
        enableClick = true;
    }

    private void OnMouseDown()
    {
        if (enableClick)
            onClick.Invoke();
    }

    public void EnableHighlight(bool enableHighlight)
    {
        this.enableHighlight = enableHighlight;
        if (!enableHighlight)
            sprite.color = defaultColor;
    }

    public void EnableClick(bool enableClick)
    {
        this.enableClick = enableClick;
    }
}
