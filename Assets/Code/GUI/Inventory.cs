using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    List<Item> itemList;
    List<GameObject> instantiatedMenu;

    public GameObject inventoryTile;
    public Canvas canvasObject;

    int inventoryLength = 0;

    public Inventory(int inventoryLength)
    {
        this.inventoryLength = inventoryLength;
        itemList = new List<Item>();
    }

    public void addItem(int itemID, int pictureID, string name, string[] miscProperties)
    {
        if (itemList.Count <= inventoryLength)
        {
            itemList.Add(new Item(itemID, pictureID, name, miscProperties));
        }
    }

    public void removeItem(int index)
    {
        itemList.RemoveAt(index);
    }

    public void moveItem(int movingItemIndex, int movingToIndex)
    {
        Item tempItemStorage = itemList[movingToIndex]; //[1][2][2]
        itemList[movingToIndex] = itemList[movingItemIndex]; //[1][1][2]
        itemList[movingItemIndex] = tempItemStorage; //[2][1][2]
        tempItemStorage = new Item();
    }

    public void openInventory()
    {
        instantiatedMenu.Add(GameObject.Instantiate(inventoryTile) as GameObject);
    }

    public void closeInventory()
    {

    }
}

public class Item
{
    int itemID;
    int pictureID;
    string name;
    string[] miscProperties;
    public Item(int itemID, int pictureID, string name, string[] miscProperties)
    {
        this.itemID = itemID;
        this.pictureID = pictureID;
        this.name = name;
        this.miscProperties = miscProperties;
    }
    public Item() //To stop any possible dupes
    {
    }
}
