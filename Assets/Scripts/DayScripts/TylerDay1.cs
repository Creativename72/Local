using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TylerDay1 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
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
        tyler.SetSprite(0);
        scout.SetVisible(true);
        GameFlags.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameFlags.GetFlag<bool>(name));
        });
        
        dialogueController.AddFunction("TylerPranked", () => GameFlags.SetFlag("TylerPranked", true));
        dialogueController.AddFunction("ShowTyler", () => tyler.SetVisible(true));
        dialogueController.AddFunction("HideTyler", () => tyler.SetVisible(false));
        dialogueController.AddFunction("SetTylerName", () => tyler.SetName("Tyler"));
        dialogueController.AddFunction("TylerMessy", () => GameFlags.SetFlag("TylerMessy", true));
        dialogueController.AddFunction("MetTyler", () => GameFlags.SetFlag("MetTyler", true));

        dialogueController.AddDefaultFunctions(backgroundHandler);

        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
