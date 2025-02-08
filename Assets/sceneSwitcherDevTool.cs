using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneSwitcherDevTool : MonoBehaviour
{
    public bool b;

    // Update is called once per frame
    void Update()
    {
        if (b)
        {
            b = false;
            GameManager.Instance.ChangeScene("Map");
        }
    }
}
