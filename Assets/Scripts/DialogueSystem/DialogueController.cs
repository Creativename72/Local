using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    public TextAsset t;
    public DialogueCharacterHandler d;
    public SetText currentSpeaker;
    public SetText currentText;
    public GameObject container;
    public DialogueScene currentScene;
    public bool dialogueRunning = false;
    public BoxCollider b;
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public BackgroundHandler bgs;

    private string[] dialogueText;


    void Start()
    {
        //runDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentScene.chooseOption(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentScene.chooseOption(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentScene.chooseOption(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentScene.chooseOption(4);
        }
    }

    //given a text file of a dialogue scene, runs dialogue
    public void runDialogue()
    {
        container.SetActive(true);
        dialogueRunning = true;
        b.enabled = true;
        dialogueText = t.ToString().Split("\n");
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
        text = this.italicize(text);
        currentSpeaker.setText(speaker);
        currentText.setText(text);
        highlightCharacter(speaker);
    }

    private string italicize(string line)
    {
        string ret = "";
        for (int i = 0; i < line.Length; i++)
        {
            string currentChar = line.Substring(i, 1);
            if (currentChar == "[")
            {
                ret += "<i>";
            } else if (currentChar == "]")
            {
                ret += "</i>";
            } else
            {
                ret += currentChar;
            }
        }
        return ret;
    }

    private string extractText(string line)
    {
        return line.Substring(line.IndexOf(":") + 1).Trim();
    }

    private void sayOptions()
    {
        currentSpeaker.setText("");
        currentText.setText("");
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
        string[] nextLineList = currentScene.nextLine();
        string nextLine = nextLineList[1];
        string type = nextLineList[0];

        if (type.ToLower() == "e")
        {
            endDialogue();
        }
        else if (type == "l")
        {
            sayLine(nextLine);
        } 
        else if (type == "lc")
        {
            sayLine(nextLine);
            bgs.changeBackground();
        }
        else if (type == "o")
        {
            sayOptions();
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

    public void enableOptions(int amt)
    {
        GameObject[] g = { option1, option2, option3 };

        for (int i = 0; i < amt; i++)
        {
            g[i].SetActive(true);
        }
    }

    public void disableOptions()
    {
        GameObject[] g = { option1, option2, option3 };

        for (int i = 0; i < 3; i++)
        {
            g[i].SetActive(false);
        }
    }

    public void setOption(int index, string text)
    {
        GameObject[] g = { option1, option2, option3 };

        g[index].GetComponent<SetText>().setText((index + 1).ToString() + ") " + this.italicize(text));
    }

    public void endDialogue()
    {
        dialogueRunning = false;
        container.SetActive(false);
        b.enabled = false;
    }

}