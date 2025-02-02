using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    private FileDataHandler fileDataHandler;
    private string fileName = "local.json";

    public void Awake()
    {
        fileDataHandler ??= new FileDataHandler(Application.persistentDataPath, fileName);
    }
    public void NewGame()
    {
        GameStateManager.Instance.Reset();
    }
    public void SaveGame()
    {
        fileDataHandler.SaveFile(GameStateManager.Instance.GetGameState());
    }
    public void LoadGame()
    {
        GameStateManager.Instance.SetGameState(fileDataHandler.LoadFile());
    }

    public GameState GetLoadedData()
    {
        return fileDataHandler.LoadFile();
    }
}
