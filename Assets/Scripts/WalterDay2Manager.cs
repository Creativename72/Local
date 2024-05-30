using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;

public class WalterDay2Manager : MonoBehaviour
{
    [SerializeField] private TextAsset walterDay2TextIntro;
    [SerializeField] private TextAsset walterDay2TextFishing;
    [SerializeField] private TextAsset caughtText;

    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private SpriteRenderer fishingSprite;

    private enum State
    {
        IntroDialogue,
        FishingStart,
        FishingDialogue,
        WaitingForBobber,
        CatchFish,
        FishingStop,
    }

    private State state;

    private void Start()
    {
        state = State.IntroDialogue;
        fishingGame.SetActive(false);
        dialogueController.t = walterDay2TextIntro;
        dialogueController.runDialogue();
    }
    private void Update()
    {
        if (state == State.IntroDialogue && !dialogueController.dialogueRunning)
        {
            WaitForCast();
        }
        else if (state == State.FishingDialogue && !dialogueController.dialogueRunning)
        {
            state = State.WaitingForBobber;
            StartCoroutine(Wait(3, () =>
            {
                SubmergeBobber();
            }));
        }
        else if (state == State.FishingStop && !dialogueController.dialogueRunning)
        {
            Application.Quit();
            // END GAME CHANGE SCENE TO MAP
        }
    }

    private void WaitForCast()
    {
        state = State.FishingStart;
        fishingGame.SetActive(true);
    }

    private void StartFishingDialogue()
    {
        state = State.FishingDialogue;
        fishingSprite.color = Color.red;
        dialogueController.t = walterDay2TextFishing;
        dialogueController.runDialogue();
    }
    private void SubmergeBobber()
    {
        state = State.CatchFish;
        fishingSprite.transform.position = new Vector3(0, -2);
        this.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -2);
    }

    private void StopFishing()
    {
        state = State.FishingStop;
        fishingSprite.transform.position = new Vector3(0, 2);
        dialogueController.t = caughtText;
        dialogueController.runDialogue();
    }

    // waits for seconds and then invokes action
    private IEnumerator Wait(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    private void OnMouseDown()
    {
        if (state == State.FishingStart)
        {
            // continue dialogue
            StartFishingDialogue();
        }
        else if (state == State.CatchFish)
        {
            StopFishing();
        }
    }
}