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

    }

    public string nextLine()
    {
        
        if (optionsFlag)
        {
            canChoose = true;
            return "o" + currentLine.readOptions();
        }
        DialogueLine nLine = segmentsDictionary[currentSegment.Trim()].nextLine();
        if (nLine == null)
        {
            return "end";
        }
        else if (nLine.hasOptions)
        {
            optionsFlag = true;
            currentLine = nLine;
        } else if (nLine.text.Split(":")[1].Trim().ToLower() == "end")
        {
            return "e";
        } else if (nLine.text.Split(":")[0].Trim().ToLower() == "goto")
        {
            return "g" + nLine.text;
        }
        return "l" + nLine.text;
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
}
