using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject c; //character prefab
    public Transform parent; //parent object

    public Transform position1;
    public Transform position2;

    public int openPosition = 1;

    protected GameObject[] characters = new GameObject[2];

    public void instantiateCharacter(string name)
    {
        GameObject newCharacter = Instantiate(c, parent);
        newCharacter.name = name;

        newCharacter.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(name.ToLower());

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

    public void clear()
    {
        openPosition = 1;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i])
            {
                Destroy(characters[i]);
                characters[0] = null;
            }
        }
    }
}
