using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    public TextMeshPro t;

    public void setText(string text)
    {
        t.text = text;
    }
}
