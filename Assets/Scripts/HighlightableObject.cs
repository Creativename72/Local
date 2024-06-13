using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HighlightableObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color defaultColor = Color.gray;
    [SerializeField] private Color highlightColor = Color.white;
    private Action onClick;

    public void OnClick(Action a)
    {
        this.onClick = a;
    }

    private void OnMouseEnter()
    {
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
    }

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
