using System.Collections.Generic;
using UnityEngine;
public class ScoutDay1 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] TextAsset dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] DialogueCharacter journalCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;
    [SerializeField] PlayDialogueSFX playDialogueSFX;
    [SerializeField] AudioClip writeSFX;

    void Start()
    {
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);
        DialogueCharacter journal = dialogueController.CreateCharacter(journalCreator);

        scout.SetVisible(false);
        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);
        dialogueController.AddFunction("WriteEntry", () =>
        {
            dialogueController.Pause(1.5f);
            playDialogueSFX.PlaySFX(writeSFX);
        });

        dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        dialogueController.AddDialogue(dialogueText);
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
