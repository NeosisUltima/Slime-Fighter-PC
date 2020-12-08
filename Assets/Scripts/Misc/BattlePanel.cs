using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlimeSpriteList
{
    public List<Sprite> SlimeStage;
}

public class BattlePanel : MonoBehaviourPunCallbacks
{
    public Image SlimeImage,SlimeHealthColor;
    public RectTransform Health;
    public TextMeshProUGUI Name, MyCurrentHealthText, MyChoice;
    public List<SlimeSpriteList> ssl = new List<SlimeSpriteList>();

    public int myCurrentSlimeHealth = 100;
    public bool isGameOver = false;

    public string myPlayeerName;
    // Start is called before the first frame update
    private void Awake()
    {
        SlimeHealthColor.color = Color.green;
    }

    void Start()
    {
    }

    private void Update()
    {
        if(myCurrentSlimeHealth == 0)
        {
            isGameOver = true;
        }
    }

    // Update is called once per frame
    public void UpdateHealthColor()
    {
        //SlimeHealthColor Update
        if (myCurrentSlimeHealth <= 67) SlimeHealthColor.color = Color.yellow;
        if (myCurrentSlimeHealth <= 33) SlimeHealthColor.color = Color.red;
    }

    public void myChoiceUpdate(string txt)
    {
        MyChoice.text = txt;
    }
}
