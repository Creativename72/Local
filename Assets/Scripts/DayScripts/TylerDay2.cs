using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static DialogueTree;

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
    [SerializeField] TextAsset graffiti;
    [SerializeField] TextAsset grill;
    [SerializeField] TextAsset metalSculpture;
    [SerializeField] TextAsset offTerrainBike;
    [SerializeField] TextAsset speakersGuitar;

    [Header("Clickables")]
    [SerializeField] private GameObject pointsOfInterest;
    [SerializeField] private GameObject bikeObject;
    [SerializeField] private GameObject grillObject;
    [SerializeField] private GameObject guitarObject;
    [SerializeField] private GameObject sculptureObject;
    [SerializeField] private GameObject graffitiObject;

    private int pointsInteracted;
    private State currentState;
    private DialogueCharacter scout;
    private DialogueCharacter narrator;
    private DialogueCharacter tyler;
    //private bool continueClickedRun = false;

    // public AudioClip sceneMusic; //music for this scene

    public void Continue()
    {
        dialogueController.ClearDialogue();
        dialogueController.AddDialogue(part2);
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueController.StartDialogue();
        currentState = State.PostDialogue;
        tyler.SetVisible(true);
        scout.SetVisible(true);
        tyler.SetLocation(DialogueCharacter.Location.LEFT);
    }
    private enum State
    {
        IntroDialogue,
        Minigame,
        PostDialogue,
    }

    // Start is called before the first frame update
    void Start()
    {
        pointsOfInterest.SetActive(false);
        pointsInteracted = 0;
        currentState = State.IntroDialogue;

        scout = dialogueController.CreateCharacter(scoutCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        tyler = dialogueController.CreateCharacter(tylerCreator);

        tyler.SetSprite(1);
        scout.SetVisible(true);

        GameFlags.ForEachFlag<bool>((name, val) =>
        {
            dialogueController.SetVariable(name, () => GameFlags.GetFlag<bool>(name));
        });
        dialogueController.AddFunction("ShowTyler", () => tyler.SetVisible(true));
        dialogueController.AddFunction("HideTyler", () => tyler.SetVisible(false));

        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.AddDialogue(part1);
        dialogueController.StartDialogue();
        dialogueController.ReparseOnNodeChange(true);

        bikeObject.GetComponent<HighlightableObject>().OnClick(() =>
        {
            TryStartDialogue(offTerrainBike, bikeObject);
        });
        grillObject.GetComponent<HighlightableObject>().OnClick(() =>
        {
            TryStartDialogue(grill, grillObject);
        });
        guitarObject.GetComponent<HighlightableObject>().OnClick(() =>
        {
            TryStartDialogue(speakersGuitar, guitarObject);
        });
        sculptureObject.GetComponent<HighlightableObject>().OnClick(() =>
        {
            TryStartDialogue(metalSculpture, sculptureObject);
        });
        graffitiObject.GetComponent<HighlightableObject>().OnClick(() =>
        {
            TryStartDialogue(graffiti, graffitiObject);
        });

    }

    private void TryStartDialogue(TextAsset text, GameObject obj)
    {
        Debug.Log("trying to start dialogue?");
        if (dialogueController.IsDone())
        {
            tyler.SetVisible(true);
            dialogueController.ClearDialogue();
            dialogueController.AddDialogue(text, false);
            dialogueController.StartDialogue();
            obj.SetActive(false);
            pointsInteracted++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.IntroDialogue && dialogueController.IsDone())
        {
            currentState = State.Minigame;
            pointsOfInterest.SetActive(true);
        }
        else if (currentState == State.Minigame && dialogueController.IsDone())
        {
            scout.SetVisible(false);
            tyler.SetVisible(false);
            tyler.SetLocation(DialogueCharacter.Location.RIGHT);
            if (pointsInteracted >= 5)
                Continue();
        } 
        else if (currentState == State.PostDialogue)
        {
           
        }
    }
}
