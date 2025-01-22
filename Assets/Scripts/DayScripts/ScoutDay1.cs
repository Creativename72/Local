using System.Collections.Generic;
using UnityEngine;
public class ScoutDay1 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;
    [SerializeField] PlayDialogueSFX playDialogueSFX;
    [SerializeField] AudioClip writeSFX;

    void Start()
    {
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);

        scout.SetVisible(false);
        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.AddFunction("WriteEntry", () =>
        {
            dialogueController.Pause(3);
            playDialogueSFX.PlaySFX(writeSFX);
        });
        dialogueController.AddFunction("GoInside", () =>
        {
            ambienceInScene.ChangeAmbience();
        });
        dialogueController.AddFunction("WalterCares", () =>
        {
            GameFlags.SetFlag("WalterCares", true);
        });
        dialogueController.AddFunction("WalterSolitude", () =>
        {
            GameFlags.SetFlag("WalterSolitude", true);
        });
        dialogueController.AddFunction("WalterConflicted", () =>
        {
             GameFlags.SetFlag("WalterConflicted", true);
        });
        dialogueController.AddFunction("AnnieBreaks", () =>
        {
            GameFlags.SetFlag("AnnieBreaks", true);
        });
        dialogueController.AddFunction("AnnieConnections", () =>
        {
            GameFlags.SetFlag("AnnieConnections", true);
        });
        dialogueController.AddFunction("AnnieSelf", () =>
        {
            GameFlags.SetFlag("AnnieSelf", true);
        });
        dialogueController.AddFunction("TylerCool", () =>
        {
            GameFlags.SetFlag("TylerCool", true);
        });
        dialogueController.AddFunction("TylerTryhard", () =>
        {
            GameFlags.SetFlag("TylerTryhard", true);
        });
        dialogueController.AddFunction("TylerFindSelf", () =>
        {
            GameFlags.SetFlag("TylerFindSelf", true);
        });
        dialogueController.AddFunction("FadeToScoutInterior", () =>
        {
            
        });
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
