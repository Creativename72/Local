using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WalterDay2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter walterCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;

    private bool waitForCast = false;
    private bool waitForReel = false;
    DialogueCharacter scout;
    DialogueCharacter narrator;
    DialogueCharacter walter;
    private void Start()
    {
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        walter = dialogueController.CreateCharacter(walterCreator);

        walter.SetName("Walter");
        walter.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);

        bool askedPlayKeyBoard = false;
        bool askedSmallKitchen = false;
        bool askedLikeFishing = false;

        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.SetVariable("AskedAllQuestions", () => askedSmallKitchen && askedPlayKeyBoard && askedLikeFishing);
        dialogueController.SetVariable("NotAskedPlayKeyBoard", () => !askedPlayKeyBoard);
        dialogueController.SetVariable("NotAskedSmallKitchen", () => !askedSmallKitchen);
        dialogueController.SetVariable("NotAskedLikeFishing", () => !askedLikeFishing);
        GameStateManager.Instance.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameStateManager.Instance.GetFlag<bool>(name));
        });

        dialogueController.AddFunction("PlayKeyboard", () => askedPlayKeyBoard = true);
        dialogueController.AddFunction("SmallKitchen", () => askedSmallKitchen = true);
        dialogueController.AddFunction("LikeFishing", () => askedLikeFishing = true);

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


        dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
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
            GameManager.WaitRoutine(1f, () =>
            {
                backgroundHandler.changeBackground(false);
                GameManager.WaitRoutine(1f, () =>
                {
                    dialogueController.Enable(true);
                });
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
                walter.SetVisible(true);
                scout.SetVisible(true);
            });

            waitForReel = false;
        }
    }
}
