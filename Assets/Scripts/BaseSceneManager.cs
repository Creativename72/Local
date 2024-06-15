using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{

    public DialogueController d;
    public TextAsset t;
    public bool endOnDialogueEnd;
    public bool incrementDay;
    // Start is called before the first frame update
    void Start()
    {
        d.t = t;
        d.runDialogue();
    }

    public void endScene()
    {
        if (false)
        {
            MapController.Instance.currentStage++;
        }
        MapController.Instance.LoadNextScene("Map");
    }
}
