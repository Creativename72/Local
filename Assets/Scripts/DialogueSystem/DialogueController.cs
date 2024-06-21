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
    public CameraChanger cmac;
    public BaseSceneManager s;
    public bool end;

    protected string[] dialogueText;
    protected Dictionary<string, Action> functions = new();
    protected bool dialogueEnabled;
    

    protected float pauseEnd;

    void Start()
    {
        pauseEnd = -1;
        //runDialogue();
    }

    private void Update()
    {
        UpdateDialogueOpen();
    }

    protected void UpdateDialogueOpen()
    {
        if (dialogueRunning)
        {
            if (pauseEnd > Time.time)
            {
                dialogueEnabled = false;
            }
            else if (pauseEnd > 0)
            {
                dialogueEnabled = true;
                pauseEnd = -1;
                simulateMouseClick();
            }
            container.SetActive(dialogueEnabled);
        }
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
        setCharacters(dialogueText[0]);
        currentScene = new DialogueScene(t.ToString());
        currentScene.parent = this;
        simulateMouseClick();
    }

    //given a line of dialogue, sets speaker name, sets textbox, and highlights the proper character
    protected void sayLine(string line)
    {
        string speaker = line.Split(":")[0];
        string text = this.extractText(line);
        text = this.italicize(text);
        currentSpeaker.setText(speaker);
        speakerContainer.SetActive(speaker.Trim() != "");
        currentText.setText(text);
        highlightCharacter(speaker);
    }

    protected string italicize(string line)
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

    protected string extractText(string line)
    {
        return line.Substring(line.IndexOf(":") + 1).Trim();
    }

    protected void sayOptions()
    {
        currentSpeaker.setText("");
        speakerContainer.SetActive(false);
        currentText.setText("");
    }

    protected void setCharacters(string rawText)
    {
        string charactersInSceneRaw = rawText.Split(":")[1];
        string[] charactersList = charactersInSceneRaw.Split(",");
        //removes whitespace
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = charactersList[i].Trim();
        }
        d.clear();
        //instantiates characters in scene n stuff
        foreach (string character in charactersList)
        {
            instantiateCharacter(character);
        }
    }

    //given a new character not in the scene, instantiates gameobject with their picture and sets location properly
    protected void instantiateCharacter(string characterName)
    {
        d.instantiateCharacter(characterName);
    }

    protected void highlightCharacter(string characterName)
    {
        //highlight character
    }

    public void simulateMouseClick()
    {
        OnMouseDown();
    }

    private void OnMouseDown()
    {
        UpdateDialogueOpen();

        Debug.Log("MD = " + dialogueEnabled);
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
        // NOTE, do not use pause() in text files and PauseDialogue() concurrently unless you want unintended behavior.
        else if (type == "p")
        {
            Debug.Log("PAUSING");
            bool b = float.TryParse(nextLine, out float f);
            if (!b)
                throw new UnityException("Bad format: pause() does not contain valid number!");
            pauseEnd = f + Time.time;
            simulateMouseClick();
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
        else if (type == "sc")
        {
            setCharacters(nextLine);
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

        g[index].GetComponent<SetText>().setText((index + 1).ToString() + ") "+ this.italicize(text));
    }

    public void endDialogue()
    {
        dialogueRunning = false;
        container.SetActive(false);
        b.enabled = false;
        Debug.Log(end);
        if (s != null && s.endOnDialogueEnd)
        {
            s.endScene();
        } else if (end)
        {
            MapController.Instance.LoadNextScene("Map");
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
        // Debug.Log("PAUSING DIALOGUE!");
        dialogueEnabled = false;
        pauseEnd = float.MaxValue;
    }
    public void ResumeDialogue()
    {
        // Debug.Log("RESUMING DIALOGUE!");
        dialogueEnabled = true;
        pauseEnd = -1;
        simulateMouseClick();
    }
}
