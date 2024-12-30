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
    [SerializeField] private AudioClip talkSFX;

    private const string ERROR = "Invalid arguments used";

    // getters and setters for visible fields
    public string GetIdentifier() { return identifier; }
    public string GetCharacterName() { return characterName; }
    public Location GetLocation() { return location; }
    public void SetCharacterName(string characterName) { this.characterName = characterName; }
    public void SetLocation(Location location) { this.location = location; }
    public void SetSprite(int index)
    {
        // sprite number must be within parameters
        if (index >= 0 && index < characterSprites.Count)
        {
            spriteRenderer.sprite = characterSprites[index];

        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
    public AudioClip GetTalkSFX() { return this.talkSFX; }
    public void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }

    public void Awake()
    {
        SetSprite(0);
        Show(false);
    }
    public enum Location
    {
        LEFT,
        RIGHT,
        CENTER,
    }

    public class CharacterException : Exception
    {
        public CharacterException(string message) : base($"{ERROR}. {message}") { }
    }
}
