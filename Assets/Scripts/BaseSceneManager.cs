using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{

    public DialogueController d;
    public TextAsset t;
    // Start is called before the first frame update
    void Start()
    {
        d.t = t;
        d.runDialogue();
    }
}
