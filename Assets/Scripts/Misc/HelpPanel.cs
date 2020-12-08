using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprPage;
    [SerializeField]
    private List<string> txtPage;
    [SerializeField]
    private Button nextBtn, prevBtn, helpCloseBtn;
    [SerializeField]
    private TextMeshProUGUI mytext;
    [SerializeField]
    private Image myImg;

    private int pgNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        nextBtn.gameObject.SetActive(false);
        txtPage = new List<string>()
        {
            "Mobile Slime FIghter is a \"somewhat\" competitive mobile game, where you and your opponent face off against each other in a classic game of Rock, Paper, Scissor.",
            "Both players choose rock, paper, or scissors and sees who's the victor.",
            "In the Event of a tie, both players will end up getting attacked(damage is based on speed stat instead of attack).",
            "There are also type advantages based on the element of the slime.",
            "Once it's game over, the winner will recieve stat boost and a few items, while the loser will lose their slime and will have to start with a new slime.",
            "If a player drops, the winner will get a +3 stat boost to their slime."
        };
        mytext.text = txtPage[pgNum];
        myImg.sprite = sprPage[pgNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (pgNum >= sprPage.Count - 1) nextBtn.gameObject.SetActive(false);
        else nextBtn.gameObject.SetActive(true);

        if (pgNum <= 0) prevBtn.gameObject.SetActive(false);
        else prevBtn.gameObject.SetActive(true);

        mytext.text = txtPage[pgNum];
        myImg.sprite = sprPage[pgNum];
    }

    public void NextPage()
    {
        pgNum++;
    }

    public void PrevPage()
    {
        pgNum--;
    }

    
}
