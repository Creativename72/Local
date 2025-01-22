using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnnieDay2 : MonoBehaviour
{
    [Header("StartMinigame")]
    [SerializeField] GameObject minigame;
    [SerializeField] TextAsset textDayPart1;
    [SerializeField] TextAsset textDayPart2;
    [SerializeField] List<ClickableObject> clickables;

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

    void Start()
    {
        // setup
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        annie = dialogueController.CreateCharacter(annieCharacter);


        annie.SetName("Annie");
        annie.SetSprite(1);
        annie.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        scout.SetVisible(true);
        clickables.ForEach(clickable => clickable.OnClick(() => itemsInspected++));

        GameFlags.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameFlags.GetFlag<bool>(name));
        });

        bool askedLostVision = false;
        bool askedOtherResidents = false;

        dialogueController.SetVariable("AskedAllQuestions", () => askedLostVision && askedOtherResidents);
        dialogueController.SetVariable("NotAskedLostVision", () => !askedLostVision);
        dialogueController.SetVariable("NotAskedOtherResidents", () => !askedOtherResidents);
        dialogueController.AddFunction("LostVisionAsked", () => askedLostVision = true);
        dialogueController.AddFunction("OtherResidentsAsked", () => askedOtherResidents = true);
        dialogueController.AddFunction("ShowAnnie", () => annie.SetVisible(true));
        dialogueController.AddFunction("EndEarly", () =>
        {
            dialogueController.ClearEndFunctions();
            dialogueController.ClearDialogue();
            GameManager.Instance.ChangeScene("Map");
        });
        // does nothing for now but if we have a display for the items we can add functionality
        dialogueController.AddFunction("ChangeSisterName", () => { });
        minigame.SetActive(false);
        itemsInspected = 0;

        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);
        dialogueController.AddDialogue(textDayPart1);
        dialogueController.StartDialogue();
        dialogueController.ReparseOnNodeChange(true);
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
        if (itemsInspected >= 5 && dialogueController.IsDone())
        {
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
            dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        }

        if (inMinigame)
        {
            annie.SetVisible(!dialogueController.IsDone());
        }
    }
}
