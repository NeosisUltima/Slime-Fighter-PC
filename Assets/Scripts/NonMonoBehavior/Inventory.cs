using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField]
    public Dictionary<string, int> inventoryCount;
    public List<Items> myInventory;
    public Inventory()
    {
        inventoryCount = new Dictionary<string, int>();
        myInventory = new List<Items>();
    }

    public void Add(Items item)
    {
        int val;
        if (inventoryCount.TryGetValue(item.itemName, out val))
        {
            inventoryCount[item.itemName] += 1;
        }
        else
        {
           inventoryCount.Add(item.itemName, 1);
        }

        myInventory.Add(item);

    }

    public void Remove(Items item, int Amount)
    {
        int val;


        if (inventoryCount.TryGetValue(item.itemName, out val))
        {
            inventoryCount[item.itemName] -= Amount;
        }

        if(inventoryCount[item.itemName] <= 0)
        {
            inventoryCount.Remove(item.itemName);
        }

        for (int i = 1; i <= Amount; i++)
        {
            myInventory.Remove(item);
        }
    }

    //Just in case items don't update correctly
    public void UpdateInventory()
    {
        inventoryCount.Clear();

        foreach(Items i in myInventory)
        {
            int val;

            if(inventoryCount.TryGetValue(i.itemName, out val))
            {
                inventoryCount[i.itemName] += 1;
            }
            else
            {
                inventoryCount.Add(i.itemName, 1);
            }
        }
    }
}
