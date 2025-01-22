using System.Collections.Generic;
using UnityEngine;
public class BerniceIntro : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
    [SerializeField] DialogueCharacter berniceCreator;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;

    void Start()
    {
        DialogueCharacter bernice = dialogueController.CreateCharacter(berniceCreator);
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);

        bernice.SetLocation(DialogueCharacter.Location.RIGHT);
        
        scout.SetLocation(DialogueCharacter.Location.LEFT);
        //
        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.AddFunction("showScout", () => scout.SetVisible(true));
        dialogueController.AddFunction("showBernice", () => bernice.SetVisible(true));
        dialogueController.AddFunction("setBerniceName", () => bernice.SetName("Bernice"));
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
