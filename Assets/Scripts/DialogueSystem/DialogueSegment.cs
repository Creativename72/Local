using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSegment
{
    private ArrayList lines;
    public int currentLine = 0;

    public DialogueSegment(string textraw)
    {

        string[] text = textraw.Split("\n");
        lines = new ArrayList();

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].Contains(":"))
            {
                DialogueLine newLine = new DialogueLine(text[i]);
                lines.Add(newLine);
            } else if (text[i].Contains("["))
            {
                if (lines.Count == 0)
                {
                    lines.Add(new DialogueLine(""));
                }
                DialogueLine current = (DialogueLine)lines[lines.Count - 1];
                string id = text[i].Substring(text[i].IndexOf("[") + 1, text[i].IndexOf("]") - 1);
                //Debug.Log(id);
                current.addOption(id, text[i + 1]);
            }
        }
    }

    public DialogueLine nextLine()
    {
        if (currentLine == lines.Count)
        {
            currentLine = 0;
            return null;
        }
        DialogueLine d = (DialogueLine) lines[currentLine];
        if (!d.hasOptions)
        {
            currentLine++;
        }
        return d;
    }
}


public class DialogueLine
{
    public string text;
    public ArrayList options;
    public bool hasOptions = false;

    public DialogueLine(string textraw)
    {
        text = textraw;
        options = new ArrayList();
    }

    public void addOption(string id, string textraw)
    {
        hasOptions = true;
        string[] option = { textraw, id };
        options.Add(option);
    }

    public void readOptions(DialogueController p)
    {
        p.enableOptions(options.Count);
        for (int i = 0; i < options.Count; i++)
        {
            p.setOption(i, ((string[]) options[i])[0]);
        }

    }

    public string chooseOption(int choice)
    {
        return ((string[])options[choice - 1])[1];
    }
}
