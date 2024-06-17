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
            string line = text[i];
            if (text[i].ToLower().Contains("inscene:"))
            {
                lines.Add(new DialogueLine(line)
                {
                    spriteChanger = true
                });
            }
            else if (text[i].Contains(":"))
            {
                DialogueLine newLine = new DialogueLine(text[i]);
                lines.Add(newLine);
            }
            else if (text[i].Contains("["))
            {
                if (lines.Count == 0)
                {
                    lines.Add(new DialogueLine(""));
                }
                DialogueLine current = (DialogueLine)lines[lines.Count - 1];
                string id = text[i].Substring(text[i].IndexOf("[") + 1, text[i].IndexOf("]") - text[i].IndexOf("[") - 1);
                //Debug.Log(id);
                current.addOption(id, text[i + 1]);
                i++; //makes sure the dialogue option isn't treated as an id if italicized
            }
            else if (text[i].ToLower().Contains("changebackground()"))
            {
                ((DialogueLine)lines[lines.Count - 1]).sceneChanger = true;
            }
            else if (text[i].ToLower().Contains("pause("))
            {
                lines.Add(new DialogueLine(line)
                {
                    pause = true
                });
            }
            else if (text[i].Contains(">"))
            {
                DialogueLine functionLine = new(text[i])
                {
                    function = true
                };
                lines.Add(functionLine);
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
        DialogueLine d = (DialogueLine)lines[currentLine];
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
    public bool sceneChanger = false;
    public bool function = false;
    public bool pause = false;
    public bool spriteChanger = false;
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
            p.setOption(i, ((string[])options[i])[0]);
        }

    }

    public string chooseOption(int choice)
    {
        return ((string[])options[choice - 1])[1];
    }
}
