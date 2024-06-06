using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestButton : MonoBehaviour
{
    void OnMouseDown() {
        Click();
    }
    
    public void Click() {
        MapController.Instance.LoadNextScene("Map");
        Debug.Log("done");
    }
}
