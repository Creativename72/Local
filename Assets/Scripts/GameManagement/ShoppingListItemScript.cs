using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script to control individual items on the shopping list
public class ShoppingListItemScript : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;

    private bool acquired = false; //tracks if this item has been acquired

    void Update()
    {
        //if item is acquired, cross off item in the list
        if (acquired)
        {
            itemName.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            itemName.fontStyle = FontStyles.Normal;
        }
    }

    //sets the name text of this item
    public void SetName(string name)
    {
        itemName.text = name;
    }

    public string GetName()
    {
        return itemName.text;
    }

    //sets the image of this item
    public void SetImage(Sprite image)
    {
        itemImage.sprite = image;
    }

    //check if this item has been acquired
    public bool CheckAcquired()
    {
        return acquired;
    }

    //toggle the acquired state of this item
    public void ToggleAcquired()
    {
        acquired = !acquired;
    }

    //set acquired state to a state
    public void SetAcquired(bool newAcquired)
    {
        acquired = newAcquired;
    }
}
