using System;
using System.Collections.Generic;
using UnityEngine;
using static DialogueTree.OptionalLine;

/// <summary>
/// Controls the dialogue system as a whole
/// </summary>
public class DC : MonoBehaviour
{

    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private CharacterController characterController;


    private const string INVALID_STATE = "Dialogue controller in invalid state";

    private DialogueTree tree;
    // characters that have been loaded into this dialogue system
    private Dictionary<string, DialogueCharacter> characters;

    private void Awake()
    {
        tree = new();
        characters = new();
    }
    public void CreateCharacter(DialogueCharacter character)
    {
        characterController.CreateCharacter(character);
        characters.Add(character.GetIdentifier(), character);
    }

    /// <summary>
    /// Adds dialogue to the controller
    /// </summary>
    /// <param name="text">text asset to be added</param>
    public void AddDialogue(TextAsset text)
    {
        tree.Parse(text, true);
    }
    public void SetVariable(string varName, DialogueTree.Variable variable)
    {
        tree.SetVariable(varName, variable);
    }
    public void SetFunction(string func, Action action)
    {
        tree.SetFunction(func, action);
    }

    /// <summary>
    /// Starts the dialogue controller
    /// </summary>
    /// <exception cref="DialogueControllerException">If there is no dialogue input given</exception>
    public void StartDialogue()
    {
        if (tree.IsEmpty())
            throw new DialogueControllerException("Dialogue controller has no input");

        SetDialogue();

        // called on an option click
        dialogueBox.AddChoiceListener(selected =>
        {
            tree.AdvanceLine(selected);
            SetDialogue();
        });
    }

    // Update is called once per frame
    void Update()
    {
        // called on a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            if (tree.GetLine() is DialogueTree.DialogueLine dLine)
            {
                if (dialogueBox.IsScrolling())
                {
                    dialogueBox.FinishScrolling();
                }
                else
                {
                    tree.AdvanceLine(0);
                    SetDialogue();
                }
            }
        }
    }

    private void SetDialogue()
    {
        DialogueTree.Line line = tree.GetLine();

        if (line == null)
        {
            dialogueBox.Enable(false);
            return;
        }

        dialogueBox.Enable(true);

        // set the dialogue
        if (line is DialogueTree.DialogueLine dLine)
        {
            // if the character is set set the speakers name to that characters name
            string speaker = dLine.GetCharacter();
            characters.TryGetValue(speaker, out DialogueCharacter character);
            if (character != null)
            {
                dialogueBox.SetDialogue(character, dLine.GetDialogue(), true);
            }
            else
            {
                dialogueBox.SetDialogue(speaker, dLine.GetDialogue(), true);
            }
            dialogueBox.ClearChoices();
        }

        // set the options
        else if (line is DialogueTree.OptionalLine oline)
        {
            List<Option> options = oline.GetOptions();
            for (int i = 0; i < dialogueBox.NumChoices(); i++)
            {
                if (i < options.Count)
                {
                    Option o = options[i];
                    dialogueBox.SetChoice(i, o.option, o.tooltip);
                }
                else
                {
                    dialogueBox.SetChoice(i, null, null);
                }
            }
        }

        PrintDialogue();
    }

    private void PrintDialogue()
    {
        DialogueTree.Line line = tree.GetLine();

        if (line == null)
        {
            Debug.Log("END");
        }
        else
        {
            Debug.Log(line.ToString());
        }
    }

    public class DialogueControllerException : Exception
    {
        public DialogueControllerException(string message) : base($"{INVALID_STATE}. {message}")
        {

        }
    }
}

