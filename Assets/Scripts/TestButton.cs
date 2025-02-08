using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestButton : MonoBehaviour
{
    void OnMouseDown() {
        Click();
    }
    
    public void Click() {

        GameManager.Instance.ChangeScene("Map");
        Debug.Log("done");
    }
}
