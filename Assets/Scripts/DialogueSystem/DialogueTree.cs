using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DialogueTree.OptionalLine;

/// <summary>
/// Class used to parse and output dialogue lines based on dialogue format text files
/// </summary>
public class DialogueTree
{
    private const string FORMAT_ERROR = "Dialogue not formatted correctly";
    private const string ACCESS_ERROR = "Accessing the tree caused an exception";
    private const string STATE_ERROR = "Dialogue tree accessing an invalid state";


    // command delimiters
    private const char CHAR_C = ':';
    private const char VARI_C = '$';
    private const char COMM_C = '/';
    private const char NODE_C = '-';
    private const char GOTO_C = '|';
    private const char FUNC_C = '>';
    private const char OPTI_C = '[';
    private const char TOOL_C = '(';
    private const char EXIT_C = '^';
    private const char STAR_C = '+';
    private const char LINE_C = '\n';

    private TextAsset textAsset;

    private Dictionary<string, string> variables;
    private Dictionary<string, Action> functions;
    private List<Action> onClose;

    // nodes in the dialogue tree
    private Dictionary<string, Node> nodes;
    private string currentNode;

    /// <summary>
    /// Constructs a new dialogue tree with the given text asset
    /// </summary>
    /// <param name="textAsset">Text used by the tree</param>
    public DialogueTree(TextAsset textAsset)
    {
        variables = new();
        functions = new();
        onClose = new();
        nodes = new();
        this.textAsset = textAsset;
    }

    /// <summary>
    /// Sets the variable in the dialogue to be changed upon dialogue parsing
    /// </summary>
    /// <param name="varName">name of the variable</param>
    /// <param name="value">text that the variable is to be set to</param>
    public void SetVariable(string varName, string value)
    {
        variables[varName] = value;
    }

    /// <summary>
    /// Sets function with specified name to be invoked when line in dialogue is read
    /// </summary>
    /// <param name="funcName">Name of the function</param>
    /// <param name="action">Action invoked on function</param>
    public void SetFunction(string funcName, Action action)
    {
        functions[funcName] = action;
    }

    /// <summary>
    /// Selects the specified option (if it exists) and advances to the next dialogue line based on the choice,
    /// invoking any necessary logic written in the script along the way
    /// </summary>
    /// <param name="index">index of selected option</param>
    public void AdvanceLine(int index)
    {
        Node node = nodes[currentNode];
        // if it is optional, execute the function and goto statment
        Line curr = node.GetLine();
        if (curr is OptionalLine oLine)
        {
            List<OptionalLine.Option> options = oLine.GetOptions();
            if (index < 0 || index >= options.Count)
                throw new DialogueAccessException();

            Goto(options[index].node);

            string function = options[index].function;
            if (function != null)
                Func(function);
        }
        else
        {
            node.AdvanceLineIndex();
        }
        Line next = node.GetLine();


        // invoke all functional lines that may be present
        while (next is FunctionalLine fline)
        {
            fline.Invoke();
            node.AdvanceLineIndex();
            next = node.GetLine();
        }
        // line is either null, dialogue line or optional line
        // if the line is null, the dialogue tree has finished execution

        if (next == null)
        {
            End();
        }
    }

    /// <summary>
    /// Returns the line at the current Node line index
    /// </summary>
    /// <returns></returns>
    public Line GetLine()
    {
        Node curr = nodes[currentNode];
        return curr.GetLine();
    }

    public void AddCloseListener(Action action)
    {
        onClose.Add(action);
    }

