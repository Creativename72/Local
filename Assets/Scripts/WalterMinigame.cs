using System;
using System.Collections;
using UnityEngine;

public class WalterMinigame : MonoBehaviour
{
    [SerializeField] private DialogueController controller;
    private bool waiting;
    private bool biting;

    void Start()
    {
        waiting = false;
        controller.AddFunction("wait_click", () =>
        {
            controller.PauseDialogue();
            StartCoroutine(Wait(0.5f, () => waiting = true));
        });
    }

    private void Update()
    {
        if (waiting && Input.GetButtonDown("Fire1"))
        {
            waiting = false;
            controller.ResumeDialogue();
        }
    }

    private IEnumerator Wait(float time, Action a)
    {
        yield return new WaitForSeconds(time);
        a.Invoke();
    }
}
