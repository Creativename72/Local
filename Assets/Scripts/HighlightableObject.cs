using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class HighlightableObject : MonoBehaviour
{
    [Header("Customization")]
    
    [SerializeField] protected Color defaultColor = Color.white;
    [SerializeField] protected Color highlightColor = Color.gray;
    protected Action onClick;
    [Header("Click / Highlight")]
    protected bool visible = true;
    protected bool enableHighlight = true;
    protected bool enableClick = true;
    protected SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
    }
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

    public void SetVisible(bool visible)
    {
        this.visible = visible;
        this.spriteRenderer.enabled = visible;
    }
}
