using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerniceDialogueCharacterHandler : DialogueCharacterHandler
{
    new public void instantiateCharacter(string name)
    {
        GameObject newCharacter = Instantiate(c, parent);
        newCharacter.name = name;

        newCharacter.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(name.ToLower());
        newCharacter.GetComponent<SpriteRenderer>().sortingLayerID = 0;
        newCharacter.GetComponent<SpriteRenderer>().sortingOrder = 1;

        if (openPosition == 1)
        {
            newCharacter.transform.position = position1.position - new Vector3(0, 6.5f, 0);
            openPosition += 1;
            characters[0] = newCharacter;
        } else
        {
            newCharacter.transform.position = position2.position - new Vector3(0, 6.5f, 0);
            characters[1] = newCharacter;
        }
    }
}
