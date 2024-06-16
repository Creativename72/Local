using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject speakerContainer;
    public BackgroundHandler bgs;
    public BaseSceneManager s;

    private string[] dialogueText;
    private Dictionary<string, Action> functions = new();
    private bool dialogueEnabled;


    void Start()
    {
        //runDialogue();
    }

    private void Update()
    {
        
    }

    //given a text file of a dialogue scene, runs dialogue
    public void runDialogue()
    {
        container.SetActive(true);
        dialogueRunning = true;
        b.enabled = true;
        dialogueText = t.ToString().Split("\n");
        dialogueEnabled = true;
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
        speakerContainer.SetActive(speaker.Trim() != "");
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
            }
            else if (currentChar == "]")
            {
                ret += "</i>";
            }
            else
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
        speakerContainer.SetActive(false);
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
        if (!dialogueEnabled)
            return;

        string[] nextLineList = currentScene.nextLine();
        string nextLine = nextLineList[1];
        string type = nextLineList[0];

        //Debug.Log(nextLine);
        //Debug.Log(type);

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
            sayOptions();
        }
        else if (type == "g")
        {
            currentScene.gotoSegment(nextLine.Split(":")[1].Trim());
        }
        else if (type == "f")
        {
            string key = nextLine;
          
            if (functions.ContainsKey(key))
            {
                functions[nextLine].Invoke();
                simulateMouseClick();
            }
            else
            {
                throw new UnityException("Function \"" + key + "\" does not exist!");
            }
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

        g[index].GetComponent<SetText>().setText(this.italicize(text));
    }

    public void endDialogue()
    {
        dialogueRunning = false;
        container.SetActive(false);
        b.enabled = false;
        if (s != null && s.endOnDialogueEnd)
        {
            s.endScene();
        }
    }

    public void AddFunction(string key, Action action)
    {
        functions[key] = action;
    }

    private IEnumerator Wait(float time, Action after)
    {
        yield return new WaitForSeconds(time);
        after.Invoke();
    }
    public void PauseDialogue()
    {
        dialogueEnabled = false;
        this.gameObject.SetActive(false);
    }

    int c = 0;
    public void ResumeDialogue()
    {
        Debug.Log("RESUMING DIALOGUE" + c++);
        dialogueEnabled = true;
        this.gameObject.SetActive(true);
        simulateMouseClick();
    }

    public void RemoveOption(int index)
    {
        
    }

    public void GotoId(string id)
    {

    }
}
