using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScene
{
    private Dictionary<string, DialogueSegment> segmentsDictionary;
    private string currentSegment;
    private DialogueLine currentLine;
    private bool optionsFlag = false;
    private bool canChoose = false;
    public DialogueController parent;

    public DialogueScene(string textraw)
    {
        segmentsDictionary = new Dictionary<string, DialogueSegment>();
        string[] text = textraw.Split("id:");
        currentSegment = text[1].Split("\n")[0].Trim();

        for (int i = 1; i < text.Length; i++)
        {
            string id = text[i].Split("\n")[0].Trim();
            segmentsDictionary.Add(id, new DialogueSegment(text[i]));
        }

        //debug_CheckDictionary();
    }

    public string[] nextLine()
    {
        
        if (optionsFlag)
        {
            canChoose = true;
            currentLine.readOptions(parent);
            return new[] { "o", "" };
        }
        DialogueLine nLine = segmentsDictionary[currentSegment.Trim()].nextLine();
        if (nLine == null)
        {
            return new[] { "e", "" };
        }

        if (nLine.sceneChanger)
        {
            parent.bgs.changeBackground();
        }
        if (nLine.camChanger)
        {
            //parent.cmac.changeCamera();
        }
        if (nLine.spriteChanger)
        {
            return new[] { "sc", nLine.text, nLine.spriteChanges };
        }
        else if (nLine.pause)
        {
            string t = nLine.text[6..];
            t = t[0..(t.Length - 2)];
            return new[] { "p", t };
        }
        else if (nLine.hasOptions)
        {
            optionsFlag = true;
            currentLine = nLine;
            if (nLine.text == "")
            {
                canChoose = true;
                currentLine.readOptions(parent);
                return new[] { "o", "" };
            }
        } else if (nLine.function)
        {
            return new[] { "f", nLine.text[1..].Trim() }; 
        }
        else if (nLine.text.Split(":")[1].Trim().ToLower() == "end")
        {
            return new[] { "e", "" };
        }
        else if (nLine.text.Split(":")[0].Trim().ToLower() == "goto")
        {
            return new[] { "g", nLine.text };
        }
        return new[] { "l", nLine.text };
    }

    public void chooseOption(int option)
    {
        if (!canChoose)
        {
            return;
        }
        segmentsDictionary[currentSegment].currentLine = 0;
        currentSegment = currentLine.chooseOption(option);
        optionsFlag = false;
        canChoose = false;
        parent.disableOptions();
        parent.simulateMouseClick();
    }

    public void gotoSegment(string id)
    {
        segmentsDictionary[currentSegment].currentLine = 0;
        currentSegment = id;
        optionsFlag = false;
        canChoose = false;
        parent.simulateMouseClick();
    }

    public string printSelf()
    {
        return currentSegment;
    }

    private void debug_CheckDictionary()
    {
        foreach (KeyValuePair<string, DialogueSegment> line in segmentsDictionary)
        {
            Debug.Log("Key: " + line.Key + "\tValue: " + line.Value);
        }
    }
}
