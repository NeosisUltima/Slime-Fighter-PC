using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title, info;
    public int  StrReq, DefReq, SpdReq;
    public float time;
    public Dictionary<string, int> statBoost = new Dictionary<string, int>();

    public Quest()
    {
        title = "";
        info = "";
        time = 0;
        StrReq = 0;
        DefReq = 0;
        SpdReq = 0;
    }

    public Quest(string TITLE, string INFO, float TIME, int STRREQ, int DEFREQ, int SPDREQ, Dictionary<string, int> statBoost)
    {
        title = TITLE;
        info = INFO;
        time = TIME;
        StrReq = STRREQ;
        DefReq = DEFREQ;
        SpdReq = SPDREQ;
        this.statBoost = statBoost;
    }

    public string getTitle() { return title; }
    public string getInfo() { return info; }
    public float getTime() { return time; }
    public int getStrReq() { return StrReq; }
    public int getDefReq() { return DefReq; }
    public int getSpdReq() { return SpdReq; }

}
