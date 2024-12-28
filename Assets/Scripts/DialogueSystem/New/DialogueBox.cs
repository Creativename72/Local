using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the dialogue box as a whole
/// </summary>
public class DialogueBox : MonoBehaviour
{
    // holding all the pieces of the dialogue box
    [Header("Attatched GameObjects")]
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [Header("References to created Choices and Hover")]
    [SerializeField] private DialogueChoice[] choices;
    [SerializeField] private DialogueHover hover;

    // speed at which characters appear in
    [Header("Box Settings")]
    [SerializeField] private float scrollSpeed;

    private const string ERROR = "Invalid usage of dialogue box";

    private string dialogueString;
    private float scrollStartTime;
    private bool containerEnabled;
    private List<Action<int>> choiceListeners;

    public void Enable(bool enabled)
    {
        containerEnabled = enabled;
        container.SetActive(enabled);
    }

    /// <summary>
    /// Sets the dialogue text to the given strings
    /// </summary>
    /// <param name="speaker">Who is currently speaking</param>
    /// <param name="dialogueString">text that they are saying</param>
    /// <param name="scroll">whether to scroll the text or not</param>
    public void SetDialogue(string speaker, string dialogueString, bool scroll)
    {
        this.dialogueString = dialogueString;
        this.speakerText.text = speaker;

        if (scroll)
        {
            scrollStartTime = Time.time;
        }
        else
        {
            scrollStartTime = -1000;
        }
    }

    /// <summary>
    /// Sets the given dialogue option
    /// </summary>
    /// <param name="choiceId">selected choice</param>
    /// <param name="text">text to put into the given choice</param>
    /// <param name="tooltip">tooltip of the given choice</param>
    public void SetChoice(int choiceId, string text, string tooltip)
    {
        choices[choiceId].Set(text, tooltip);
    }

    /// <summary>
    /// Adds a listener to one of the dialogue choice options
    /// </summary>
    /// <param name="listener"></param>
    public void AddChoiceListener(Action<int> listener)
    {
        choiceListeners.Add(listener);
    }

    /// <summary>
    /// Used by the choice buttons to invoke choice listeners
    /// </summary>
    /// <param name="choiceId"></param>
    public void ButtonChoice(int choiceId)
    {
        choiceListeners.ForEach(choiceFunc => choiceFunc.Invoke(choiceId));
    }

    public void Awake()
    {
        dialogueString = string.Empty;
        scrollStartTime = -1000;
        choiceListeners = new();

        hover.Set(false, "");
        Enable(false);
        for (int i = 0; i < choices.Length; i++)
            SetChoice(i, null, null);
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (containerEnabled)
            UpdateText();
    }

    /// <summary>
    /// Updates the main dialogue text based on scroll time
    /// </summary>
    private void UpdateText()
    {
        float dt = Time.time - scrollStartTime;
        int chars = Mathf.Min((int)(dt / scrollSpeed), dialogueString.Length);

        dialogueText.text = dialogueString.Insert(chars, "<color=#00000000>");
    }


    public class DialogueBoxException : Exception
    {
        public DialogueBoxException() : base(ERROR)
        {

        }
    }
}