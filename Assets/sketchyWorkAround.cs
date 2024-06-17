using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sketchyWorkAround : MonoBehaviour
{
    public GameObject prefab;

    public BackgroundHandler bgs;
    public BaseSceneManager s;
    public TextAsset t;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefab);
        DialogueController d = prefab.GetComponent<DialogueController>();
        d.bgs = bgs;

        d.end = true;
        d.t = t;
        d.runDialogue();
    }
}
