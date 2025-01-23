using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalterDay3 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private DialogueCharacter scoutCreator;
    [SerializeField] private DialogueCharacter walterCreator;
    [SerializeField] private DialogueCharacter narratorCreator;
    [SerializeField] private AmbienceInScene ambienceInScene;
    [SerializeField] private BackgroundHandler backgroundHandler;

    private DialogueCharacter scout;
    private DialogueCharacter walter;
    private DialogueCharacter narrator;

    private bool waitForCast;
    private bool waitForReel;

    // Start is called before the first frame update
    void Start()
    {
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        walter = dialogueController.CreateCharacter(walterCreator);

        scout.SetVisible(true);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        walter.SetLocation(DialogueCharacter.Location.LEFT);

        GameFlags.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameFlags.GetFlag<bool>(name));
        });
        dialogueController.AddFunction("FishingCast", () =>
        {
            dialogueController.Enable(false);
            GameManager.WaitRoutine(0.5f, () =>
            {
                waitForCast = true;
            });
        });
        dialogueController.AddFunction("FishingBite", () =>
        {
            dialogueController.Enable(false);

            // fish bites after 3 seconds
            GameManager.WaitRoutine(3f, () =>
            {
                backgroundHandler.changeBackground(false);
                waitForReel = true;
            });
        });
        dialogueController.AddFunction("ShowScout", () =>
        {
            GameManager.WaitRoutine(0.75f, () =>
            {
                walter.SetVisible(false);
                scout.SetVisible(true);
            });
        });
        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueController.AddDialogue(dialogueText);
        dialogueController.ReparseOnNodeChange(true);
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // casting
        if (waitForCast && Input.GetButtonDown("Fire1"))
        {
            backgroundHandler.changeBackground(false);
            GameManager.WaitRoutine(2f, () =>
            {
                backgroundHandler.changeBackground(false);
                dialogueController.Enable(true);
            });
            waitForCast = false;
        }
        // reeling the fish
        if (waitForReel && Input.GetButtonDown("Fire1"))
        {
            // show the fish and then change background after 3 seconds
            backgroundHandler.changeBackground(false);

            GameManager.WaitRoutine(3, () =>
            {
                backgroundHandler.changeBackground(true);
            });
            GameManager.WaitRoutine(3.75f, () =>
            {
                dialogueController.Enable(true);
            });

            waitForReel = false;
        }
    }
}
