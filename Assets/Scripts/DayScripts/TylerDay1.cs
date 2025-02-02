using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TylerDay1 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] TextAsset dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter tylerCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;

    // Start is called before the first frame update
    void Start()
    {
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);
        DialogueCharacter tyler = dialogueController.CreateCharacter(tylerCreator);

        tyler.SetName("???");
        tyler.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        tyler.SetSprite(0);
        scout.SetVisible(true);
        dialogueController.AddGameFlags();

        dialogueController.AddFunction("TylerPranked", () => GameStateManager.Instance.SetFlag("TylerPranked", true));
        dialogueController.AddFunction("ShowTyler", () => tyler.SetVisible(true));
        dialogueController.AddFunction("HideTyler", () => tyler.SetVisible(false));
        dialogueController.AddFunction("SetTylerName", () => tyler.SetName("Tyler"));
        dialogueController.AddFunction("TylerMessy", () => GameStateManager.Instance.SetFlag("TylerMessy", true));
        dialogueController.AddFunction("MetTyler", () => GameStateManager.Instance.SetFlag("MetTyler", true));

        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);

        dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        dialogueController.AddDialogue(dialogueText);
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
