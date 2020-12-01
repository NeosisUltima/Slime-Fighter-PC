using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;



public class MultiplayerLobbyController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static MultiplayerLobbyController mlc;
    public GameObject quickMatchBtn, cancelBtn;

    [SerializeField]
    private int RoomSize;

    [SerializeField]
    private List<Color> slimeSprites;
    [SerializeField]
    private List<SlimeSpriteList> ssl;

    private List<RoomInfo> RoomListings;
    [SerializeField]
    private GameObject roomListingPrefab;

    private PlayerInfo playerInfo,myInfo;
    private string roomName;
    public Transform roomsContainer;

    private Hashtable myCustomInfo = new Hashtable();
    private float timer = 120;
    private void Awake()
    {
        mlc = this;
        myInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.JoinLobby();

        //myCustomInfo.["PlayerInfo"] =  myInfo;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = myInfo.pName;

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

        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomInfo);

        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties);

        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            OnRoomListUpdate(RoomListings);
        }
    }

    public void QuickStart()
    {
        quickMatchBtn.SetActive(false);
        cancelBtn.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomListings = new List<RoomInfo>();
        //PhotonNetwork.NickName = playerInfo.pName;
        quickMatchBtn.SetActive(true);
    }

    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            quickMatchBtn.SetActive(true);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();
        foreach (RoomInfo room in roomList)
        {
            print(room.Name);
            ListRoom(room);
        }
    }

    void RemoveRoomListings()
    {
        while(roomsContainer.childCount != 0)
        {
            Destroy(roomsContainer.GetChild(0).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen == true && room.IsVisible == true && room.PlayerCount < room.MaxPlayers)
        {
            string temp = "";
            int slimeStage = int.Parse(room.Name[room.Name.Length - 1].ToString()) - 1;
            int slimeElem = int.Parse(room.Name[room.Name.Length - 3].ToString());

            for(int i = 0; i < room.Name.Length - 4; i++)
            {
                temp = temp+room.Name[i];
            }
            GameObject tempListing = Instantiate(roomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name,temp, slimeSprites[slimeElem], ssl[slimeStage].SlimeStage[slimeElem]);
        }
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }

    public void OnRoomSizeChanged(string sizeIn)
    {
        RoomSize = int.Parse(sizeIn);
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    public void CreateRoom()
    {
        //int randomRoomNumber = Random.Range(0, 10000); //creaing a random name for the room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetupController.msc.maxPlayers};
        PhotonNetwork.CreateRoom("Room: " + PhotonNetwork.LocalPlayer.NickName + "_"+myInfo.mySlime.getSlimeElement() + "_" + myInfo.mySlime.getSlimeEvolutionStage(), roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Name is taken... trying again");
        //CreateRoom();
    }

    public void MatchmakingCancel()
    {
        quickMatchBtn.SetActive(true);
        cancelBtn.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void LoadHomeScreen()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Main");
    }

}
