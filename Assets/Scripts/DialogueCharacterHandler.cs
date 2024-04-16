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

    public void instantiateCharacter(string name)
    {
        GameObject newCharacter = Instantiate(c, parent);
        newCharacter.name = name;

        newCharacter.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(name);

        if (openPosition == 1)
        {
            newCharacter.transform.position = position1.position;
            openPosition += 1;
        } else
        {
            newCharacter.transform.position = position2.position;
        }
    }
}
