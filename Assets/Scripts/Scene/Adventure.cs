using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Adventure : MonoBehaviour
{
    /*--------------------------------------------------|
     * What's Needed in this 
     * ----->Quest unlocks
     * ----->Quest Requirements
     * ----->Access to the scrollviewList
     * ----->Connect to the QuestList and Quest Prefab 
     * ----->Enable and disable SlimeUse
     * ----->Items Rewards List.
     * -------------------------------------------------|
     * What Other Scripte Needs to be Modified
     * ----->PlayerInfo
     * -------------------------------------------------|
     * Scripts to Be Made
     * ----->Inventory Master Class
     * ----->Quest Master Class
     * -------------------------------------------------|
     * 
     */

    private QuestDatabase qd;
    private List<Quest> UnlockedQuests = new List<Quest>();
    private PlayerInfo pi;
    private MyQuestSaver mqs;

    [SerializeField]
    private GameObject QuestListings;
    [SerializeField]
    private Transform QuestBox;

    private float timer = 15;

    // Start is called before the first frame update
    void Start()
    {
        qd = GetComponent<QuestDatabase>();
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        mqs = GameObject.Find("PlayerInfo").GetComponent<MyQuestSaver>();
        mqs.leftAdventure = false;
        UpdateList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshList()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        UnlockedQuests = new List<Quest>();

        foreach(Transform child in QuestBox.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Quest q in qd.QuestList)
        {
            //DO STUFF
            if(pi.mySlime.getAtk() >= q.getStrReq() && pi.mySlime.getDef() >= q.getDefReq() && pi.mySlime.getSpd() >= q.getSpdReq())
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
        if (q == mqs.thisQuest) { 
            tempInfo.setQInfo(q.info, pi.QuestTimer);
            tempInfo.StartQuest();
        }
        else tempInfo.setQInfo(q.info, q.time);
        tempInfo.StrReward = q.statBoost["Attack"];
        tempInfo.DefReward = q.statBoost["Defense"];
        tempInfo.SpdReward = q.statBoost["Speed"];
    }

    public void BackToHome()
    {
        mqs.leftAdventure = true; 
        SceneManager.LoadScene("Main");
    }
}
