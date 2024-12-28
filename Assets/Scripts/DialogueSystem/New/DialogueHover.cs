using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Follows the mouse and displays needed text
/// </summary>
public class DialogueHover : MonoBehaviour
{
    // [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform textBackground;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform textTransform;

    public void Set(bool enabled, string text)
    {
        container.gameObject.SetActive(enabled);
        this.text.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        // set container position to mouse position
        // note this function call below only works if canvas render mode is ScreenSpaceOverlay
        // if this changes, the new canvas camera must be used.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, null, out Vector2 anchoredPos);
        container.anchoredPosition = anchoredPos;
    }
}
