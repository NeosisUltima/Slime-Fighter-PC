using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Image MyIcon;
    [SerializeField]
    private Image MyImage;
    
    private string roomName;
    private int roomSize;
    private int playerCount;

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetRoom(string nameInput, string displayName, Color SlimeElemColor, Sprite MySlimeSprite)
    {
        roomName = nameInput;
        nameText.text = displayName;
        MyIcon.color = SlimeElemColor;
        MyImage.sprite = MySlimeSprite;
    }
}
