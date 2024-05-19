using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnieSceneController : MonoBehaviour
{
    public TextAsset annieDay2;
    public DialogueController d;
    // Start is called before the first frame update
    void Start()
    {
        d.t = annieDay2;
        d.runDialogue();
    }
}
