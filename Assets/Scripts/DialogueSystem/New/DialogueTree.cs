using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static DialogueTree;
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
    private const char ESCA_C = '\\';
    private const char FORM_C = '*';
    private const char LINE_C = '\n';
    private const char SPACE = ' ';
    private readonly char[] commands = { CHAR_C, VARI_C, COMM_C, NODE_C, GOTO_C, FUNC_C, OPTI_C, TOOL_C, EXIT_C, STAR_C, ESCA_C, FORM_C, LINE_C };
    private readonly char[] optionsCommands = { GOTO_C, FUNC_C, TOOL_C };

    private Dictionary<string, Variable> variables;
    private Dictionary<string, Action> functions;
    private List<Action> onClose;

    // nodes in the dialogue tree
    private Dictionary<string, Node> nodes;
    private string currentNode;

    /// <summary>
    /// Constructs a new dialogue tree with the given text asset
    /// </summary>
    /// <param name="textAsset">Text used by the tree</param>
    public DialogueTree()
    {
        variables = new();
        functions = new();
        onClose = new();
        nodes = new();
    }

    /// <summary>
    /// Upon parse evaluation, sets the variable in the dialogue to the given value
    /// </summary>
    /// <param name="varName">name of the variable</param>
    /// <param name="value">text that the variable is to be set to</param>
    public void SetVariable(string varName, Variable var)
    {
        variables[varName] = var;
    }
    public bool IsEmpty()
    {
        return nodes.Count == 0;
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

    public void Parse(TextAsset asset)
    {
        ParseText(asset, null, 0);
    }

    /// <summary>
    /// Parses the dialogue tree returning to its existing spot
    /// </summary>
    /// <param name="returnToExisting">if true, return to the existing spot when parsing</param>
    public void Parse(TextAsset asset, bool returnToExisting)
    {
        if (returnToExisting)
        {
            int index = 0;
            if (currentNode != null && nodes.ContainsKey(currentNode))
                index = nodes[currentNode].GetLineIndex();

            ParseText(asset, currentNode, index);
        }
        else
        {
            ParseText(asset, null, 0);
        }
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
            List<Option> options = oLine.GetOptions();
            if (index < 0 || index >= options.Count)
                throw new DialogueAccessException();

            // invoke options as needed such as function or goto
            Option selected = options[index];
            if (selected.function != null)
                Func(selected.function);
            if (selected.node != null)
                Goto(selected.node);
            else
                node.AdvanceLineIndex();
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
    /// <returns>The line if it exists, else returns null</returns>
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
    private void ParseText(TextAsset textAsset, string startNode, int startLineIndex)
    {
        string text = textAsset.text;

        // replace all instances of $ with respective variables
        while (text.Contains(VARI_C))
        {
            StringBuilder sb = new();

            int varIndex = text.IndexOf(VARI_C);
            string afterAll = text[varIndex..];
            string afterSymbol = RemoveCommand(VARI_C, afterAll);
            int symbolLength = afterAll.Length - afterSymbol.Length;
            int varLength = afterSymbol.IndexOf(VARI_C);
            string beforeVar = text[..varIndex];
            string var = afterSymbol[..varLength];
            // does not check that after symbols are actually $
            // just removes same length of $ variables as on beginning.
            string afterVar = afterSymbol[(varLength + symbolLength)..];

            if (!variables.ContainsKey(var))
                throw new DialogueStateException(var);

            string varValue = variables[var].GetValue();

            sb.Append(beforeVar).Append(varValue).Append(afterVar);
            text = sb.ToString();
        }

        // remove comments
        string[] commLines = text.Split('\n');
        StringBuilder csb = new();
        for (int i = 0; i < commLines.Length; i++)
        {
            string line = commLines[i];

            // remove comments
            if (line.Contains(COMM_C))
                line = line[..line.IndexOf(COMM_C)];

            csb.Append(line).Append('\n');
        }
        text = csb.ToString();

        // bold and italicize the text
        int escapeCount = 0;
        bool bold = false;
        bool italic = false;
        StringBuilder fsb = new();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            char prevChar = i == 0 ? '\0' : text[i - 1];
            char nextChar = i == text.Length - 1 ? '\0' : text[i + 1];

            // character that is not escape once started escaping
            if (c != FORM_C && escapeCount > 0)
            {
                bool newItalic = italic;
                bool newBold = bold;
                // shouldn't have more than 3 escapes
                if (escapeCount > 3)
                {
                    throw new DialogueParseException("More than 3 escapes in a row");
                }
                switch (escapeCount)
                {
                    // invert italic *
                    case 1:
                        {
                            newItalic = !italic;
                        }
                        break;
                    // invert bold **
                    case 2:
                        {
                            newBold = !bold;
                        }
                        break;
                    // invert both ***
                    case 3:
                        {
                            newItalic = !italic;
                            newBold = !bold;
                        }
                        break;
                }

                // if there were changes to any, insert the appropriate characters
                if (bold != newBold)
                {
                    bold = newBold;
                    fsb.Append(bold ? "<b>" : "</b>");
                }
                if (italic != newItalic)
                {
                    italic = newItalic;
                    fsb.Append(italic ? "<i>" : "</i>");
                }
                fsb.Append(c);
                escapeCount = 0;
            }
            // character that is regular character with no escapes or is escape
            else
            {
                // * character
                if (c == FORM_C)
                {
                    if (prevChar != ESCA_C)
                    {
                        escapeCount++;
                    }
                    else
                    {
                        fsb.Append(c);
                    }
                }
                // \ character
                else if (c == ESCA_C)
                {
                    if (nextChar != FORM_C)
                    {
                        fsb.Append(c);
                    }
                }
                // regular character
                else
                {
                    fsb.Append(c);
                }
            }
        }
        text = fsb.ToString();

        // put all the formatted lines in the text asset into nodes;
        string[] lines = text.Split(LINE_C);
        string parseCurrentNode = null;
        string firstNode = null;
        bool setStart = false;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            line = line.Trim();

            // if the line is empty continue
            if (string.IsNullOrEmpty(line))
                continue;

            char command = line[0];
            if (commands.Contains(command))
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
                        int length = FirstIndexOf(optionsCommands, line);
                        if (length < 0)
                            length = line.Length;
                        string optionText = line[..length].Trim();
                        string toolText = null;
                        string gotoText = null;
                        string funcText = null;

                        // get goto function and tooltip if present
                        if (line.Contains(GOTO_C))
                        {
                            int startIndex = line.IndexOf(GOTO_C);
                            string textAfter = RemoveCommand(GOTO_C, line[startIndex..]);
                            int endIndex = FirstIndexOf(optionsCommands, textAfter);
                            if (endIndex < 0)
                                endIndex = textAfter.Length;
                            gotoText = textAfter[..endIndex].Trim();
                        }
                        if (line.Contains(TOOL_C))
                        {
                            int startIndex = line.IndexOf(TOOL_C);
                            string textAfter = RemoveCommand(TOOL_C, line[startIndex..]);
                            int endIndex = FirstIndexOf(optionsCommands, textAfter);
                            if (endIndex < 0)
                                endIndex = textAfter.Length;
                            toolText = textAfter[..endIndex].Trim();
                        }
                        if (line.Contains(FUNC_C))
                        {
                            int startIndex = line.IndexOf(FUNC_C);
                            string textAfter = RemoveCommand(FUNC_C, line[startIndex..]);
                            int endIndex = FirstIndexOf(optionsCommands, textAfter);
                            if (endIndex < 0)
                                endIndex = textAfter.Length;
                            funcText = textAfter[..endIndex].Trim();
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
                        string charName = line[..charIndex].Trim();
                        string charText = RemoveCommand(CHAR_C, line[charIndex..]).Trim();

                        nodes[parseCurrentNode].AddLine(new DialogueLine(charName, charText));
                    }
                    break;
            }
        }

        // priority of start node is #1 startNode, #2 ++ node, #3 first node
        if (startNode != null)
        {
            currentNode = startNode;
        }
        else if (currentNode == null)
        {
            // ++ node defined
        }
        else
        {
            currentNode = firstNode;
        }

        // advance through lines in current node until index is on a Dialogue or Optional line
        Node currNodes = nodes[currentNode];
        currNodes.SetLineIndex(startLineIndex);

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

    /// <summary>
    /// Returns the first index of any of the specified chars in the given array
    /// </summary>
    /// <param name="chars">characters to check</param>
    /// <param name="text">text to parse through</param>
    /// <returns>index of this character in text or -1 if there was no instance</returns>
    private int FirstIndexOf(char[] chars, string text)
    {
        if (chars.Length <= 0)
            return -1;

        int min = int.MaxValue;
        for (int i = 0; i < chars.Length; i++)
        {
            int index = text.IndexOf(chars[i]);
            if (index >= 0)
                min = Math.Min(min, index);
        }
        // no hits
        if (min == int.MaxValue)
            return -1;
        
        return min;
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
        public int GetLineIndex()
        {
            return index;
        }
        public void SetLineIndex(int index)
        {
            this.index = index;
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
            return $"DialogueLine {{{{name:{name}}}, {{text:{text}}}}}";
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

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("OptionalLine {");
            for (int i = 0; i < options.Count; i++)
            {
                Option o = options[i];
                sb.Append($"{{options[{i}] {{").Append(o.ToString()).Append("}}");
                if (i < options.Count - 1)
                    sb.Append(", ");
            }
            sb.Append("}");
            return sb.ToString();
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
                return $"Option {{{{option:{option}}}, {{tooltip:{tooltip}}}, {{node:{node}}}, {{function:{function}}}}}";
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

    /// <summary>
    /// Variable is used to define variables to be replaced in the dialogue parser
    /// </summary>
    public class Variable
    {
        private string value;
        public virtual string GetValue()
        {
            return value;
        }

        public Variable(string value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Line variables are used to set sections of the text to be active if a condition is met
    /// If the condition is not met, the text will be hidden.
    /// </summary>
    public class LineVariable : Variable
    {
        private Func<bool> condition;
        public override string GetValue()
        {
            bool val = condition.Invoke();
            if (val)
                return string.Empty;
            else
                return COMM_C.ToString();
        }
        public LineVariable(Func<bool> condition) : base(null)
        {
            this.condition = condition;
        }
    }

    public class DialogueParseException : Exception
    {
        public DialogueParseException(int lineIndex) : base($"{FORMAT_ERROR} at line {lineIndex}")
        {

        }
        public DialogueParseException(string desc) : base($"{FORMAT_ERROR}. {desc}")
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
        public DialogueStateException(string state) : base($"{STATE_ERROR} state: {state}")
        {

        }
    }
}
