using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TylerDay3 : MonoBehaviour
{
    [Header("General Setup")]
    [SerializeField] private DC dialogueController;
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private DialogueCharacter scoutCreator;
    [SerializeField] private DialogueCharacter tylerCreator;
    [SerializeField] private DialogueCharacter narratorCreator;
    [SerializeField] private BackgroundHandler backgroundHandler;
    [SerializeField] private AmbienceInScene ambienceInScene;
    [SerializeField] private PlayDialogueSFX playDialogueSFX;

    [Header("Harvest Game Stuff")]
    [SerializeField] private GameObject harvestGameObject;

    private DialogueCharacter scout;
    private DialogueCharacter narrator;
    private DialogueCharacter tyler;

    private int harvestCount;
    private int winCount;
    private bool inMinigame;
    // Start is called before the first frame update
    void Start()
    {
        harvestGameObject.SetActive(false);

        tyler = dialogueController.CreateCharacter(tylerCreator);
        narrator = dialogueController.CreateCharacter(narratorCreator);
        scout = dialogueController.CreateCharacter(scoutCreator);

        tyler.SetVisible(false);
        tyler.SetSprite(1);
        scout.SetVisible(true);
        tyler.SetLocation(DialogueCharacter.Location.LEFT);
        scout.SetLocation(DialogueCharacter.Location.RIGHT);

        dialogueController.AddGameFlags();
        dialogueController.AddDefaultFunctions(backgroundHandler);
        dialogueController.AddFunction("StartMinigame", () => StartMinigame());
        dialogueController.AddFunction("FocusOnLeftPlot", () => Debug.Log("Focusing On Left Plot"));
        dialogueController.AddFunction("FocusOnRightPlot", () => Debug.Log("Focusing On Right Plot"));
        dialogueController.AddFunction("Scale04", () => GameManager.WaitRoutine(0.75f, () => backgroundHandler.transform.localScale = new(0.4f, 0.4f)));
        dialogueController.AddFunction("Scale06", () => GameManager.WaitRoutine(0.75f, () => backgroundHandler.transform.localScale = new(0.6f, 0.6f)));
        dialogueController.AddFunction("ShowTyler", () => tyler.SetVisible(true));
        dialogueController.AddFunction("EnterMinigameBackground", () =>
        {
  
            backgroundHandler.changeBackground();
            GameManager.WaitRoutine(0.75f, () => harvestGameObject.SetActive(true));

        });
        dialogueController.AddFunction("ExitMinigameBackground", () =>
        {
            backgroundHandler.changeBackground();
            GameManager.WaitRoutine(0.75f, () => harvestGameObject.SetActive(false));
        });

        dialogueController.AddDialogue(dialogueText);
        dialogueController.AddEndFunction(() => GameManager.Instance.ChangeScene("Map"));
        dialogueController.ReparseOnNodeChange(true);
        dialogueController.StartDialogue();

        SetupHarvestGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (winCount == harvestCount && inMinigame)
        {
            StopMinigame();
        }
    }

    private void StopMinigame()
    {
        GameManager.WaitRoutine(1f, () =>
        {
            int childCount = harvestGameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject go = harvestGameObject.transform.GetChild(i).gameObject;
                HighlightableObject highlight = go.GetComponent<HighlightableObject>();
                highlight.EnableClick(false);
                highlight.EnableHighlight(false);
            }
            dialogueController.Enable(true);
        });
        inMinigame = false;
    }
    private void StartMinigame()
    {
        dialogueController.Enable(false);
        // enable all the vegetables to be clicked
        int childCount = harvestGameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            HighlightableObject highlight = harvestGameObject.transform.GetChild(i).gameObject.GetComponent<HighlightableObject>();
            // disable all clicks and highlights to start
            highlight.EnableClick(true);
            highlight.EnableHighlight(true);
        }
        inMinigame = true;
    }

    private void SetupHarvestGame()
    {
        harvestCount = 0;
        winCount = 0;
        inMinigame = false;
        int childCount = harvestGameObject.transform.childCount;
        // go through each child and if their name is one of the selectable ones add a function to increase score on click
        for (int i = 0; i < childCount; i++)
        {
            GameObject go = harvestGameObject.transform.GetChild(i).gameObject;
            HighlightableObject highlight = go.GetComponent<HighlightableObject>();
            // disable all clicks and highlights to start
            highlight.EnableClick(false);
            highlight.EnableHighlight(false);
            if (go.name.Contains("Potato") || go.name.Contains("Carrots") || go.name.Contains("Onions"))
            {
                // count needeed to win
                winCount++;
                highlight.OnClick(() =>
                {
                    // increase count and disable object click don't know yet how we want to handle the harvesting of vegetables visually
                    harvestCount++;
                    highlight.EnableClick(false);
                    highlight.EnableHighlight(false);
                    highlight.sprite.enabled = false;
                    playDialogueSFX.PlaySFX("s_harvest");
                });
            }
            else
            {
                highlight.OnClick(() =>
                {
                    // disable highlighting if can't harvest it
                    highlight.EnableHighlight(false);
                });
            }
        }
    }
}
