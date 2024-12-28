using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a character that can say dialogue lines in game.
/// Used to hold various properties held by a certain character in game
/// </summary>

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
public class DialogueCharacter : ScriptableObject
{
    // identifier to be used in the dialogue text to refer to this character
    [SerializeField] private string identifier;
    // name of this character to be used when displaying text on the dialogue box
    [SerializeField] private string characterName;
    // sprites used by this character in scenes
    [SerializeField] private Sprite[] characterSprites;

    public static DialogueCharacter Walter { get => new("Walter", "Walter", new string[] { "walter" }); private set { } }
    public static DialogueCharacter Annie { get => new("Annie", "Annie", new string[] { "annie1", "annie2" }); private set { } }
    public static DialogueCharacter Tyler { get => new("Tyler", "Tyler", new string[] { "tyler1", "tyler2" }); private set { } }
    public static DialogueCharacter Scout { get => new("Scout", "Scout", new string[] { "scout" }); private set { } }
    public static DialogueCharacter Bernice { get => new("Bernice", "Bernice", new string[] { "bernice" }); private set { } }

    public DialogueCharacter(string identifier, string characterName, string[] spriteReferences)
    {
        this.identifier = identifier;
        this.characterName = characterName;
        
        for (int i = 0; i < spriteReferences.Length; i++)
        {
            characterSprites[i] = Resources.Load<Sprite>(spriteReferences[i]);
        }
    }
    public string Identifier { get { return identifier; } }
    public string CharacterName { get { return characterName; } }
    public Sprite[] CharacterSprites { get { return characterSprites; } }
}
