using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutDay2 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private DialogueCharacter scoutCreator;
    [SerializeField] private DialogueCharacter narratorCreator;
    [SerializeField] private DialogueCharacter journalCreator;
    [SerializeField] private DialogueCharacter berniceCreator;
    [SerializeField] private AmbienceInScene ambienceInScene;
    [SerializeField] private BackgroundHandler backgroundHandler;
    [SerializeField] private PlayDialogueSFX audioPlayer;
    [SerializeField] private AudioClip writeSFX;

    private DialogueCharacter scout;
    private DialogueCharacter narrator;
    private DialogueCharacter bernice;
    private DialogueCharacter journal;

    // Start is called before the first frame update
    void Start()
    {
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        journal = dialogueController.CreateCharacter(journalCreator);
        bernice = dialogueController.CreateCharacter(berniceCreator);

        dialogueController.AddGameFlags();
        dialogueController.AddFunction("WriteEntry", () =>
        {
            dialogueController.Pause(3);
            audioPlayer.PlaySFX(writeSFX);
        });

        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);
        dialogueController.AddDialogue(dialogueText);
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
