using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    //Needs to COnnect to INventory System
    [SerializeField]
    public TextMeshProUGUI itmName, itmDesc, Amount;

    [SerializeField]
    private Button useButton,addButton,subButton;

    private int num = 0;
    public int MaxItemInInventory =0;
    [SerializeField]
    private int AtkBoost, DefBoost, SpdBoost, ElemChange;
    private bool statsAdded = false;
    public Items thisItem;

    private PlayerInfo pi;

    void Start()
    {
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        useButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisItem.itemName != "" && !statsAdded)
        {
            if (this.thisItem.myItemType == Items.ItemType.Stat)
            {
                AtkBoost = thisItem.iteminfo["Attack"];
                DefBoost = thisItem.iteminfo["Defense"];
                SpdBoost = thisItem.iteminfo["Speed"];
                
            }
            else
            {
                ElemChange = thisItem.iteminfo["Element"];
            }
            statsAdded = true;
        }
       

        if (num > 0) useButton.interactable = true;
        else useButton.interactable = false;
        
    }

    //Add 1 to the item use
    public void AddAmount()
    {
        if (num < MaxItemInInventory) { num++;}
        Amount.text = num.ToString();
    }

    //Subtract 1 to item use
    public void SubtractAmount()
    {
        if (num > 0) { num--; }
        Amount.text = num.ToString();
    }

    public void UseItem()
    {
        if (thisItem.myItemType == Items.ItemType.Stat)
        {
            Debug.Log(AtkBoost + ", " + DefBoost + ", " + SpdBoost);
            pi.mySlime.setAtk(pi.mySlime.getAtk() + (AtkBoost * num));
            pi.mySlime.setDef(pi.mySlime.getDef() + (DefBoost * num));
            pi.mySlime.setSpd(pi.mySlime.getSpd() + (SpdBoost * num));
        }
        else
        {
            if (!pi.SlimeEggTapped) pi.presetElem = ElemChange;
        }
        pi.myInventory.Remove(thisItem, num);

        num = 0;
        Amount.text = num.ToString();

        useButton.interactable = false;
        pi.myInventory.UpdateInventory();
        if (!pi.myInventory.inventoryCount.ContainsKey(thisItem.itemName)) Destroy(this.gameObject);
    }
}
