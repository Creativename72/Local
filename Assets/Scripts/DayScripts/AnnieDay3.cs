using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potentially the last script to be written yippeeeeeeeeeeeeeeeeeeeeeeeeeeee!!!
/// </summary>
public class AnnieDay3 : MonoBehaviour
{
    [Header("General Day")]
    [SerializeField] GameObject minigame;
    [SerializeField] TextAsset textDayPart1;
    [SerializeField] TextAsset textDayPart2;

    [Header("Clickables")]
    [SerializeField] List<ClickableObject> clickables;
    [SerializeField] ClickableObject mysteryJars;

    [Header("Dialogue System")]
    [SerializeField] private DC dialogueController;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter annieCharacter;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;


    private DialogueCharacter scout;
    private DialogueCharacter narrator;
    private DialogueCharacter annie;

    private int itemsInspected;
    private bool inMinigame;
    private bool inPart2;

    void Start()
    {
        // setup
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        annie = dialogueController.CreateCharacter(annieCharacter);

        annie.SetSprite(1);
        annie.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        scout.SetVisible(true);

        // minigame init
        clickables.ForEach(clickable => clickable.OnClick(() =>
        {
            itemsInspected++;
            clickable.SetVisible(false);
        }));
        mysteryJars.OnClick(() => itemsInspected++);

        minigame.SetActive(false);
        itemsInspected = 0;

        dialogueController.AddGameFlags();
        int count = 1; // one option always shows

        // we want these options to always show
        if (GameStateManager.Instance.GetFlag<bool>("AnnieMovesForward")) count++;
        if (GameStateManager.Instance.GetFlag<bool>("AnnieWalterBreak")) count++;
        if (GameStateManager.Instance.GetFlag<bool>("AnnieMissesCity")) count++;

        bool show = count < 3;

        // if space show the last one
        dialogueController.SetVariable("ShowYouNeedBreak", () => show);
        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);


        dialogueController.AddDialogue(textDayPart1);
        dialogueController.StartDialogue();

        dialogueController.AddEndFunction(() =>
        {
            backgroundHandler.changeBackground(true);
            GameManager.WaitRoutine(0.75f, () =>
            {
                annie.SetVisible(false);
                scout.SetVisible(false);
                minigame.SetActive(true);
                inMinigame = true;
            });
            dialogueController.ClearEndFunctions();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (itemsInspected >= clickables.Count + 1 && dialogueController.IsDone() && !inPart2)
        {
            inPart2 = true;
            dialogueController.ClearDialogue();
            dialogueController.AddDialogue(textDayPart2);
            dialogueController.StartDialogue();
            backgroundHandler.changeBackground(true);
            GameManager.WaitRoutine(0.75f, () =>
            {
                inMinigame = false;
                annie.SetVisible(true);
                scout.SetVisible(true);
                minigame.SetActive(false);
            });
            dialogueController.ClearEndFunctions();
            dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        }

        if (inMinigame)
        {
            annie.SetVisible(!dialogueController.IsDone());
        }
    }
}
