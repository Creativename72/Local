using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockDay : MonoBehaviour
{
    [SerializeField] private DialogueCharacter walter;
    [SerializeField] private DialogueCharacter narrator;
    [SerializeField] private DialogueCharacter scout;

    [SerializeField] private TextAsset text;
    [SerializeField] private DC dc;

    // Start is called before the first frame update
    void Start()
    {
        dc.SetFunction("Func", () => Debug.Log("Func!"));
        // pauses the game for 3 seconds
        dc.SetFunction("Pause", () =>
        {
            dc.Enable(false);
            StartCoroutine(Wait(3, () => dc.Enable(true)));
        });

        dc.SetVariable("var", new("wonderful"));
        dc.SetVariable("ScoutNice", new DialogueTree.LineVariable(() => true));

        dc.AddDialogue(text);
        dc.CreateCharacter(walter);
        dc.CreateCharacter(narrator);
        dc.CreateCharacter(scout);
        dc.StartDialogue();
    }

    private IEnumerator Wait(float seconds, Action a)
    {
        yield return new WaitForSeconds(seconds);
        a.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
