using System;
using System.IO;
using UnityEngine;
public class FileDataHandler
{
    private string dataDirectoryPath;
    private string dataFileName;
    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public void SaveFile(GameState state)
    {
        string path = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string json = JsonUtility.ToJson(state, true);

            using FileStream stream = new(path, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data to file " + path + "\n" + e);
        }
    }
    public GameState LoadFile()
    {
        string path = Path.Combine(dataDirectoryPath, dataFileName);
        GameState state = null;
        if (File.Exists(path))
        {
            try
            {
                using FileStream stream = new(path, FileMode.Open);
                using StreamReader reader = new(stream);
                string json = reader.ReadToEnd();

                state = JsonUtility.FromJson<GameState>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data from file " + path + "\n" + e);
            }
        }
        else
        {
            state = new GameState();
        }
        return state;
    }
}
