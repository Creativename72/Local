using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static DialogueTree;

public class WalterDay1 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter walterCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;
    void Start()
    {
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);
        DialogueCharacter walter = dialogueController.CreateCharacter(walterCreator);

        walter.SetName("Bear");
        dialogueController.AddFunction("changeWalterName", () => walter.SetName("Walter"));
        dialogueController.AddFunction("ShowWalter", () =>
        {
            walter.SetVisible(true);
            walter.SetLocation(DialogueCharacter.Location.CENTER);
        });
        dialogueController.AddFunction("HideWalter", () => walter.SetVisible(false));
        dialogueController.AddFunction("StartSpeaking", () =>
        {
            walter.SetVisible(true);
            scout.SetVisible(true);
            scout.SetLocation(DialogueCharacter.Location.RIGHT);
            walter.SetLocation(DialogueCharacter.Location.LEFT);
        });

        GameStateManager.Instance.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameStateManager.Instance.GetFlag<bool>(name));
        });

        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
