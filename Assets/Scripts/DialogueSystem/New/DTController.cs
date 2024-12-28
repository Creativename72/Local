using UnityEngine;

public class DTController : MonoBehaviour
{
    [SerializeField] private TextAsset text;
    [SerializeField] private DialogueBox box;

    private DialogueTree tree;
    private bool scoutNice;

    // Start is called before the first frame update
    void Start()
    {
        scoutNice = false;

        tree = new DialogueTree(text);
        tree.SetFunction("Func", () => Debug.Log("Func!"));
        tree.SetVariable("var", "wonderful ");
        tree.SetVariable("ScoutNice", () => scoutNice);
        tree.Parse();
        PrintDialogue();

        box.Enable(true);
        box.SetDialogue("Tyler", "Hello world!", true);
        box.SetChoice(0, "It makes you an idiot.", null);
        box.SetChoice(1, "Weak.", "Nice unlocked!");
        box.SetChoice(2, "Strong.", null);
        box.AddChoiceListener(num => Debug.Log($"Choice {num} was pressed!"));
    }

    // Update is called once per frame
    void Update()
    {
        return;

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
        if (Input.GetKeyDown(KeyCode.N))
        {
            scoutNice = !scoutNice;
            tree.Parse(true);
        }
    }

    private void PrintDialogue()
    {
        DialogueTree.Line line = tree.GetLine();
      
        if (line == null)
        {
            Debug.Log("END");
        }
        else
        {
            Debug.Log(line.ToString());
        }
    }
}

