using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSegment
{
    private ArrayList lines;
    public int currentLine = 0;
    public DialogueController p;
    private DialogueLine current = null;
    private bool canBind = false;

    public DialogueSegment(string textraw, DialogueController parent)
    {
        this.p = parent;
        string[] text = textraw.Split("\n");
        lines = new ArrayList();
        

        for (int i = 0; i < text.Length; i++)
        {
            string line = text[i];
            if (line.ToLower().Contains("inscene:"))
            {
                if (!canBind)
                {
                    current = new DialogueLine("");
                    lines.Add(current);
                }
                current.spriteChanger = true;
                current.spriteChanges = line;
            }
            else if (line.Contains(":"))
            {
                string speaker = line.Split(":")[0].Trim().ToLower();
                if (speaker == "goto")
                {
                    current = new DialogueLine(line);
                    current.segmentChanger = true;
                    lines.Add(current);
                    current = null;
                    canBind = false;
                }
                else
                {
                    current = new DialogueLine(line);
                    current.canRead = true;
                    lines.Add(current);
                    canBind = true;
                }
            }
            else if (line.Contains("["))
            {
                if (!canBind)
                {
                    current = new DialogueLine("");
                    lines.Add(current);
                    canBind = true;
                }
                string id = line.Substring(line.IndexOf("[") + 1, line.IndexOf("]") - line.IndexOf("[") - 1);
                current.addOption(id, text[i + 1]);
                i++; //makes sure the dialogue option isn't treated as an id if italicized
            }
            else if (line.ToLower().Contains("changebackground("))
            {
                if (!canBind)
                {
                    current = new DialogueLine("");
                    lines.Add(current);
                }
                current.sceneChanger = true;
                if (line.ToLower().Contains("nofade")) {
                    current.fade = false;
                }
            }
            else if (line.ToLower().Contains("changebackgroundunbound("))
            {
                current = new DialogueLine("");
                lines.Add(current);
                current.sceneChanger = true;
                if (line.ToLower().Contains("nofade"))
                {
                    current.fade = false;
                }
            }
            else if (line.ToLower().Contains("pause("))
            {
                lines.Add(new DialogueLine(line)
                {
                    pause = true,
                });
                canBind = false;
                current = null;
            }
            else if (text[i].Contains(">"))
            {
                DialogueLine functionLine = new(text[i])
                {
                    function = true
                };
                lines.Add(functionLine);
                canBind = false;
                current = null;
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

    public bool canRead = false;
    public bool hasOptions = false;
    public bool sceneChanger = false;
    public bool spriteChanger = false;
    public string spriteChanges = "";

    public bool function = false;
    public bool pause = false;
    public bool segmentChanger = false;
    public bool fade = true;
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
