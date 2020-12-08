using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

public class DataSendingCustomType : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private SerializeData _playerSerial = new SerializeData();
    [SerializeField]
    private SerializeSlimeData _slimeSerial = new SerializeSlimeData();
    [SerializeField]
    private PlayerData pd = new PlayerData();
    [SerializeField]
    private bool _sendAsTyped = true;
    [SerializeField]
    private PlayerInfo pi;

    // Start is called before the first frame update
    void Start()
    {
        PhotonPeer.RegisterType(typeof(SerializeData), (byte)'Z', SerializeData.Serialize, SerializeData.Deserialize);
        PhotonPeer.RegisterType(typeof(SerializeSlimeData), (byte)'Y', SerializeSlimeData.SerializeSlime, SerializeSlimeData.DeserializeSlime);
        PhotonPeer.RegisterType(typeof(PlayerData), (byte)'X', PlayerData.Serialize, PlayerData.Deserialize);
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_slimeSerial.myPSlime == null)
        {
            pd.losts = pi.losts;
            pd.wins = pi.wins;
            pd.mySlime = pi.mySlime;
            pd.pName = pi.pName;
            SendPlayerSerial(pd, _sendAsTyped);
        }
    }

    private void SendPlayerSerial(PlayerData data, bool typed)
    {
        if (!typed)
            base.photonView.RPC("RPC_RecievedMySerialData", RpcTarget.AllViaServer, PlayerData.Serialize(pd));
        else
            base.photonView.RPC("RPC_TypedRecievedMySerialData",RpcTarget.AllViaServer,pd);
    }

    [PunRPC]
    private void RPC_RecievedMySerialData(byte[] datas, byte[] sdatas)
    {
        PlayerData result = (PlayerData)PlayerData.Deserialize(datas);
        //print("Recieved player: " + result.pName + ", with Slime Partner: " + result.mySlime.getNme());
    }

    [PunRPC]
    private void RPC_TypedRecievedMySerialData(PlayerData datas)
    {
        //print("Recieved player: " + datas.pName + " with partner: " + datas.mySlime.getNme());
    }

}
