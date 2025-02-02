using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// This class holds the general game state of the game
/// </summary>

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    // Get this object from anywhere without needing it to be in scene
    public void Awake()
    {
        if (_Instance != null)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _Instance = this;
            this.gameObject.name = _Instance.GetType().ToString();
            DontDestroyOnLoad(_Instance.gameObject);
        }

        gameState ??= new();
    }

    private static GameStateManager _Instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameObject().AddComponent<GameStateManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public void Reset()
    {
        this.gameState = new GameState();
    }

    public void SetFlag<T>(string name, T value)
    {
        List<FieldInfo> fields = GetFields(name);
        if (fields.Count == 0)
        {
            throw new GameFlagsException($"Flag {name} does not exist.");
        }
        else if (fields.Count != 1)
        {
            throw new GameFlagsException("Multiple flags with the same name.");
        }

        FieldInfo field = fields[0];
        object val = field.GetValue(gameState);
        if (val is not T)
        {
            throw new GameFlagsException("Flag is not of given type.");
        }

        field.SetValue(gameState, value);
    }
    public T GetFlag<T>(string name)
    {
        List<FieldInfo> fields = GetFields(name);
        if (fields.Count == 0)
        {
            throw new GameFlagsException($"Flag {name} does not exist.");
        }
        else if (fields.Count != 1)
        {
            throw new GameFlagsException("Multiple flags with the same name.");
        }

        FieldInfo field = fields[0];
        object val = field.GetValue(gameState);
        if (val is not T casted)
        {
            throw new GameFlagsException("Flag is not of given type.");
        }

        return casted;
    }

    public void ForEachFlag<T>(Action<string, T> action)
    {
        foreach (FieldInfo field in gameState.GetType().GetFields())
        {
            string name = field.Name;
            object val = field.GetValue(gameState);
            if (val is T casted)
            {
                action.Invoke(name, casted);
            }
        }
    }

    private List<FieldInfo> GetFields(string fieldName)
    {
        return gameState.GetType().GetFields().ToList().FindAll(element => element.Name == fieldName);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}

internal class GameFlagsException : Exception
{
    public GameFlagsException(string message) : base(message)
    {
    }
}