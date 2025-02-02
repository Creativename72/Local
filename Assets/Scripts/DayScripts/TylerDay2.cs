using System.Collections.Generic;
using UnityEngine;

public class TylerDay2 : MonoBehaviour
{
    [Header("Needed Initialization")]
    [SerializeField] private DC dialogueController;
    [SerializeField] DialogueCharacter scoutCreator;
    [SerializeField] DialogueCharacter tylerCreator;
    [SerializeField] DialogueCharacter narratorCreator;
    [SerializeField] AmbienceInScene ambienceInScene;
    [SerializeField] BackgroundHandler backgroundHandler;

    [Header("TextAssets")]
    [SerializeField] TextAsset part1;
    [SerializeField] TextAsset part2;

    [Header("Clickables")]
    [SerializeField] private GameObject pointsOfInterest;
    [SerializeField] private List<ClickableObject> clickables;

    private int itemsInspected;
    private State currentState;
    private DialogueCharacter scout;
    private DialogueCharacter narrator;
    private DialogueCharacter tyler;
    //private bool continueClickedRun = false;

    // public AudioClip sceneMusic; //music for this scene

    public void Continue()
    {
        backgroundHandler.FadeInOut();
        dialogueController.ClearDialogue();
        dialogueController.AddDialogue(part2);
        dialogueController.AddEndFunction(() => { GameManager.Instance.ChangeScene("Map"); GameManager.Instance.SaveGame(); });
        currentState = State.Transition;
        
        GameManager.WaitRoutine(0.75f, () =>
        {
            tyler.SetVisible(true);
            scout.SetVisible(true);
            tyler.SetLocation(DialogueCharacter.Location.LEFT);

        });
        GameManager.WaitRoutine(1.5f, () =>
        {
            dialogueController.StartDialogue();
        });

    }
    private enum State
    {
        IntroDialogue,
        Minigame,
        Transition,
    }

    // Start is called before the first frame update
    void Start()
    {
        pointsOfInterest.SetActive(false);
        itemsInspected = 0;
        currentState = State.IntroDialogue;

        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        tyler = dialogueController.CreateCharacter(tylerCreator);

        tyler.SetSprite(1);
        tyler.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);
        scout.SetVisible(true);

        clickables.ForEach(clickable => clickable.OnClick(() =>
        {
            itemsInspected++;
            clickable.SetVisible(false);
        }));

        dialogueController.AddGameFlags();
        dialogueController.AddDefaultFunctions(backgroundHandler, ambienceInScene);
        dialogueController.AddDialogue(part1);
        dialogueController.ReparseOnNodeChange(true);
        dialogueController.StartDialogue();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.IntroDialogue && dialogueController.IsDone())
        {
            currentState = State.Transition;
            backgroundHandler.changeBackground(true);
            GameManager.WaitRoutine(0.75f, () =>
            {
                currentState = State.Minigame;
            });
        }
        else if (currentState == State.Minigame)
        {
            pointsOfInterest.SetActive(true);
            scout.SetVisible(false);
            tyler.SetLocation(DialogueCharacter.Location.RIGHT);
            tyler.SetVisible(!dialogueController.IsDone());

            if (itemsInspected >= 5 && dialogueController.IsDone())
            {
                Continue();
            }

        }
        else if (currentState == State.Transition)
        {
            // do nothing
        }
    }
}
