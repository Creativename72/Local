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
    public DialogueScene currentScene;
    private int currentLine = 2;

    private string[] dialogueText;


    void Start()
    {
        runDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentScene.chooseOption(1);
            //OnMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentScene.chooseOption(2);
            //OnMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentScene.chooseOption(3);
            //OnMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentScene.chooseOption(4);
            //OnMouseDown();
        }
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

        //instantiates characters in scene n stuff
        foreach (string character in charactersList)
        {
            instantiateCharacter(character);
        }

        currentScene = new DialogueScene(t.ToString());
        currentScene.parent = this;

        OnMouseDown();

    }

    //given a line of dialogue, sets speaker name, sets textbox, and highlights the proper character
    private void sayLine(string line)
    {
        string speaker = line.Split(":")[0];
        string text = this.extractText(line);
        currentSpeaker.setText(speaker);
        currentText.setText(text);
        highlightCharacter(speaker);
    }

    private string extractText(string line)
    {
        return line.Substring(line.IndexOf(":") + 1).Trim();
    }

    private void sayOptions(string options)
    {
        string speaker = "";
        string text = options;
        currentSpeaker.setText(speaker);
        currentText.setText(text);
        highlightCharacter(speaker);
    }

    //given a new character not in the scene, instantiates gameobject with their picture and sets location properly
    private void instantiateCharacter(string characterName)
    {
        d.instantiateCharacter(characterName);
    }

    private void highlightCharacter(string characterName)
    {
        //highlight character
    }

    public void simulateMouseClick()
    {
        OnMouseDown();
    }

    private void OnMouseDown()
    {
        string nextLine = currentScene.nextLine();
        string type = nextLine.Substring(0, 1);
        nextLine = nextLine.Substring(1);
        if (type.ToLower() == "e")
        {
            endDialogue();
        }
        else if (type == "l")
        {
            sayLine(nextLine);
        }
        else if (type == "o")
        {
            sayOptions(nextLine);
        }
        else if (type == "g")
        {
            currentScene.gotoSegment(nextLine.Split(":")[1].Trim());
        }
        else
        {
            throw new UnityException("Bad format: " + type + nextLine);
        }
    }

    public void endDialogue()
    {
        //dummy function for now
        self.SetActive(false);
    }

}