    /// <summary>
    /// Reads dialogue input in the Text Asset and builds the dialogue tree
    /// </summary>
    public void Parse()
    {
        string text = textAsset.text;

        // replace all instances of $ with respective variables
        while(text.Contains(VARI_C))
        {
            StringBuilder sb = new();

            int varIndex = text.IndexOf(VARI_C);
            string afterVar = text[varIndex..];
            int varLength = afterVar.IndexOf(' ');
            string var = RemoveCommand(VARI_C, afterVar[..varLength]);

            if (!variables.ContainsKey(var))
                throw new DialogueStateException(var);

            sb.Append(text[..varIndex]).Append(variables[var]).Append(text[(varIndex + varLength + 1)..]);
            text = sb.ToString();
        }
        // put all the lines in the text asset into nodes;
        string[] lines = text.Split(LINE_C);
        nodes = new();
        string parseCurrentNode = null;
        string firstNode = null;
        bool setStart = false;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            line = line.Trim();

            // remove comments
            if (line.Contains(COMM_C))
                line = line[..line.IndexOf(COMM_C)];

            // if the line is empty continue
            if (string.IsNullOrEmpty(line))
                continue;

            char command = line[0];
            line = RemoveCommand(command, line).Trim();
            switch (command)
            {
                // node
                case NODE_C:
                    {
                        // set the current node
                        parseCurrentNode = line;
                        Node node = new(parseCurrentNode);
                        nodes.Add(parseCurrentNode, node);

                        firstNode ??= parseCurrentNode;
                    }
                    break;
                // goto
                case GOTO_C:
                    {
                        if (parseCurrentNode == null)
                            throw new DialogueParseException(i);

                        nodes[parseCurrentNode].AddLine(new GotoLine(line, this));
                    }
                    break;
                // function
                case FUNC_C:
                    {
                        if (parseCurrentNode == null)
                            throw new DialogueParseException(i);

                        nodes[parseCurrentNode].AddLine(new FunctionLine(line, this));
                    }
                    break;
                // options
                case OPTI_C:
                    {
                        // parse string into option tooltip goto and func
                        if (!line.Contains(GOTO_C))
                            throw new DialogueParseException(i);

                        int gotoIndex = line.IndexOf(GOTO_C);
                        string optionText = line[..gotoIndex].Trim();
                        string toolText = null;
                        string gotoText = RemoveCommand(GOTO_C, line[gotoIndex..]).Trim();
                        string funcText = null;

                        // get function and tooltip if present
                        if (optionText.Contains(TOOL_C))
                        {
                            int toolIndex = optionText.IndexOf(TOOL_C);
                            toolText = RemoveCommand(TOOL_C, optionText[toolIndex..]).Trim();
                            optionText = optionText[..toolIndex].Trim();
                        }
                        if (gotoText.Contains(FUNC_C))
                        {
                            int funcIndex = gotoText.IndexOf(FUNC_C);
                            funcText = RemoveCommand(FUNC_C, gotoText[funcIndex..]).Trim();
                            gotoText = gotoText[..funcIndex].Trim();
                        }

                        // add option to existing / new options
                        if (nodes[parseCurrentNode].LastLine() is not OptionalLine)
                        {
                            nodes[parseCurrentNode].AddLine(new OptionalLine());
                        }

                        ((OptionalLine)nodes[parseCurrentNode].LastLine()).AddOption(optionText, toolText, gotoText, funcText);

                    }
                    break;
                // exit
                case EXIT_C:
                    {
                        if (parseCurrentNode == null)
                            throw new DialogueParseException(i);

                        nodes[parseCurrentNode].AddLine(new FunctionLine(line, this));
                    }
                    break;
                // start node
                case STAR_C:
                    {
                        // only one start node should be specified
                        if (setStart)
                            throw new DialogueParseException(i);

                        line = RemoveCommand(command, line);
                        line = line.Trim();
                        currentNode = line;
                        setStart = true;
                    }
                    break;
                // dialogue line
                default:
                    {
                        if (parseCurrentNode == null)
                            throw new DialogueParseException(i);

                        if (!line.Contains(CHAR_C))
                            throw new DialogueParseException(i);

                        int charIndex = line.IndexOf(CHAR_C);
                        string charName = line[..charIndex];
                        string charText = RemoveCommand(CHAR_C, line[charIndex..]);
                        
                        nodes[parseCurrentNode].AddLine(new DialogueLine(charName, charText));
                    }
                    break;
            }
        }

        // set the current node if none was specified to the first node
        currentNode ??= firstNode;

        // advance through lines in current node until index is on a Dialogue or Optional line
        Node currNodes = nodes[currentNode];

        Line funcLine = currNodes.GetLine();

