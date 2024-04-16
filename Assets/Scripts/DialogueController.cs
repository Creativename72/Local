using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    public TextAsset t;
    public DialogueCharacterHandler d;
    public SetText currentSpeaker;
    public SetText currentText;
    public GameObject self;
    private int currentLine = 2;

    private string[] dialogueText;


    void Start()
    {
        runDialogue();
    }

    //given a text file of a dialogue scene, runs dialogue
    private void runDialogue()
    {
        dialogueText = t.ToString().Split("\n");
        self.SetActive(true);

        //instantiates characters in scene
        string charactersInSceneRaw = dialogueText[0].Split(":")[1];
        string[] charactersList = charactersInSceneRaw.Split(",");

        //removes whitespace
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = charactersList[i].Trim();
        }

        foreach (string character in charactersList)
        {
            instantiateCharacter(character);
        }

        OnMouseDown();

    }

    //given a line of dialogue, sets speaker name, sets textbox, and highlights the proper character
    private void sayLine(string line)
    {
        string[] splitLine = line.Split(":");

        string characterName = splitLine[0];
        string newText = splitLine[1];
        currentSpeaker.setText(characterName);
        currentText.setText(newText.Trim());
    }

    //given a new character not in the scene, instantiates gameobject with their picture and sets location properly
    private void instantiateCharacter(string characterName)
    {
        d.instantiateCharacter(characterName);
    }

    private void OnMouseDown()
    {
        if (currentLine >= dialogueText.Length)
        {
            endDialogue();
        } else
        {
            sayLine(dialogueText[currentLine]);
            currentLine += 1;
        }
    }

    public void endDialogue()
    {
        //dummy function for now
        self.SetActive(false);
    }

}
