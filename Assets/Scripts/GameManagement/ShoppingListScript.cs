using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListScript : MonoBehaviour
{
    [SerializeField] GameObject shoppingItemsContainer;
    [SerializeField] GameObject shoppingItemPrefab; //prefab of shoppint items to add to list

    [SerializeField] Animator shoppingListAnimator;
    private bool show = false;

    int itemCount = 0;

    static GameObject Instance; //singleton for shopping list

    void Start()
    {
        if(Instance == null)
        {
            Instance = this.gameObject;
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //adds a new item and sets name
    public void AddItem(string name)
    {
        if(name != null)
        {
            GameObject newItem = Instantiate(shoppingItemPrefab, shoppingItemsContainer.transform);
            ShoppingListItemScript itemScript = newItem.GetComponent<ShoppingListItemScript>();
            if(itemScript != null)
            {
                itemScript.SetName(name);
            }
            itemCount++;
        }
    }

    //adds a new item and sets name and image
    public void AddItem(string name, Sprite image)
    {
        if (name != null)
        {
            GameObject newItem = Instantiate(shoppingItemPrefab, shoppingItemsContainer.transform);
            ShoppingListItemScript itemScript = newItem.GetComponent<ShoppingListItemScript>();
            if (itemScript != null)
            {
                itemScript.SetName(name);
                itemScript.SetImage(image);
            }
            itemCount++;
        }
    }

    //finds an item by name
    public void CheckOffItem(string name)
    {
        foreach(Transform shoppingItem in shoppingItemsContainer.transform)
        {
            ShoppingListItemScript itemScript = shoppingItem.gameObject.GetComponent<ShoppingListItemScript>();
            if(itemScript != null && itemScript.GetName().Equals(name) && !itemScript.CheckAcquired())
            {
                itemScript.SetAcquired(true);
                return;
            }
        }
    }

    public void ToggleHidden()
    {
        show = !show;
        shoppingListAnimator.SetBool("ShowList", show);
    }

    public void SetHidden(bool newShow)
    {
        show = newShow;
        shoppingListAnimator.SetBool("ShowList", show);
    }
}
