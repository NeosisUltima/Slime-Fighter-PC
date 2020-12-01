using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items
{
    public string itemName, desc;
    public Dictionary< string, int> iteminfo;
    public int weight;

    public Items()
    {
        this.itemName = "";
        this.desc = "";
        this.weight = 0;
    }

    public Items(string itemName, string desc, int weight, Dictionary<string, int> itemInfo)
    {
        this.itemName = itemName;
        this.desc = desc;
        this.weight = weight;
        this.iteminfo = itemInfo;
    }

    public string getName() { return itemName; }
    public string getDesc() { return desc; }

}
