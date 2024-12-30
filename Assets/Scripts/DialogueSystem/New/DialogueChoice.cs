using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// One of three dialogue choices that can be selected in the dialogue prompt
/// </summary>
public class DialogueChoice : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image unlocked;
    [SerializeField] private Image locked;
    [SerializeField] private Button button;
    [SerializeField] private DialogueHover hover;

    private string tooltip;
    private bool isLocked;
    /// <summary>
    /// Sets this dialogue choice box's text
    /// </summary>
    /// <param name="text">text to be shown</param>
    /// <param name="tooltip">tooltip for this box</param>
    /// <param name="function">function to be invoked on this button click</param>
    public void Set(string text, string tooltip)
    {
        // if there is no text set the container to empty
        container.SetActive(!string.IsNullOrEmpty(text));

        isLocked = tooltip == null;
        locked.enabled = isLocked;
        unlocked.enabled = !isLocked;

        button.image = isLocked ? locked : unlocked;

        this.textUI.text = text;
        this.tooltip = tooltip;
    }
    public void HoverLockEnter()
    {
        if (!isLocked)
            hover.Set(true, tooltip);
    }

    public void HoverLockExit()
    {
        if (!isLocked)
            hover.Set(false, "");
    }
}
