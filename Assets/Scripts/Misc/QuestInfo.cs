using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI QTitle, QInfo;
    [SerializeField]
    public Button GoBtn,ClaimBtn,CancelBtn;
    public Quest ThisQuest;
    private float timer;
    private bool QStart, QEnded, QCancelled;

    public int StrReward, DefReward, SpdReward;

    private PlayerInfo pi;
    private MyQuestSaver mqs;


    // Start is called before the first frame update
    void Start()
    {
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        mqs = GameObject.Find("PlayerInfo").GetComponent<MyQuestSaver>();
        if (mqs.thisQuest == ThisQuest) { 
            ThisQuest.time = mqs.timer;
            GoBtn.gameObject.SetActive(false);
            CancelBtn.gameObject.SetActive(true);
        }
        StrReward = ThisQuest.statBoost["Attack"];
        DefReward = ThisQuest.statBoost["Defense"];
        SpdReward = ThisQuest.statBoost["Speed"];

    }

    // Update is called once per frame
    void Update()
    {
        if (QStart) {
            timer -= Time.deltaTime;
            mqs.timer = timer;
            CancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = timer.ToString("0") + "\nCancel"; 
        }
        if (QEnded || QCancelled) timer = ThisQuest.time;
        if (timer <= 0)
        {
            QEnded = true;
            QStart = false;
            QCancelled = false;

            CancelBtn.gameObject.SetActive(false);
            ClaimBtn.gameObject.SetActive(true);
        }
    }

    public void setQTitle(string title)
    {
        QTitle.text = title;
    }

    public void setQInfo(string desc, float time)
    {
        QInfo.text = desc + "\n(Quest Length: " + time + " seconds.)";
        timer = time;
    }

    public void StartQuest()
    {
        mqs = GameObject.Find("PlayerInfo").GetComponent<MyQuestSaver>();
        if (!mqs.inQuest || mqs.thisQuest == ThisQuest)
        {
            QStart = true;
            pi.myCurrentQuest = ThisQuest;
            mqs.thisQuest = ThisQuest;
            mqs.inQuest = true;
            pi.questActive = true;
            mqs.timerActivated = true;
            GoBtn.gameObject.SetActive(false);
            CancelBtn.gameObject.SetActive(true);
        }
    }

    public void EndQuest()
    {
        QEnded = false;
        mqs.inQuest = false;
        ClaimBtn.gameObject.SetActive(false);
        GoBtn.gameObject.SetActive(true);
        //Insert Reward Items once you created an Inventory system
        pi.mySlime.setAtk(pi.mySlime.getAtk() + StrReward);
        pi.mySlime.setDef(pi.mySlime.getAtk() + DefReward);
        pi.mySlime.setSpd(pi.mySlime.getAtk() + SpdReward);
        pi.myCurrentQuest = new Quest();
        mqs.thisQuest = new Quest();
        pi.questActive = false;
    }

    public void CancelQuest()
    {
        QCancelled = true;
        QStart = false;
        QEnded = false;
        pi.myCurrentQuest = new Quest();
        mqs.thisQuest = new Quest();
        mqs.inQuest = false;
        pi.questActive = false;
        CancelBtn.gameObject.SetActive(false);
        GoBtn.gameObject.SetActive(true);
    }

}
