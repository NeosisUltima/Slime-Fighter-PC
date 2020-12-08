using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items
{
    public enum ItemType { Stat = 0, Element = 1};
    public string itemName, desc;
    public ItemType myItemType;
    public Dictionary< string, int> iteminfo;
    public int weight;

    public Items()
    {
        this.itemName = "";
        this.desc = "";
        this.myItemType = ItemType.Stat;
        this.weight = 0;
    }

    public Items(string itemName, string desc, int weight, Dictionary<string, int> itemInfo, ItemType mit)
    {
        this.itemName = itemName;
        this.desc = desc;
        this.weight = weight;
        this.myItemType = mit;
        this.iteminfo = itemInfo;
    }

    public string getName() { return itemName; }
    public string getDesc() { return desc; }

}
