using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabase : MonoBehaviour
{
    public List<Quest> QuestList;

    private void Awake()
    {
        BuildQuests();
    }

    public void BuildQuests()
    {
        QuestList = new List<Quest>()
        {
            /* Quest Database is a updating type of system where we, creators, update the number of quests in game. Just create a new quest, with a Title(string), 
             * Description of the quest(string), Length of time to do quest(float), the required stats in strength, defense,& speed (INT), and boosted stats in a dictionary format...
             * 
             * public Quest(string TITLE, string INFO, float TIME, int STRREQ, int DEFREQ, int SPDREQ, Dictionary<string, int> statBoost)*/
            new Quest("Journey Begins", "You, a young slime trainer begins his journey lookin for excitement, what you will find may shock you.", 30, 5,5,5, new Dictionary<string, int>{
                {"Attack", 1 },
                {"Defense", 1},
                {"Speed", 1}
            }),
            new Quest("Slime Training", "After travelling with your slime, you see that he is acting sluggish. Might be a good ide to train it.", 120, 5,5,5, new Dictionary<string, int>{
                {"Attack", Random.Range(0,3) },
                {"Defense", Random.Range(0,3)},
                {"Speed", Random.Range(0,3)}
            }),
            new Quest("Slime Challenger Billy", "You meet a kid named Billy who wants to fight you. Time to take him on.", 180, 5,5,5, new Dictionary<string, int>{
                {"Attack", Random.Range(0,5) },
                {"Defense", Random.Range(0,5)},
                {"Speed", Random.Range(0,5)}
            }),
        };
    }
}
