using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Rendering.DebugUI;
using static DialogueCharacter;

/// <summary>
/// Handles placing character sprites on the game screen
/// </summary>
public class CharacterController : MonoBehaviour
{
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform farLeftPos;
    [SerializeField] private Transform centerLeftPos;
    [SerializeField] private Transform centerRightPos;
    [SerializeField] private Transform farRightPos;

    private List<DialogueCharacter> created;

    /// <summary>
    /// Creates a spriteRenderer of a character in scene
    /// </summary>
    /// <param name="character">Character being created</param>
    /// <param name="location">Location of the character</param>
    /// <exception cref="CharacterException">Thrown if using an invalid spriteRenderer number</exception>
    public DialogueCharacter CreateCharacter(DialogueCharacter character)
    {
        created ??= new();

        DialogueCharacter newCharacter = Instantiate(character);
        newCharacter.name = character.GetCharacterName();
        newCharacter.SetVisible(false);

        created.Add(newCharacter);

        return newCharacter;
    }

    /// <summary>
    /// Deletes all created characters
    /// </summary>
    public void DeleteCharacters()
    {
        created.ForEach(created => Destroy(created));
        created.Clear();
    }

    public void Update()
    {
        // update locations based on character
        created.ForEach(created =>
        {
            Transform parent = null;
            Location createdLoc = created.GetLocation();
            switch (createdLoc)
            {
                case Location.FAR_LEFT: parent = farLeftPos; break;
                case Location.LEFT: parent = leftPos; break;
                case Location.CENTER_LEFT: parent = centerLeftPos; break;
                case Location.CENTER: parent = centerPos; break;
                case Location.CENTER_RIGHT: parent = centerRightPos; break;
                case Location.RIGHT: parent = rightPos; break;
                case Location.FAR_RIGHT: parent = farRightPos; break;
                case Location.CUSTOM: parent = centerPos; break;
            }

            created.transform.parent = parent;
            created.transform.localPosition = Vector3.zero;

            if (createdLoc == Location.CUSTOM)
            {
                created.transform.localPosition = new Vector3(created.GetCustomX(), 0, 0);
            }
        });
    }
}
