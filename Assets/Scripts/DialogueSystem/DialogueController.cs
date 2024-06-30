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
    public GameObject subcontainer;
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
    public AudioSource a;

    protected string[] dialogueText;
    protected Dictionary<string, Action> functions = new();
    protected bool dialogueEnabled;

    [SerializeField] bool scrollingText = true; //inspector option for turning scrolling text on/off
    //current dialogue coroutine (should only be one running)
    private IEnumerator currDialogue;
    private String currLine;
    //determines text scroll speed, default is 1 letter every 1 frames 
    private float textScrollDelayQuantity;
    private float textScrollDelay;
    public bool textCurrentlyScrolling = false;
    public bool changeBackgroundOnResume = false;


    protected float pauseEnd;

    void Start()
    {
        textScrollDelayQuantity = 0.015f;
        textScrollDelay = textScrollDelayQuantity;
        pauseEnd = -1;
        //runDialogue();
    }

    private void Update()
    {
        if (textCurrentlyScrolling)
        {
            textScrollDelay -= Time.deltaTime;
        } else
        {
            textScrollDelay = textScrollDelayQuantity;
        }
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
                if (!dialogueEnabled)
                {
                    Debug.Log("UNPAUSING");
                }
                dialogueEnabled = true;
                pauseEnd = -1;
                
                simulateMouseClick();
            }
            subcontainer.SetActive(dialogueEnabled);
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
        currentScene = new DialogueScene(t.ToString(), this);
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

        if (scrollingText)
        {
            if (currDialogue != null) { StopCoroutine(currDialogue); } //stop current dialogue coroutine
            this.textCurrentlyScrolling = true;
            currDialogue = TypeSentence(line); //set curr dialogue to a new coroutine with new sentence
            StartCoroutine(currDialogue); //start a new dialogue coroutine with a new sentence
        }
        else
        {
            currentText.setText(this.italicize(text)); //this line sets the new text
            a.Stop();
        }

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

    public void sayOptions()
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
        /*
         * Checks if there is currently a line of dialogue being scrolled through
         * If so, stop scrolling and complete the full dialogue
         * This check happens before any other functions that trigger off mouseClick to avoid messing up dialogue later.
         */
        if (currDialogue != null)
        {
            StopCoroutine(currDialogue);
            currentText.setText(this.italicize(currLine));
            a.Stop();
            currDialogue = null;
            return;
        }

        UpdateDialogueOpen();

        //Debug.Log("MD = " + dialogueEnabled);
        if (!dialogueEnabled)
            return;

        if (this.textCurrentlyScrolling)
        {
            this.textCurrentlyScrolling = false;
            this.textScrollDelay = this.textScrollDelayQuantity;
        }
        //TODO:options
        DialogueLine nextLine = currentScene.nextLine();
        if (nextLine == null)
        {
            return;
        }
        if (nextLine.segmentChanger)
        {
            string destSegment = nextLine.text.Split(":")[1].Trim();
            if (destSegment.ToLower() == "end")
            {
                endDialogue();
                return;
            }
            currentScene.gotoSegment(nextLine.text.Split(":")[1].Trim());
            return;
        }

        if (nextLine.sceneChanger)
        {
            bgs.changeBackground(nextLine.fade);
            //Debug.Log("Changing background");
        }
        if (nextLine.spriteChanger)
        {
            this.setCharacters(nextLine.spriteChanges);
        }
        if (nextLine.pause)
        {
            //Debug.Log("PAUSING");
            string t = nextLine.text[6..];
            t = t[0..(t.Length - 2)];
            bool b = float.TryParse(t, out float f);
            if (!b)
                throw new UnityException("Bad format: pause() does not contain valid number!");
            pauseEnd = f + Time.time;
            simulateMouseClick();
        }
        if (nextLine.function)
        {
            string key = nextLine.text.Substring(1, nextLine.text.Length - 1).Trim();

            if (functions.ContainsKey(key))
            {
                functions[key].Invoke();
                simulateMouseClick();
            }
            else
            {
                throw new UnityException("Function \"" + key + "\" does not exist!");
            }
        }
        if (nextLine.canRead)
        {
            sayLine(nextLine.text);
        } else
        {
            OnMouseDown();
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
        this.functions[key] = action;
    }

    private IEnumerator Wait(float time, Action after)
    {
        yield return new WaitForSeconds(time);
        after.Invoke();
    }

    //Coroutine for scrolling dialogue (adds one letter at a time)
    private IEnumerator TypeSentence(string sentence)
    {
        //Debug.Log(sentence);
        currLine = sentence.Split(":")[1];
        string[] toIterate = sentenceHelper(currLine).ToArray();
        string text = "";
        int index = 0;
        char[] tempArray = currLine.ToCharArray();
        a.Play();
        while (index < toIterate.Length)
        {
            string letter = toIterate[index];
            if (!this.textCurrentlyScrolling)
            {
                currentText.setText(this.italicize(currLine));
                a.Stop();
                break;
            } else if (textScrollDelay < 0)
            {
                text += letter;
                index += 1;
                currentText.setText(text);               
                textScrollDelay += textScrollDelayQuantity;
                yield return textScrollDelay;
            } else
            {
                yield return textScrollDelay;
            }
        }
        a.Stop();
        currDialogue = null;
        this.textCurrentlyScrolling = false;
        textScrollDelay = textScrollDelayQuantity;
    }

    private List<String> sentenceHelper(string sentence)
    {
        // should return string list with each character as its own element with the exception of lines that include the markdown tokesn <i> or <\i> which should be appended to the previous character.
        // if you replace the .ToCharArray() call in the typesentence that should make this work
        List<String> ret = new List<string>();
        
        for (int i = 0; i < sentence.Length; i++)
        {
            string currentChar = sentence.Substring(i, 1);
            if (currentChar == "[")
            {
                if (ret.Count == 0)
                {
                    ret.Add("<i>");
                }
                else
                {
                    ret[ret.Count - 1] = ret[ret.Count - 1] + "<i>";
                }
            }
            else if (currentChar == "]")
            {
                if (ret.Count == 0)
                {
                    ret.Add("</i>");
                }
                else
                {
                    ret[ret.Count - 1] = ret[ret.Count - 1] + "</i>";
                }
            }
            else
            {
                ret.Add(currentChar);
            }
        }

        return ret;
    }

    public void PauseDialogue()
    {
        // Debug.Log("PAUSING DIALOGUE!");
        currentText.setText("");
        dialogueEnabled = false;
        
        pauseEnd = float.MaxValue;
    }
    public void ResumeDialogue()
    {
        // Debug.Log("RESUMING DIALOGUE!");
        dialogueEnabled = true;
        if (changeBackgroundOnResume)
        {
            bgs.changeBackground();
            changeBackgroundOnResume = false;
        }
        pauseEnd = -1;
        simulateMouseClick();
    }
}
