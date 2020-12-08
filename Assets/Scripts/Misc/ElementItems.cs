using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementItems : Items
{
    public string elemType = "";
    public Items itembase = new Items();

    public ElementItems(Items i, string elemType)
    {
        itembase = i;
        this.elemType = elemType;
    }
}
