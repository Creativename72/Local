using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// K: Having this be it's own separate class is kind of unnecessary (I think?) but I'm
// doing it here as a precuation to avoid merge problems in commonly used scripts.
// Also makes it easier to change cursor behavior if we want to edit that in the future
public class CursorChanger : MonoBehaviour
{
    public Texture2D newCursor;
    public Vector2 cursorImageOffset = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(newCursor, cursorImageOffset, cursorMode);
    }

}
