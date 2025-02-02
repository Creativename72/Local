using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FinalHangout : MonoBehaviour
{
    // all the different text assets for the different endings
    [Header("Dialogue Scripts")]
    [SerializeField] private TextAsset allBadText;
    [SerializeField] private TextAsset walterGoodText;
    [SerializeField] private TextAsset walterAnnieGoodText;
    [SerializeField] private TextAsset walterTylerGoodText;
    [SerializeField] private TextAsset annieGoodText;
    [SerializeField] private TextAsset annieTylerGoodText;
    [SerializeField] private TextAsset tylerGoodText;
    [SerializeField] private TextAsset allGoodText;

    [Header("Other setup")]
    [SerializeField] private DC dialogueController;
    [SerializeField] private DialogueCharacter scoutCreator;
    [SerializeField] private DialogueCharacter annieCreator;
    [SerializeField] private DialogueCharacter walterCreator;
    [SerializeField] private DialogueCharacter tylerCreator;
    [SerializeField] private DialogueCharacter narratorCreator;
    [SerializeField] private DialogueCharacter journalCreator;
    [SerializeField] private AmbienceInScene ambienceInScene;
    [SerializeField] private BackgroundHandler backgroundHandler;
    [SerializeField] private PlayDialogueSFX audioPlayer;
    [SerializeField] private AudioClip writeSFX;


    private DialogueCharacter walter;
    private DialogueCharacter annie;
    private DialogueCharacter tyler;
    private DialogueCharacter scout;
    private DialogueCharacter narrator;

    // Start is called before the first frame update
    void Start()
    {
        walter = dialogueController.CreateCharacter(walterCreator);
        annie = dialogueController.CreateCharacter(annieCreator);
        tyler = dialogueController.CreateCharacter(tylerCreator);
        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);

        annie.SetSprite(1);
        tyler.SetSprite(1);
        dialogueController.AddGameFlags();
        dialogueController.AddFunction("WriteEntry", () =>
        {
            dialogueController.Pause(3);
            audioPlayer.PlaySFX(writeSFX);
        });
        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene, audioPlayer);
        
        bool walterGood = GameFlags.GetFlag<bool>("WalterGoodEnding");
        bool annieGood = GameFlags.GetFlag<bool>("AnnieGoodEnding");
        bool tylerGood = GameFlags.GetFlag<bool>("TylerGoodEnding");

        if (!annieGood && !walterGood && !tylerGood)
        {
            // all bad ending
            dialogueController.AddDialogue(allBadText);
        }
        else if (walterGood && !annieGood && !tylerGood)
        {
            // walter good
            dialogueController.AddDialogue(walterGoodText);

        }
        else if (walterGood && annieGood && !tylerGood)
        {
            // walter annie good
            dialogueController.AddDialogue(walterAnnieGoodText);

        }
        else if (walterGood && !annieGood && tylerGood)
        {
            // walter tyler good
            dialogueController.AddDialogue(walterTylerGoodText);

        }
        else if (!walterGood && annieGood && !tylerGood)
        {
            // annie good
            dialogueController.AddDialogue(annieGoodText);

        }
        else if (!walterGood && annieGood && tylerGood)
        {
            // annie tyler good
            dialogueController.AddDialogue(annieTylerGoodText);

        }
        else if (!walterGood && !annieGood && tylerGood)
        {
            // tyler good
            dialogueController.AddDialogue(tylerGoodText);

        }
        else if (walterGood && annieGood && tylerGood)
        {
            // all good
            dialogueController.AddDialogue(allGoodText);

        }

        dialogueController.StartDialogue();
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("EndScene"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
