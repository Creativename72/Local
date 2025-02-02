using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HighlightableObject : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.gray;
    protected Action onClick;
    [Header("Click / Highlight")]
    protected bool enableHighlight = true;
    protected bool enableClick = true;

    public void OnClick(Action a)
    {
        this.onClick = a;
    }

    private void OnMouseEnter()
    {
        // Debug.Log("Mouse enter");
        if (enableHighlight)
            spriteRenderer.color = highlightColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = defaultColor;
    }

    public virtual void OnMouseDown()
    {
        if (enableClick && onClick != null)
            onClick.Invoke();
    }

    public void EnableHighlight(bool enableHighlight)
    {
        this.enableHighlight = enableHighlight;
        if (!enableHighlight)
            spriteRenderer.color = defaultColor;
    }

    public void EnableClick(bool enableClick)
    {
        this.enableClick = enableClick;
    }
}
