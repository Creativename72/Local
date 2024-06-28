using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class BerniceDialogueController : DialogueController, IPointerClickHandler
{

    public GraphicRaycaster raycaster;
    public BerniceSceneManager bsm;
    public BerniceDialogueCharacterHandler bd;

    new public void simulateMouseClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        OnPointerClick(pointerData);
    }

    public void OnPointerClick(PointerEventData pointerEventData) // 3
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
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
    }

    new public void endDialogue()
    {
        dialogueRunning = false;
        container.SetActive(false);
        b.enabled = false;
        Debug.Log(end);
        if (bsm != null && bsm.endOnDialogueEnd)
        {
            bsm.endScene();
        } else if (end)
        {
            MapController.Instance.LoadNextScene("Map");
        }
    }

    new protected void setCharacters(string rawText)
    {
        string charactersInSceneRaw = rawText.Split(":")[1];
        string[] charactersList = charactersInSceneRaw.Split(",");
        //removes whitespace
        for (int i = 0; i < charactersList.Length; i++)
        {
            charactersList[i] = charactersList[i].Trim();
        }
        bd.clear();
        //instantiates characters in scene n stuff
        foreach (string character in charactersList)
        {
            instantiateCharacter(character);
        }
    }

    //given a new character not in the scene, instantiates gameobject with their picture and sets location properly
    new protected void instantiateCharacter(string characterName)
    {
        bd.instantiateCharacter(characterName);
    }

    new public void runDialogue()
    {
        container.SetActive(true);
        dialogueRunning = true;
        b.enabled = true;
        dialogueText = t.ToString().Split("\n");
        dialogueEnabled = true;
        //instantiates characters in scene
        setCharacters(dialogueText[0]);
        //currentScene = new DialogueScene(t.ToString());
        currentScene.parent = this;
        simulateMouseClick();
    }
}
