using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockDay : MonoBehaviour
{
    [SerializeField] private DialogueCharacter walter;
    [SerializeField] private DialogueCharacter narrator;
    [SerializeField] private TextAsset text;
    [SerializeField] private DC dc;

    // Start is called before the first frame update
    void Start()
    {
        dc.SetFunction("Func", () => Debug.Log("Func!"));
        dc.SetVariable("var", new("wonderful"));
        dc.SetVariable("ScoutNice", new DialogueTree.LineVariable(() => true));

        dc.AddDialogue(text);
        dc.CreateCharacter(walter);
        dc.CreateCharacter(narrator);
        dc.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
