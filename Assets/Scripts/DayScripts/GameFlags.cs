using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using static DialogueTree;

/// <summary>
/// This class holds the general game state of the game
/// </summary>
public class GameFlags : MonoBehaviour
{
    public bool KnowBerniceLiveTime;
    public bool PackedLightly;
    public bool MetWalter = false;
    public bool MetAnnie = false;
    public bool MetTyler = false;
    public bool WalterSafePath = false;
    public bool WalterRockyPath = false;
    public bool AnnieWalterSlowDown = false;
    public bool TylerPranked = false;
    public bool TylerMessy = false;
    public bool AnnieRangDoorbell = false;
    public bool AnnieJoked = false;
    public bool TylerVeggiesToBernice = false;
    public bool WalterCares = false;
    public bool WalterSolitude = false;
    public bool WalterConflicted = false;
    public bool WalterFullStory = false;
    public bool WalterFish = false;
    public bool AnnieBrushOff = false;
    public bool AnnieConnections = false;
    public bool AnnieFullStory = false;
    public bool AnnieBreak = false;
    public bool AnnieSelf = false;
    public bool TylerCool = false;
    public bool TylerTryhard = false;
    public bool TylerFindSelf = false;
    public bool AnnieRaces = false;
    public bool AnnieTakesABreak = false;
    public bool TylerIsCool = false;
    public bool TylerKeepBike = false;
    public bool TylerKeepArt = false;
    public bool TylerKeepGrill = false;
    public bool WalterFishForTyler = false;
    public bool WalterLakesidePhoto = false;
    public bool WalterTalkedAboutLakesidePhoto = false;
    public bool WalterTalkedAboutTylerFishing = false;
    public bool TylerStory = false;
    public bool AnnieWalterBreak = false;
    public bool WalterBlamesHimself;
    public bool WalterBlamesTyler;
    public bool WalterBlamesEveryone;
    public bool WalterGoodEndingPush;
    public bool WalterGoodEnding;
    public bool TylerFrustratedWithWalter;
    public bool TylerWantsDistance;
    public bool TylerWantsIndependence;
    public bool TylerGoodEnding;

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
    }

    private static GameFlags _Instance;
    public static GameFlags Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameObject().AddComponent<GameFlags>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    public static void SetFlag<T>(string name, T value)
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
        object val = field.GetValue(Instance);
        if (val is not T)
        {
            throw new GameFlagsException("Flag is not of given type.");
        }

        field.SetValue(Instance, value);
    }
    public static T GetFlag<T>(string name)
    {
        List<FieldInfo> fields = GetFields(name);
        if (fields.Count == 0) {
            throw new GameFlagsException($"Flag {name} does not exist.");
        } 
        else if (fields.Count != 1)
        {
            throw new GameFlagsException("Multiple flags with the same name.");
        }

        FieldInfo field = fields[0];
        object val = field.GetValue(Instance);
        if (val is not T casted) {
            throw new GameFlagsException("Flag is not of given type.");
        }

        return casted;
    }

    public static void ForEachFlag<T>(Action<string, T> action)
    {
        foreach (FieldInfo field in Instance.GetType().GetFields())
        {
            string name = field.Name;
            object val = field.GetValue(Instance);
            if (val is T casted)
            {
                action.Invoke(name, casted);
            }
        }
    }

    private static List<FieldInfo> GetFields(string fieldName)
    {
        return Instance.GetType().GetFields().ToList().FindAll(element => element.Name == fieldName);
    }

   
}

internal class GameFlagsException : Exception
{
    public GameFlagsException(string message) : base(message)
    {
    }
}