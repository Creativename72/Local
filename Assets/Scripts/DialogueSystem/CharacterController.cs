using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Handles placing character sprites on the game screen
/// </summary>
public class CharacterController : MonoBehaviour
{
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform rightPos;
    [SerializeField] private Transform centerPos;

    private List<DialogueCharacter> created;

    /// <summary>
    /// Creates a sprite of a character in scene
    /// </summary>
    /// <param name="character">Character being created</param>
    /// <param name="location">Location of the character</param>
    /// <exception cref="CharacterException">Thrown if using an invalid sprite number</exception>
    public DialogueCharacter CreateCharacter(DialogueCharacter character)
    {
        created ??= new();

        DialogueCharacter newCharacter = Instantiate(character);
        newCharacter.name = character.GetCharacterName();
        newCharacter.Show(true);

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

            switch (created.GetLocation())
            {
                case DialogueCharacter.Location.LEFT: parent = leftPos; break;
                case DialogueCharacter.Location.RIGHT: parent = rightPos; break;
                case DialogueCharacter.Location.CENTER: parent = centerPos; break;
            }

            created.transform.parent = parent;
            created.transform.localPosition = Vector3.zero;
        });
    }
}
