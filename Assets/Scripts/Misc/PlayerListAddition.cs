using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListAddition : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo player;
    public int element;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
