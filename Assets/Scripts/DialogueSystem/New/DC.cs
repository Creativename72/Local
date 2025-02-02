using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool dialogueEnabled;
    private bool printDialogue;
    private bool startedDialogueThisFrame;

    private void Awake()
    {
        tree = new();
        characters = new();
        dialogueEnabled = true;
        printDialogue = false;
    }
    public DialogueCharacter CreateCharacter(DialogueCharacter character)
    {
        DialogueCharacter created = characterController.CreateCharacter(character);
        characters.Add(character.GetIdentifier(), created);
        return created;
    }
    public void AddGameFlags()
    {
        GameStateManager.Instance.ForEachFlag<bool>((name, val) =>
        {
            this.SetVariable(name, () => GameStateManager.Instance.GetFlag<bool>(name));
        });
    }

    public void AddDefaultFunctions(BackgroundHandler backgroundHandler = null, AmbienceInScene ambience = null, PlayDialogueSFX audioPlayer = null)
    {
        if (ambience != null)
        {
            this.AddFunction("ChangeAmbience", () => ambience.ChangeAmbience());
            this.AddFunction("changeAmbience", () => ambience.ChangeAmbience());
        }
        if (backgroundHandler != null)
        {
            this.AddFunction("changeBackground", () => backgroundHandler.changeBackground());
            this.AddFunction("ChangeBackground", () => backgroundHandler.changeBackground());
        }

        this.AddFunction("pause1", () =>
        {
            Pause(1);
        });
        this.AddFunction("pause2", () =>
        {
            Pause(2);
        });
        this.AddFunction("pause3", () =>
        {
            Pause(3);
        });
        this.AddFunction("SetFlag", (arguments) =>
        {
            if (arguments.Length != 2)
            {
                throw new DialogueControllerException("SetFlag() must have 2 arguments.");
            }
            string flagName = arguments[0];
            bool parsed = bool.TryParse(arguments[1], out bool value);
            if (!parsed)
            {
                throw new DialogueControllerException("SetFlag() did not parse bool correctly.");
            }
            Debug.Log($"Setting flag {flagName} to {value}");
            GameStateManager.Instance.SetFlag(flagName, value);
        });
        this.AddFunction("showCharacters", () => characters.Values.ToList().ForEach(c => c.SetVisible(true)));
        this.AddFunction("showCharactersFade", () => StartCoroutine(Wait(0.75f, () => characters.Values.ToList().ForEach(c => c.SetVisible(true)))));
        this.AddFunction("hideCharacters", () => characters.Values.ToList().ForEach(c => c.SetVisible(false)));
        this.AddFunction("hideCharactersFade", () => StartCoroutine(Wait(0.75f, () => characters.Values.ToList().ForEach(c => c.SetVisible(false)))));
        // set walter visible with a wait
        // Show {Walter, true, 0.75}
        this.AddFunction("Show", (args) =>
        {
            if (args.Length < 2)
            {
                Debug.LogWarning("Not enough arguments for show funtion.");
                return;
            }
            float wait = 0;
            if (args.Length >= 3)
            {
                try
                {
                    wait = float.Parse(args[2]);
                }
                catch (Exception)
                {
                    Debug.LogWarning("Unable to parse time for show function.");
                }
            }
            string character = args[0];
            bool value = bool.Parse(args[1]);

            if (!characters.ContainsKey(character))
            {
                Debug.LogWarning("Character has not yet been added to the dialogue controller.");
                return;
            }

            GameManager.WaitRoutine(wait, () => characters[character].SetVisible(value));
        });
        this.AddFunction("Move", (args) =>
        {
            if (args.Length < 2)
            {
                Debug.LogWarning("Not enough arguments for move funtion.");
                return;
            }
            float wait = 0;
            if (args.Length >= 3)
            {
                try
                {
                    wait = float.Parse(args[2]);
                }
                catch (Exception)
                {
                    Debug.LogWarning("Unable to parse time for move function.");
                }
            }
            string character = args[0];
            if (!characters.ContainsKey(character))
            {
                Debug.LogWarning("Character has not yet been added to the dialogue controller.");
                return;
            }
            if (!Enum.TryParse(args[1], out DialogueCharacter.Location location))
            {
                Debug.LogWarning("Unable to parse location from move function.");
                return;
            }

            GameManager.WaitRoutine(wait, () => characters[character].SetLocation(location));
        });
        this.AddFunction("PlaySound", args =>
        {
            if (args.Length < 1)
            {
                Debug.LogWarning("Not enough arguments to execute PlaySound.");
                return;
            }

            if (audioPlayer == null)
            {
                Debug.LogWarning("Audio player not defined in default functions.");
                return;
            }

            audioPlayer.PlaySFX(args[0]);
        });
        this.AddFunction("FadeScreen", () =>
        {
            if (backgroundHandler == null)
            {
                Debug.LogWarning("Background handler added to default functions for FadeScreen.");
                return;
            }
            backgroundHandler.FadeInOut();
        });
    }
    public void Pause(float seconds)
    {
        this.Enable(false);
        StartCoroutine(Wait(seconds, () => this.Enable(true)));
    }

    private IEnumerator Wait(float seconds, Action a)
    {
        yield return new WaitForSeconds(seconds);
        a.Invoke();
    }

    /// <summary>
    /// Adds dialogue to the controller
    /// </summary>
    /// <param name="text">text asset to be added</param>
    public void AddDialogue(TextAsset text, bool returnToExisting = true)
    {
        tree.Parse(text, returnToExisting);
    }
    public void SetVariable(string varName, DialogueTree.Variable variable)
    {
        tree.SetVariable(varName, variable);
    }
    public void SetVariable(string varName, Func<bool> func)
    {
        tree.SetVariable(varName, func);
    }
    public void AddFunction(string func, Action action)
    {
        tree.SetFunction(func, action);
    }
    public void AddFunction(string func, Action<string[]> action)
    {
        tree.SetFunction(func, action);
    }

    /// <summary>
    /// Enables / disables interactions with the dialogue box, closing it / opening it
    /// </summary>
    /// <param name="enable">enable / disable dialogue</param>
    public void Enable(bool enable)
    {
        dialogueEnabled = enable;
        dialogueBox.Enable(enable);

        if (enable)
            SetDialogue();
    }

    /// <summary>
    /// Adds an function to be invoked upon dialogue ending
    /// </summary>
    /// <param name="action">function invoked</param>
    public void AddEndFunction(Action action)
    {
        tree.AddCloseListener(action);
    }
    public void ClearEndFunctions()
    {
        tree.ClearCloseListeners();
    }

    public void ReparseOnNodeChange(bool value)
    {
        this.tree.ReparseOnNodeChange(value);
    }
    public void ClearDialogue()
    {
        this.tree.ClearNodes();
        this.dialogueBox.SetDialogue("", "", false);
    }
    public bool IsDone()
    {
        return this.tree.IsDone();
    }
    public void EnablePrint(bool enable)
    {
        this.printDialogue = enable;
    }

    /// <summary>
    /// Starts the dialogue controller
    /// </summary>
    /// <exception cref="DialogueControllerException">If there is no dialogue input given</exception>
    public void StartDialogue()
    {
        startedDialogueThisFrame = true;

        if (tree.IsEmpty())
            throw new DialogueControllerException("Dialogue controller has no input");

        SetDialogue();
    }

    private void Start()
    {
        // called on an option click
        dialogueBox.AddChoiceListener(selected =>
        {
            // only be able to select choices if dialogue is canVisit
            if (dialogueEnabled)
            {
                int index = 0;
                DialogueTree.Line line = tree.GetLine();

                if (line is DialogueTree.OptionalLine line1)
                {
                    int c = line1.GetOptions().Count();
                    index = GetOptionalIndex(selected, c);
                }

                tree.AdvanceLine(index);
                SetDialogue();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        // skip frame when just started dialogue
        if (startedDialogueThisFrame)
        {
            startedDialogueThisFrame = false;
            return;
        }

        // called on a mouse click
        // only be able to advance if dialogue is canVisit, and game is not paused
        if (Input.GetMouseButtonDown(0) && this.dialogueEnabled && !GameManager.Instance.Paused())
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

        if (line == null || !dialogueEnabled)
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
                int optionsIndex = GetOptionalIndex(i, options.Count());
                if (optionsIndex >= 0)
                {
                    Option o = options[optionsIndex];
                    dialogueBox.SetChoice(i, o.option, o.tooltip);
                }
                else
                {
                    dialogueBox.SetChoice(i, null, null);
                }
            }
        }

        if (printDialogue)
            PrintDialogue();
    }

    public int GetOptionalIndex(int choiceIndex, int optionsCount)
    {
        return optionsCount - 1 - choiceIndex;
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