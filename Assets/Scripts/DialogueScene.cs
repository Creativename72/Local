using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScene
{
    private Dictionary<string, DialogueSegment> segmentsDictionary;
    private string currentSegment;
    private DialogueLine currentLine;
    private bool optionsFlag = false;

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
            return "o" + currentLine.readOptions();
        }
        DialogueLine nLine = segmentsDictionary[currentSegment].nextLine();
        if (nLine == null)
        {
            return "end";
        }
        else if (nLine.hasOptions)
        {
            optionsFlag = true;
            currentLine = nLine;
        }
        return "l" + nLine.text;
    }

    public void chooseOption(int option)
    {
        segmentsDictionary[currentSegment].currentLine = 0;
        currentSegment = currentLine.chooseOption(option);
        optionsFlag = false;
    }
}
