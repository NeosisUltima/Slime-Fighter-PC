using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFeedBox : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo pi;
    [SerializeField]
    private GameObject FeedBoxListings;
    [SerializeField]
    private Transform FeedBox;
    private void Start()
    {
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }

    public void UpdateList()
    {
        foreach (Transform child in FeedBox.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Items i in pi.myInventory.myInventory)
        {
            FillInventoryList(i, pi.myInventory.inventoryCount[i.itemName]);
        }
    }

    public void FillInventoryList(Items i, int amount)
    {
        bool isAlreadyInstantiated = false;

        foreach(Transform t in FeedBox.transform)
        {
            if (t.GetComponent<ItemInfo>().thisItem == i)
            {
                isAlreadyInstantiated = true;
                break;
            }
        }

        if (!isAlreadyInstantiated)
        {
            GameObject tempListing = Instantiate(FeedBoxListings, FeedBox);
            ItemInfo tempInfo = tempListing.GetComponent<ItemInfo>();
            tempInfo.thisItem = i;
            tempInfo.itmName.text = i.itemName;
            tempInfo.itmDesc.text = i.desc;
            tempInfo.MaxItemInInventory = pi.myInventory.inventoryCount[i.itemName];
        }
    }

    public void OpenBox() {this.gameObject.SetActive(true); pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>(); UpdateList(); }

    public void CloseBox() { this.gameObject.SetActive(false); }
}
