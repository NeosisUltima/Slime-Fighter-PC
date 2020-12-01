using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAdventureBox : MonoBehaviour
{
    [SerializeField]
    private QuestDatabase qd;
    private List<Quest> UnlockedQuests = new List<Quest>();
    [SerializeField]
    private PlayerInfo pi;
    [SerializeField]
    private MyQuestSaver mqs;

    [SerializeField]
    private GameObject QuestListings;
    [SerializeField]
    private Transform QuestBox;

    private float timer = 15;

    private void Update()
    {
    }

    public void RefreshList()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        UnlockedQuests = new List<Quest>();

        foreach (Transform child in QuestBox.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest q in qd.QuestList)
        {
            //DO STUFF
            if (pi.mySlime.getAtk() >= q.getStrReq() && pi.mySlime.getDef() >= q.getDefReq() && pi.mySlime.getSpd() >= q.getSpdReq())
            {
                UnlockedQuests.Add(q);
                FillScroll(q);
            }
            else
            {
                Debug.Log("All requirements not reached...");
            }
        }
    }

    public void FillScroll(Quest q)
    {
        GameObject tempListing = Instantiate(QuestListings, QuestBox);
        QuestInfo tempInfo = tempListing.GetComponent<QuestInfo>();
        tempInfo.ThisQuest = q;
        tempInfo.setQTitle(q.title);
        if (mqs.thisQuest.getTitle() == q.getTitle())
        {
            tempInfo.setQInfo(q.info, mqs.timer);
            tempInfo.StartQuest();
        }
        else tempInfo.setQInfo(q.info, q.time);
        tempInfo.StrReward = q.statBoost["Attack"];
        tempInfo.DefReward = q.statBoost["Defense"];
        tempInfo.SpdReward = q.statBoost["Speed"];
    }

    public void OpenBox() {
        this.gameObject.SetActive(true);
        qd = GetComponent<QuestDatabase>();
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        mqs = GameObject.Find("PlayerInfo").GetComponent<MyQuestSaver>();
        mqs.leftAdventure = false;
        UpdateList();
    }

    public void CloseBox() {
        mqs.leftAdventure = true;
        this.gameObject.SetActive(false); 
    }
}
