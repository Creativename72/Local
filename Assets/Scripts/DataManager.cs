using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameState gameState;
    public static DataManager Instance { get; private set; }

    private FileDataHandler fileDataHandler;
    private string fileName = "local.json";

    public void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        fileDataHandler ??= new FileDataHandler(Application.persistentDataPath, fileName);
    }
    public void NewGame()
    {
        this.gameState = new GameState();
    }
    public void SaveGame()
    {
        fileDataHandler.SaveFile(gameState);
    }
    public void LoadGame()
    {
        this.gameState = fileDataHandler.LoadFile();
    }
}
