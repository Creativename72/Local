using UnityEngine;

public class DTController : MonoBehaviour
{
    [SerializeField] private TextAsset text;

    private DialogueTree tree;

    // Start is called before the first frame update
    void Start()
    {
        tree = new DialogueTree(text);
        tree.SetFunction("Func", () => Debug.Log("Func!"));
        tree.SetVariable("var", "wonderful ");
        tree.Parse();
        PrintDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tree.AdvanceLine(-1);
            PrintDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            tree.AdvanceLine(0);
            PrintDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tree.AdvanceLine(1);
            PrintDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tree.AdvanceLine(2);
            PrintDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PrintDialogue();
        }
    }

    private void PrintDialogue()
    {
        DialogueTree.Line line = tree.GetLine();
        if (line is DialogueTree.DialogueLine dLine)
            Debug.Log(dLine.GetDialogue());

        if (line is DialogueTree.OptionalLine oLine)
        {
            string s = "";
            foreach (DialogueTree.OptionalLine.Option o in oLine.GetOptions())
            {
                s += o.ToString() + " ";
            }
            Debug.Log(s);
        }
        if (line == null)
        {
            Debug.Log("END");
        }
    }
}
