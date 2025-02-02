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
    [SerializeField] private GameObject speakerContainer;
    [SerializeField] private TextMeshProUGUI dialogueTextWithSpeaker;
    [SerializeField] private TextMeshProUGUI dialougeTextNoSpeaker;

    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private AudioSource audioPlayer;

    [Header("References to created Choices and Hover")]
    [SerializeField] private DialogueChoice[] choices;
    [SerializeField] private DialogueHover hover;

    // speed at which characters appear in
    [Header("Box Settings")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float speakSpeed;

    private const string ERROR = "Invalid usage of dialogue box";
    private const string ALPHA = "<color=#00000000>";
    private const int SHOW_TEXT = -1000;

    private string dialogueString;
    private float scrollStartTime;
    private bool containerEnabled;
    private List<Action<int>> choiceListeners;
    private DialogueCharacter currentlySpeaking;
    private float speakStartTime;
    private TextMeshProUGUI currentDialogueText;

    public void Enable(bool enabled)
    {
        containerEnabled = enabled;
        container.SetActive(enabled);
    }

    /// <summary>
    /// Sets the dialogue text to the given strings
    /// </summary>
    /// <param name="speaker"></param>
    /// <param name="dialogueString"></param>
    /// <param name="scroll"></param>
    public void SetDialogue(string speaker, string dialogueString, bool scroll)
    {
        bool hasSpeaker = !string.IsNullOrEmpty(speaker);

        this.dialogueString = DialogueTree.FormatAsterisks(dialogueString);
        this.speakerText.text = speaker;

        dialogueTextWithSpeaker.text = string.Empty;
        dialougeTextNoSpeaker.text = string.Empty;

        if (hasSpeaker)
        {
            speakerContainer.SetActive(true);
            currentDialogueText = dialogueTextWithSpeaker;
        } else
        {
            speakerContainer.SetActive(false);
            currentDialogueText = dialougeTextNoSpeaker;
        }

        if (scroll)
        {
            scrollStartTime = Time.time;
        }
        else
        {
            scrollStartTime = SHOW_TEXT;
        }

        currentlySpeaking = null;
    }

    /// <summary>
    /// Sets the dialogue text to the given strings with a given character who may be speaking
    /// </summary>
    /// <param name="speaker">Who is currently speaking</param>
    /// <param name="dialogueString">text that they are saying</param>
    /// <param name="scroll">whether to scroll the text or not</param>
    public void SetDialogue(DialogueCharacter speaker, string dialogueString, bool scroll)
    {
        SetDialogue(speaker.GetCharacterName(), dialogueString, scroll);

        currentlySpeaking = speaker;
        speakStartTime = Time.time - speakSpeed;
    }

    /// <summary>
    /// Sets the given dialogue option's text
    /// </summary>
    /// <param name="choiceId">selected choice</param>
    /// <param name="text">text to put into the given choice</param>
    /// <param name="tooltip">tooltip of the given choice</param>
    public void SetChoice(int choiceId, string text, string tooltip)
    {
        choices[choiceId].Set(DialogueTree.FormatAsterisks(text), DialogueTree.FormatAsterisks(tooltip));
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

    /// <summary>
    /// Removes all choices from being displayed
    /// </summary>
    public void ClearChoices()
    {
        for (int i = 0; i < NumChoices(); i++)
        {
            SetChoice(i, null, null);
        }
    }

    /// <summary>
    /// Returns number of chocie options
    /// </summary>
    /// <returns></returns>
    public int NumChoices()
    {
        return choices.Length;
    }

    public bool IsScrolling()
    {
        return FinishedText() != ScrolledText();
    }

    public void FinishScrolling()
    {
        scrollStartTime = SHOW_TEXT;
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
        {
            currentDialogueText.text = ScrolledText();
            PlaySpeakSounds();
        }
    }

    /// <summary>
    /// Returns a string to be used in the the main dialogue text based on scroll time
    /// </summary>
    /// <returns>The formatted string</returns>
    private string ScrolledText()
    {
        float dt = Time.time - scrollStartTime;
        // number of characters to be displayed
        int chars = Mathf.Min((int)(dt / scrollSpeed), dialogueString.Length);

        // actual index to insert the alpha character
        int insertIndex = 0;

        // if there is a < increment the index by the number of characters in between
        int count = 0;
        while (count < chars && insertIndex < dialogueString.Length)
        {
            string sub = dialogueString[insertIndex..];

            // escaping just add length of escape to i
            if (sub[0] == '<')
            {
                int length = sub.IndexOf('>') + 1;
                if (length < -1)
                    insertIndex += sub.Length;
                else
                    insertIndex += length;
            }
            // not escaping, increment i and count characters
            else
            {
                insertIndex++;
                count++;
            }
        }

        return dialogueString.Insert(insertIndex, ALPHA);
    }

    /// <summary>
    /// Returns the finished string of text
    /// </summary>
    /// <returns>finished text displayed</returns>
    private string FinishedText()
    {
        return dialogueString + ALPHA;
    }

    private void PlaySpeakSounds()
    {
        if (currentlySpeaking == null || !IsScrolling())
            return;

        float now = Time.time;
        float dt = now - speakStartTime;

        if (dt > speakSpeed)
        {
            speakStartTime = now;
            audioPlayer.PlayOneShot(currentlySpeaking.GetRandomTalkSFX());
        }
    }

    public class DialogueBoxException : Exception
    {
        public DialogueBoxException() : base(ERROR)
        {

        }

        public DialogueBoxException(string message) : base($"{ERROR}. {message}")
        {

        }
    }
}