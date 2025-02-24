using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a character that can say dialogue lines in game.
/// Used to hold various properties held by a certain character in game
/// </summary>

public class DialogueCharacter : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private string identifier;
    [SerializeField] private string characterName;
    [SerializeField] private Location location;

    [Header("Sprite attributes")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> characterSprites;

    [Header("Sound")]
    [SerializeField] private AudioClip[] talkSFX;

    private const string ERROR = "Invalid arguments used";

    private float customX;

    // getters and setters for visible fields
    public string GetIdentifier() { return identifier; }
    public string GetCharacterName() { return characterName; }
    public Location GetLocation() { return location; }
    public float GetCustomX() { return customX; }
    public void SetName(string characterName) { this.characterName = characterName; }
    public void SetLocation(Location location) { this.location = location; }
    public void SetLocation(Location location, float customX) { 
        this.location = location;
        this.customX = customX;
    }
    public void SetSprite(int index)
    {
        // spriteRenderer number must be within parameters
        if (index >= 0 && index < characterSprites.Count)
        {
            spriteRenderer.sprite = characterSprites[index];

        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
    public AudioClip GetRandomTalkSFX() { return this.talkSFX[(int) (UnityEngine.Random.value * talkSFX.Length)]; }
    public void SetVisible(bool show)
    {
        spriteRenderer.enabled = show;
    }

    public void Awake()
    {
        SetSprite(0);
        SetVisible(false);
    }
    public enum Location
    {
        FAR_LEFT,
        LEFT,
        CENTER_LEFT,
        CENTER,
        CENTER_RIGHT,
        RIGHT,
        FAR_RIGHT,
        CUSTOM,
    }

    public class CharacterException : Exception
    {
        public CharacterException(string message) : base($"{ERROR}. {message}") { }
    }
}
