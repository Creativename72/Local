using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnieDay1 : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    [SerializeField] List<TextAsset> dialogueText;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter annieCharacter;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;
    [SerializeField] PlayDialogueSFX playDialogueSFX;

    void Start()
    {
        DialogueCharacter scout = dialogueController.CreateCharacter(scoutCreator);
        DialogueCharacter narrator = dialogueController.CreateCharacter(narratorCreator);
        DialogueCharacter annie = dialogueController.CreateCharacter(annieCharacter);

        annie.SetName("Mole");
        annie.SetSprite(0);
        scout.SetVisible(true);
        annie.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        GameStateManager.Instance.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameStateManager.Instance.GetFlag<bool>(name));
        });
        dialogueController.AddFunction("ShowAnnie", () => annie.SetVisible(true));
        dialogueController.AddFunction("HideAnnie", () => annie.SetVisible(false));
        dialogueController.AddFunction("setAnnieName", () => annie.SetName("Annie"));
        dialogueController.AddFunction("AnnieRangDoorbell", () => GameStateManager.Instance.SetFlag("AnnieRangDoorbell", true));
        dialogueController.AddFunction("AnnieJoked", () => GameStateManager.Instance.SetFlag("AnnieJoked", true));
        dialogueController.AddFunction("MetAnnie", () => GameStateManager.Instance.SetFlag("MetAnnie", true));

        dialogueController.AddDefaultFunctions(backgroundHandler);

        dialogueController.AddEndFunction(() => {
            GameManager.Instance.ChangeScene("Map");
            GameManager.Instance.SaveGame();
        });
        dialogueText.ForEach(dialogueText => dialogueController.AddDialogue(dialogueText));
        dialogueController.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
