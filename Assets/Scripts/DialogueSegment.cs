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
                string id = text[i].Substring(1, text[i].Length - 3);
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
    private ArrayList options;
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

    public string readOptions()
    {
        string final = "";

        for (int i = 0; i < options.Count; i++)
        {
            final = final + (i + 1) + ") " + ((string[]) options[i])[0] + "\n";
        }
        return final;
    }

    public string chooseOption(int choice)
    {
        return ((string[])options[choice - 1])[1];
    }
}