        // invoke all functional lines that may be present
        while (funcLine is FunctionalLine fline)
        {
            fline.Invoke();
            currNodes.AdvanceLineIndex();
            funcLine = currNodes.GetLine();
        }
    }

    /// <summary>
    /// Invokes on dialogue finish listners
    /// </summary>
    private void End()
    {
        onClose.ForEach(action => action.Invoke());
    }

    /// <summary>
    /// Goes to the target node
    /// </summary>
    /// <param name="destination">target node</param>
    private void Goto(string destination)
    {
        currentNode = destination;
        if (!nodes.ContainsKey(currentNode))
            throw new DialogueStateException(destination);

        nodes[currentNode].Init();
    }

    private void Func(string func)
    {
        if (!functions.ContainsKey(func))
            throw new DialogueStateException(func);

        functions[func].Invoke();
    }

    /// <summary>
    /// Removes the lines command character for all # of repetitions
    /// </summary>
    /// <param name="command">Command character</param>
    /// <param name="line">Line</param>
    /// <returns>the parsed string</returns>
    private string RemoveCommand(char command, string line)
    {
        while (true)
        {
            if (line[0] == command && line.Length > 0)
                line = line.Remove(0, 1);
            else
                return line;
        }
    }

    private class Node
    {
        private int index;
        private string name;
        private List<Line> lines;
        public Node(string name)
        {
            this.name = name;
            lines = new();
        }
        public void AddLine(Line l)
        {
            lines.Add(l);
        }
        public Line LastLine()
        {
            return lines[^1];
        }
        public Line GetLine()
        {
            if (lines.Count > index)
                return lines[index];

            return null;
        }
        public void Init()
        {
            index = 0;
        }
        public void AdvanceLineIndex()
        {
            index++;
        }
    }
    public interface Line
    {

    }
    public class DialogueLine : Line
    {
        private string name;
        private string text;
        public DialogueLine(string name, string text)
        {
            this.name = name;
            this.text = text;
        }
        public string GetDialogue()
        {
            return text;
        }
        public string GetCharacter()
        {
            return name;
        }

        public override string ToString()
        {
            return $"DialogueLine {{name:{name}}}, {{text:{text}}}";
        }
    }
    public class OptionalLine : Line
    {
        private List<Option> options;
        public OptionalLine()
        {
            options = new();
        }
        public List<Option> GetOptions()
        {
            return options;
        }

        public void AddOption(string option, string tooltip, string node, string function)
        {
            options.Add(new(option, tooltip, node, function));
        }

        public class Option
        {
            public Option(string option, string tooltip, string node, string function)
            {
                this.option = option;
                this.tooltip = tooltip;
                this.node = node;
                this.function = function;
            }
            public string option;
            public string tooltip;
            public string node;
            public string function;

            public override string ToString()
            {
                return $"Option {{option:{option}}}, {{tooltip:{tooltip}}}, {{node:{node}}}, {{function:{function}}}";
            }
        }
    }

    public interface FunctionalLine : Line
    {
        public abstract void Invoke();
    }

    public class GotoLine : FunctionalLine
    {
        private string destination;
        private DialogueTree tree;
        public GotoLine(string destination, DialogueTree tree)
        {
            this.destination = destination;
            this.tree = tree;
        }
        public void Invoke()
        {
            tree.Goto(destination);
        }
    }

    public class FunctionLine : FunctionalLine
    {
        private string function;
        private DialogueTree tree;
        public FunctionLine(string function, DialogueTree tree)
        {
            this.function = function;
            this.tree = tree;
        }
        public void Invoke()
        {
            tree.functions[function].Invoke();
        }
    }

    public class ExitLine : FunctionalLine
    {
        public void Invoke()
        {
            // does nothing
        }
    }

    public class DialogueParseException : Exception
    {
        public DialogueParseException(int lineIndex) : base($"{FORMAT_ERROR} at line {lineIndex}")
        {

        }
    }

    public class DialogueAccessException : Exception
    {
        public DialogueAccessException() : base($"{ACCESS_ERROR}")
        {

        }
    }

    public class DialogueStateException : Exception
    {
        public DialogueStateException(string state) : base($"{STATE_ERROR} state: ${state}")
        {

        }
    }
}
