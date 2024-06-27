using System;
using UnityEngine;

//[RequireComponent(typeof(Collider2D))]
public class HighlightableObject : MonoBehaviour
{
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.gray;
    private Action onClick;
    public bool enableHighlight;
    public bool enableClick;

    public void OnClick(Action a)
    {
        this.onClick = a;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
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
