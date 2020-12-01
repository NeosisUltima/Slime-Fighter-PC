using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Items> ItemList;
    public int totalWeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        BuildItems();
        GetWeightSum();
    }

    // Update is called once per frame
    public void BuildItems()
    {
        ItemList = new List<Items>()
        {
            
            //Won By fighting a slime that is weaker than yours.
            new Items("PowerEssence","A part of a slime that contains a portion of their attack. Boosts Attack by 1",30,new Dictionary<string, int>()
            {
                { "Attack",1 },
                {"Defense", 0 },
                {"Speed", 0 }
            }),
            new Items("DefenseEssence","A part of a slime that contains a portion of their defense. Boosts Defense by 1",30,new Dictionary<string, int>()
            {
                { "Attack",0 },
                {"Defense", 1 },
                {"Speed", 0 }
            }),
            new Items("SpeedEssence","A part of a slime that contains a portion of their speed. Boosts Speed by 1",30,new Dictionary<string, int>()
            {
                { "Attack",0 },
                {"Defense", 0 },
                {"Speed", 1 }
            }),
            new Items("RandomEssence","An unstable part of a slime that may increase or decrease your slime stats. Changes stats slightly", 25, new Dictionary<string, int>()
            {
                {"Attack",Random.Range(-2,2) },
                {"Defense",Random.Range(-2,2) },
                {"Speed",Random.Range(-2,2) }
            }),
            new Items("OmegaEssence","A part of a slime. Boosts all stats by 1",20,new Dictionary<string, int>()
            {
                {"Attack",1 },
                {"Defense", 1 },
                {"Speed", 1 }
            }),
            //Won By fighting a slime that is equal to yours (3% drop chance)
            new Items("SuperPowerEssense","A major part of a slime's attack stat. Boosts Attack by 5.", 15,new Dictionary<string, int>()
            {
                {"Attack", 5 },
                {"Defense", 0 },
                {"Speed",0 }
            }),
            new Items("SuperDefenseEssense","A major part of a slime's attack stat. Boosts Defense by 5.",15, new Dictionary<string, int>()
            {
                {"Attack", 0 },
                {"Defense", 5 },
                {"Speed",0 }
            }),
            new Items("SuperSpeedEssense","A major part of a slime's defense stat. Boosts Speed by 5.",15, new Dictionary<string, int>()
            {
                {"Attack", 0 },
                {"Defense", 0 },
                {"Speed",5 }
            }),
            new Items("SuperOmegaEssense","A major part of a slime. Boosts all stats by 5.",5, new Dictionary<string, int>()
            {
                {"Attack", 5 },
                {"Defense", 5 },
                {"Speed",5 }
            }),
            new Items("SuperRandomEssence","An unstable part of a slime that may increase or decrease your slime stats. Changes stats partially",10,new Dictionary<string, int>()
            {
                {"Attack",Random.Range(-5,5) },
                {"Defense",Random.Range(-5,5) },
                {"Speed",Random.Range(-5,5) }
            }),
            //Won By fighting a slime that is greater yours (2% drop)
            new Items("UltraPowerEssense","A extreme part of a slime's attack stat. Boosts Attack by 10.",7, new Dictionary<string, int>()
            {
                {"Attack", 10 },
                {"Defense", 0 },
                {"Speed",0 }
            }),
            new Items("UltraDefenseEssense","A extreme part of a slime's attack stat. Boosts Defense by 10.",7, new Dictionary<string, int>()
            {
                {"Attack", 0 },
                {"Defense", 10 },
                {"Speed",0 }
            }),
            new Items("UltraSpeedEssense","A extreme part of a slime's defense stat. Boosts Speed by 10.",7, new Dictionary<string, int>()
            {
                {"Attack", 0 },
                {"Defense", 0 },
                {"Speed",10 }
            }),
            new Items("UltraOmegaEssense","A extreme part of a slime. Boosts all stats by 10.",2, new Dictionary<string, int>()
            {
                {"Attack", 10 },
                {"Defense", 10 },
                {"Speed",10 }
            }),
            //Random At any point in battle (75% chance drop).         
            new Items("UltraRandomEssence","An unstable part of a slime that may increase or decrease your slime stats. Changes stats extremely",5,new Dictionary<string, int>()
            {
                {"Attack",Random.Range(-10,10) },
                {"Defense",Random.Range(-10,10) },
                {"Speed",Random.Range(-10,10) }
            }),
        };
        
    }

    public void GetWeightSum()
    {
        foreach(Items i in ItemList)
        {
            totalWeight += i.weight;
        }
        Debug.Log("total weight: " + totalWeight);
    }

    public Items GiveItem()
    {
        int randChoice = Random.Range(1, totalWeight);
        int tempWeight = 0;
        Items itemGiven = new Items();

        print(randChoice);

        foreach(Items i in ItemList)
        {
            tempWeight += i.weight;
            Debug.Log("current weight: " + tempWeight);
            if(randChoice <= tempWeight)
            {
                itemGiven = i;
                break;

            }
        }
        
        return itemGiven;
    }
}
