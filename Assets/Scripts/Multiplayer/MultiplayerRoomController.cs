using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerRoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField]
    private GameObject roomPanel;
    [SerializeField]
    private GameObject roomPanelCoverP2;
    [SerializeField]
    private GameObject disconnectPanel;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject ReadyButton;
    [SerializeField]
    private GameObject CancelButton;
    [SerializeField]
    private List<SlimeSpriteList> ssl = new List<SlimeSpriteList>();
    private GameObject p1, p2;
    private bool p1Added, p2Added;
    [SerializeField]
    private bool ImReady, TheyreReady;
    //PlayerInfo
    [SerializeField]
    private Image p1SlimeImage, p2SlimeImage;
    [SerializeField]
    private TextMeshProUGUI p1user, p2user, p1wins, p2wins, p1loss, p2loss,p1SlimeStats,p2SlimeStats;
    [SerializeField]
    private List<BattlerInfo> myRoomBattlers = new List<BattlerInfo>();
    public PlayerData myInfo = new PlayerData();
    public SerializeData sd = new SerializeData();
    public SerializeSlimeData ssd = new SerializeSlimeData();
    private Player[] PhotonPlayers;
    public int playersInRoom = 0;
    public int MyNumberInRoom;
    public int playerInGame;
    public int playersReady = 0;

    //Photon Variables
    public static MultiplayerRoomController mrc;
    private PhotonView pv;

    public bool isGameLoaded;
    public int currentScene;

    private bool readyToCount, readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers, atMaxPlayer, timeToStart;

    private void Awake()
    {
        PlayerInfo tmp = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        myInfo.pName = tmp.pName;
        myInfo.wins = tmp.wins;
        myInfo.losts = tmp.losts;
        myInfo.mySlime = tmp.mySlime;
        if (MultiplayerRoomController.mrc == null)
        {
            MultiplayerRoomController.mrc = this;
        }
        else
        {
            if(MultiplayerRoomController.mrc != this)
            {
                Destroy(MultiplayerRoomController.mrc.gameObject);
                MultiplayerRoomController.mrc = this;
            }
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayer = 4;
        timeToStart = startingTime;
    }

    void Update()
    {
        if(playersInRoom == 1)
        {
            RestartTimer();

        }
        if (!isGameLoaded)
        {
            if(this.ImReady == true && this.TheyreReady == true)
            {
                StartGame();
            }
        }
    }

    void ClearPlayerListings()
    {
        p1user.text = "NA";
        p1wins.text = "Wins: ";
        p1loss.text = "Loss: ";
        p2user.text = "NA";
        p2wins.text = "Wins: ";
        p2loss.text = "Loss: ";
        p1Added = false;
        p2Added = false;
        foreach(Transform t in this.gameObject.transform)
        {
            Destroy(t.gameObject);
        }
        roomPanelCoverP2.SetActive(true);
    }

    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {

                if (!p1Added)
                {
                    p1user.text = (string)player.CustomProperties["PlayerName"];
                    p1wins.text = "Wins: " + player.CustomProperties["PlayerWins"];
                    p1loss.text = "Loss: " + player.CustomProperties["PlayerLosses"];
                    p1SlimeStats.text = "Atk: " + player.CustomProperties["PlayerSlimeAttack"] + "\nDef: " + player.CustomProperties["PlayerSlimeDefense"] + "\nSpd: " + player.CustomProperties["PlayerSlimeSpeed"];
                    p1SlimeImage.sprite = ssl[(int)player.CustomProperties["PlayerSlimeEvolStage"]-1].SlimeStage[(int)player.CustomProperties["PlayerSlimeElement"]];
                    p1Added = true;
                }
                else if(!p2Added)
                {
                    p2user.text = (string)player.CustomProperties["PlayerName"];
                    p2wins.text = "Wins: " + player.CustomProperties["PlayerWins"];
                    p2loss.text = "Loss: " + player.CustomProperties["PlayerLosses"];
                    p2SlimeStats.text = "Atk: " + player.CustomProperties["PlayerSlimeAttack"] + "\nDef: " + player.CustomProperties["PlayerSlimeDefense"] + "\nSpd: " + player.CustomProperties["PlayerSlimeSpeed"];
                    p2SlimeImage.sprite = ssl[(int)player.CustomProperties["PlayerSlimeEvolStage"]-1].SlimeStage[(int)player.CustomProperties["PlayerSlimeElement"]];
                    roomPanelCoverP2.SetActive(false);
                    p2Added = true;
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined");

        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);  
        ReadyButton.SetActive(true);
        ClearPlayerListings();
        //RoomUpdated();
        ListPlayers();

        PhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = PhotonPlayers.Length;
        MyNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myInfo.pName;

        if (MultiplayerSetupController.msc.delayStart)
        {
            if(playersInRoom > 1)
            {
                readyToCount = true;

            }
            if(playersInRoom == MultiplayerSetupController.msc.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        /*else
        {
            StartGame();
        }*/
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PhotonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        ClearPlayerListings();
        //RoomUpdated();
        ListPlayers();
        if (MultiplayerSetupController.msc.delayStart)
        {
            if (playersInRoom > 1)
            {
                readyToCount = true;

            }
            if (playersInRoom == MultiplayerSetupController.msc.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        /*else
        {
            StartGame();
        }*/
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        playersInRoom--;
        ClearPlayerListings();
        //RoomUpdated();
        ListPlayers();
    }

    public void StartGame()
    {
        isGameLoaded = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        PhotonNetwork.LoadLevel(MultiplayerSetupController.msc.multiplayerScene);

    }

    [PunRPC]
    public void PlayerReady(bool readyChoice)
    {
        this.TheyreReady = readyChoice;
    }

    public void ReadyForBattleButton()
    {
        if(!this.ImReady)
            this.ImReady = true;
        else
            this.ImReady = false;

        pv.RPC("PlayerReady", RpcTarget.Others, this.ImReady);
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayer = 4;
        readyToCount = false;
        readyToStart = false;
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        disconnectPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }

    public void LeavingRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
            BackOnClick();
        }
        else
        {
            disconnectPanel.SetActive(true);
        }
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSetupController.msc.multiplayerScene)
        {
            isGameLoaded = true;
            pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
        }
        else
        {
            RPC_CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length)
        {
            pv.RPC("RPC_CreatePlayer", RpcTarget.All, myInfo);
        }
    }

    [PunRPC]
    private void RoomUpdated()
    {
        pv.RPC("GetBattlerInfo", RpcTarget.All,PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PhotonNetworkPlayer"),transform.position,Quaternion.identity,0);
    }

    [PunRPC]
    private void GetBattlerInfo(Player player)
    {
        /*
        myCustomInfo.Add("PlayerName", myInfo.pName);
        myCustomInfo.Add("PlayerWins", myInfo.wins);
        myCustomInfo.Add("PlayerLosses", myInfo.losts);
        myCustomInfo.Add("PlayerSlimeName", myInfo.mySlime.getNme());
        myCustomInfo.Add("PlayerSlimeAttack", myInfo.mySlime.getAtk());
        myCustomInfo.Add("PlayerSlimeDefense", myInfo.mySlime.getDef());
        myCustomInfo.Add("PlayerSlimeSpeed", myInfo.mySlime.getSpd());
        myCustomInfo.Add("PlayerSlimeHealth", myInfo.mySlime.getHlth());
        myCustomInfo.Add("PlayerSlimeElement", myInfo.mySlime.getSlimeElement());
        myCustomInfo.Add("PlayerSlimeStrElement", myInfo.mySlime.getSlimeStrElement());
        myCustomInfo.Add("PlayerSlimeWkElement", myInfo.mySlime.getSlimeWkElement());
        myCustomInfo.Add("PlayerSlimeEvolStage", myInfo.mySlime.getSlimeEvolutionStage());
        */

        GameObject tempInfo = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BattlerInfo"), transform.position, Quaternion.identity, 0);
        tempInfo.gameObject.transform.parent = this.transform;
        BattlerInfo bi = tempInfo.GetComponent<BattlerInfo>();
        bi.pName = (string)player.CustomProperties["PlayerName"];
        bi.wins = (int)player.CustomProperties["PlayerWins"];
        bi.losts = (int)player.CustomProperties["PlayerLosses"];
        bi.mySlime = new Slime((string)player.CustomProperties["PlayerSlimeName"], (int)player.CustomProperties["PlayerSlimeAttack"], (int)player.CustomProperties["PlayerSlimeDefense"], (int)player.CustomProperties["PlayerSlimeSpeed"], (int)player.CustomProperties["PlayerSlimeElement"], (int)player.CustomProperties["PlayerSlimeEvolStage"]);
        bi.joinNumber = playersInRoom;
        print("Player " + bi.pName + " and his partner " + bi.mySlime.getNme() + " has joined the room.");
    }
}
