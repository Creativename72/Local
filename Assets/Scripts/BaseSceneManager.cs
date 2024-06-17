using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{

    public DialogueController d;
    public TextAsset t;
    public bool endOnDialogueEnd;
    public bool nextScene = false;
    // Start is called before the first frame update
    void Start()
    {
        d.t = t;
        d.runDialogue();
    }

    private void Update()
    {
        if (nextScene)
        {
            nextScene = false;
            endScene();
        }
    }

    public void endScene()
    {
        MapController.Instance.LoadNextScene("Map");
    }
}
