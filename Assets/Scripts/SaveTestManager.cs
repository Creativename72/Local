using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.LoadGame();
    }
    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGame();
    }
}
