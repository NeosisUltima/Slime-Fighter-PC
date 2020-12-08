using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenSlimeInfoBox : MonoBehaviour
{
    private PlayerInfo pi;
    [SerializeField]
    private TextMeshProUGUI myinfoText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        myinfoText.text = "Slime Name: " + pi.mySlime.getNme() + "\nOwner: " + pi.pName + "\nSlime Type: " + pi.mySlime.getElement().MySlimeElement + "\nWeak to: " + pi.mySlime.getElement().MyWeakElement + "\nStrong to: " + pi.mySlime.getElement().MyStrongElement;
    }

    public void OpenBox()
    {
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        if (pi.SlimeHatched)
        {
            this.gameObject.SetActive(true);
            UpdateText();
        }
    }

    public void CloseBox()
    {
        this.gameObject.SetActive(false);
    }
}
