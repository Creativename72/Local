using System;
using System.Collections;
using UnityEngine;

public class WalterDay2Manager : MonoBehaviour
{
    [SerializeField] private TextAsset text;

    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject towerRain;
    [SerializeField] private GameObject blank;
    [SerializeField] private GameObject towerClear;
    [SerializeField] private GameObject interior;
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject bobber;
    [SerializeField] private GameObject fish;

    //private int itemsTalkedAbout;
    //^commented out bc unused so a warning doesn't pop up

    private enum State
    {
        IntroDialogue,
        FishingStart,
        FishingDialogue,
        WaitingForBobber,
        CatchFish,
        PostFishing,
    }

    private State state;

    private void Start()
    {
        //itemsTalkedAbout = 0;
        trail.SetActive(true);
        towerRain.SetActive(false);
        blank.SetActive(false);
        towerClear.SetActive(false);
        interior.SetActive(false);
        bobber.SetActive(false);
        fish.SetActive(false);

        state = State.IntroDialogue;
        fishingGame.SetActive(false);
        dialogueController.t = text;

        dialogueController.AddFunction("walk_tower", () =>
        {
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(3, () => dialogueController.ResumeDialogue()));
            trail.SetActive(false);
            towerRain.SetActive(true);
        });
        dialogueController.AddFunction("climb_a_bit", () =>
        {
            dialogueController.PauseDialogue();
            towerRain.SetActive(false);
            blank.SetActive(true);
            StartCoroutine(Wait(3, () =>
            {
                towerClear.SetActive(true);
                blank.SetActive(false);
                dialogueController.ResumeDialogue();
            }));
        });
        dialogueController.AddFunction("interior", () =>
        {
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(3, () => dialogueController.ResumeDialogue()));
            towerClear.SetActive(false);
            interior.SetActive(true);
        });
        dialogueController.AddFunction("charcoal", () =>
        {
            Debug.Log("Charcoal Selected!");
        });
        dialogueController.AddFunction("keyboard", () =>
        {
            Debug.Log("Keyboard Selected!");
        });
        dialogueController.AddFunction("kitchen", () =>
        {
            Debug.Log("Kitchen Selected!");
        });
        dialogueController.AddFunction("rods", () =>
        {
            Debug.Log("Rods Selected!");
        });
        dialogueController.AddFunction("no_fishing", () =>
        {
            Application.Quit();
            // END GAME CHANGE SCENE TO MAP
        });
        dialogueController.AddFunction("going_fishing", () =>
        {
            interior.SetActive(false);
            fishingGame.SetActive(true);
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(3, () => dialogueController.ResumeDialogue()));
            state = State.FishingStart;
        });
        dialogueController.AddFunction("wait_cast", () =>
        {
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(0.5f, () => state = State.WaitingForBobber));
        });
        dialogueController.AddFunction("pause_3", () =>
        {
            dialogueController.PauseDialogue();
            StartCoroutine(Wait(3, () => dialogueController.ResumeDialogue()));
            Debug.Log("Pausing for 3");
        });
        dialogueController.AddFunction("bite", () =>
        {
            state = State.CatchFish;
            bobber.transform.position = new Vector3(0, -5);
            dialogueController.PauseDialogue();
        });
        bobber.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (state == State.CatchFish)
            {
                bobber.SetActive(false);
                fish.SetActive(true);
                state = State.PostFishing;
                StartCoroutine(Wait(2, () => dialogueController.ResumeDialogue()));
            }
        });
        dialogueController.runDialogue();
    }

    private IEnumerator Wait(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
    private void Update()
    {
        // bobber casted!
        if (state == State.WaitingForBobber && Input.GetButtonDown("Fire1"))
        {
            bobber.SetActive(true);
            // and now we wait
            StartCoroutine(Wait(1f, () => dialogueController.ResumeDialogue()));
            state = State.FishingStart;
        }
        if (state == State.PostFishing && !dialogueController.dialogueRunning)
        {
            Application.Quit();
            // END GAME CHANGE SCENE TO MAP
        }
    }
}