using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScene
{
    private Dictionary<string, DialogueSegment> segmentsDictionary;
    private string currentSegment;
    private DialogueLine currentLine;
    public bool optionsFlag = false;
    private bool canChoose = false;
    public DialogueController parent;

    public DialogueScene(string textraw, DialogueController parent)
    {
        this.parent = parent;
        segmentsDictionary = new Dictionary<string, DialogueSegment>();
        string[] text = textraw.Split("id:");
        currentSegment = text[1].Split("\n")[0].Trim();

        for (int i = 1; i < text.Length; i++)
        {
            string id = text[i].Split("\n")[0].Trim();
            segmentsDictionary.Add(id, new DialogueSegment(text[i], parent));
        }
        //debug_CheckDictionary();
    }

    public DialogueLine nextLine()
    {
        
        if (optionsFlag)
        {
            canChoose = true;
            parent.sayOptions();
            currentLine.readOptions(parent);
            return null;
        }
        DialogueLine nLine = segmentsDictionary[currentSegment.Trim()].nextLine();
        if (nLine == null)
        {
            parent.endDialogue();
            return null;
        }
        if (nLine.hasOptions)
        {
            optionsFlag = true;
            currentLine = nLine;
            if (nLine.text == "")
            {
                canChoose = true;
                parent.sayOptions();
                currentLine.readOptions(parent);
                return null;
            }
        }
        return nLine;
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
