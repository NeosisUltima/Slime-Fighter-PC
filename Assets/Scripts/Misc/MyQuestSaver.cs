using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuestSaver : MonoBehaviour
{
    public Quest thisQuest;
    public float timer;
    public bool leftAdventure, timerActivated, inQuest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thisQuest == null)
        {
            timer = 0;
        }

        if (thisQuest != null && !timerActivated)
        {
            timerActivated = true;
        }
        else if(thisQuest != null && timerActivated){
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
            timer = 0;
            timerActivated = false;
        }
    }

}
