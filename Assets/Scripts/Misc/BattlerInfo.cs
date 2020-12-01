using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerInfo : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
{
    //public PlayerInfo InfoFromPlayer;

    public int wins, losts, joinNumber;
    public string pName = "";
    public Slime mySlime = new Slime();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //wins,lost,pName,Slime
        if (stream.IsWriting)
        {
            stream.SendNext(pName);
            stream.SendNext(wins);
            stream.SendNext(losts);
            stream.SendNext(mySlime);
        }
        else
        {
            pName = (string)stream.ReceiveNext();
            wins = (int)stream.ReceiveNext();
            losts = (int)stream.ReceiveNext();
            mySlime = (Slime)stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
