using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;



public class BattleScreen : MonoBehaviourPunCallbacks, IInRoomCallbacks, IConnectionCallbacks
{
    /*BattleScreen Script
     * 
     * -Should allow connectivity for rock paper scissors
     * -sends info back to player for whether or not they won or lost
     * -if player did lose, should refer back to their playerinfo script for reincarnation function
     * -if player won, should boost stats of the player's slime
     */

    [SerializeField]
    private GameObject p1SlimePanel, p2SlimePanel,DisconnectPanel, WinPanel,LosePanel,HelpPanel,PlayerDisconnect, QuitPanel;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private CanvasGroup buttons;
    [SerializeField]
    private Button helpBtn;
    [SerializeField]
    private ItemDatabase idata;
    private BattlePanel p1, p2;
    private bool p1PanelSet, p2PanelSet = false;
    private bool p1MoveMade, p2MoveMade;
    public int turn = 0;
    private int prevHealth;
    public enum Results { None = 0, Draw, Win, Loss}

    private PhotonView pv;

    public MultiplayerSetupController msc;
    private PlayerInfo pi;
    //Events
    public const byte SetPlayerOverlayEvent = 1;

    private bool LostGame = false;
    private bool GameSet = false;

    void Awake()
    {
        p1 = p1SlimePanel.GetComponent<BattlePanel>();
        p2 = p2SlimePanel.GetComponent<BattlePanel>();
        pv = GetComponent<PhotonView>();
        //msc = GameObject.Find()
        pv.RPC("SetBattleOverlay",RpcTarget.All);
        prevHealth = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeHealth"];
        pi = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

        Hashtable resetMyHealth = new Hashtable();
        resetMyHealth.Add("PlayerSlimeHealth", 100);
        PhotonNetwork.LocalPlayer.SetCustomProperties(resetMyHealth);

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) p2.MyCurrentHealthText.gameObject.SetActive(false);
        else p1.MyCurrentHealthText.gameObject.SetActive(false);

        idata.BuildItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(!p1PanelSet || !p2PanelSet) pv.RPC("SetBattleOverlay", RpcTarget.All);

        if (this.theirMove != -1) print(this.theirMove);
        if (this.myMove != -1 && this.theirMove != -1)
        {
            FindWinner();

            if (this.myResult == Results.Win)
            {
                pv.RPC("Attack",RpcTarget.Others,PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.GetNext());
            }
            else if(this.myResult == Results.Draw)
            {
                pv.RPC("DrawAttack", RpcTarget.Others, PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.GetNext());
            }
            StartCoroutine(ShowResults());

        }

        if((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeHealth"] <= 0 && !GameSet)
        {
            this.LostGame = true;
            pv.RPC("CheckForWinner", RpcTarget.All);
            GameSet = true;

        }

        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeHealth"] != prevHealth)
        {
            Debug.Log("Health has changed for this player.... was " + prevHealth + ", now it's " + (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeHealth"]);
            prevHealth = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeHealth"];
            pv.RPC("UpdateScene", RpcTarget.All);
        }
    }

   public void HelpPanelOpen()
   {
        if (HelpPanel.activeSelf == false)
        {
            HelpPanel.SetActive(true);
            helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
            
        }
        else
        {
            HelpPanel.SetActive(false);
            helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Help";

        }
    }

    public void HelpPanelClose()
   {
        HelpPanel.SetActive(false);
   }

    //ROCK PAPER SCISSORS
    private static int ROCK = 0;
    private static int PAPER = 1;
    private static int SCISSORS = 2;
    private static int NONE = -1;

    [SerializeField]
    private int myMove = -1; 
    [SerializeField]
    private int theirMove = -1;

    private string myChoice, theirChoice = "";

    private int MyPanelNum = 0;
    private Results myResult;

    public const byte  MoveMadeEvent = 2;

    private int stradded, defadded, spdadded;
    public void ChooseRock()
    {
        this.MakeMove(ROCK,"ROCK");
    }


    public void ChoosePaper()
    {
        this.MakeMove(PAPER,"PAPER");
    }


    public void ChooseScissors()
    {
        this.MakeMove(SCISSORS,"SCISSORS");
    }

    public void MakeMove(int selection,string seltxt)
    {
        this.myMove = selection;
        this.myChoice = seltxt;
        p1MoveMade = true;
        buttons.interactable = false;
        pv.RPC("SendMove", RpcTarget.Others, this.myMove,this.myChoice);
        infoText.text = "Waiting for Opponent...";
    }

    [PunRPC]
    public void SendMove(int choice,string choicetxt)
    {
        if (this.theirMove == -1)
        {
            this.theirMove = choice;
            this.theirChoice = choicetxt;
            p2MoveMade = true;
        }
    }

    public void FindWinner()
    {
        Debug.Log("Determining Winner");
        this.myResult = Results.None;
        if (this.myMove == this.theirMove) this.myResult = Results.Draw;
        else if (this.myMove == ROCK) this.myResult = (this.theirMove == SCISSORS) ? Results.Win : Results.Loss;
        else if (this.myMove == PAPER) this.myResult = (this.theirMove == ROCK) ? Results.Win : Results.Loss;
        else if (this.myMove == SCISSORS) this.myResult = (this.theirMove == PAPER) ? Results.Win : Results.Loss;

        this.myMove = -1;
        this.theirMove = -1;
    }

    //IENumerators
    public IEnumerator ShowResults()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) { p1.myChoiceUpdate(this.myChoice); p2.myChoiceUpdate(this.theirChoice); }
        else { p2.myChoiceUpdate(this.myChoice); p1.myChoiceUpdate(this.theirChoice); }
        infoText.text = this.myResult == Results.Win ? "You attack the oponent." : "You have been attacked.";
        this.myResult = Results.None;
        yield return new WaitForSeconds(5.0f);
        infoText.text = "Choose an attack.";
        buttons.interactable = true;
        p1.myChoiceUpdate("");
        p2.myChoiceUpdate("");
        yield return new WaitForSeconds(1.0f);
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;

    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if(eventCode == MoveMadeEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            this.theirMove = (int)data[0];
        }
    }

    [PunRPC]
    public void UpdateScene()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == p1.myPlayeerName)
            {
                p1.myCurrentSlimeHealth = (int)player.CustomProperties["PlayerSlimeHealth"];

            }
            else if (player.NickName == p2.myPlayeerName)
            {
                p2.myCurrentSlimeHealth = (int)player.CustomProperties["PlayerSlimeHealth"];

            }

        }
        p1.MyCurrentHealthText.text = p1.myCurrentSlimeHealth + "/100";
        p2.MyCurrentHealthText.text = p2.myCurrentSlimeHealth + "/100";

        p1.UpdateHealthColor();
        p2.UpdateHealthColor();
    }


    //PUN References
    [PunRPC]
    public void SetBattleOverlay()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int myNumber = p.ActorNumber;

            if (myNumber == 1)
            {
                this.p1.myPlayeerName = p.NickName;
                this.p1.Name.text = (string)p.CustomProperties["PlayerSlimeName"];
                this.p1.myCurrentSlimeHealth = (int)p.CustomProperties["PlayerSlimeHealth"];
                this.p1.SlimeImage.sprite = p1.ssl[(int)p.CustomProperties["PlayerSlimeEvolStage"] - 1].SlimeStage[(int)p.CustomProperties["PlayerSlimeElement"]];
                this.p1.MyCurrentHealthText.text = p1.myCurrentSlimeHealth + "/100";
                this.MyPanelNum = 1;
            }
            else if (myNumber == 2)
            {
                this.p2.myPlayeerName = p.NickName;
                this.p2.Name.text = (string)p.CustomProperties["PlayerSlimeName"];
                this.p2.myCurrentSlimeHealth = (int)p.CustomProperties["PlayerSlimeHealth"];
                this.p2.SlimeImage.sprite = p2.ssl[(int)p.CustomProperties["PlayerSlimeEvolStage"] - 1].SlimeStage[(int)p.CustomProperties["PlayerSlimeElement"]];
                this.p2.MyCurrentHealthText.text = p2.myCurrentSlimeHealth + "/100";
                this.MyPanelNum = 2;
            }
        }
    }
    
    [PunRPC]
    public void Attack(Player a, Player b)
    {
        Hashtable temp = new Hashtable();
        float modifier = 1;
        Debug.Log(a.CustomProperties["PlayerSlimeName"] + " attacked " + b.CustomProperties["PlayerSlimeName"] + "!");

        //Elemental weaknesses
        if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeElement"]) modifier = 1;
        else if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeStrElement"]) modifier = 0.5f;
        else if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeWkElement"]) modifier = 2;
        else modifier = 1;

        //Critical Chance
        int critCHance = Random.Range(1, 3);
        bool crit = critCHance == 1 ? false : true;

        if (crit) modifier *= 1.75f;

        int dmg = (int)(((int)a.CustomProperties["PlayerSlimeAttack"] * 2 - (((int)b.CustomProperties["PlayerSlimeDefense"]) * 3)/2)*modifier);
        if (dmg <= 0) dmg =(int)( 1 * modifier);

        int curhlth = (int)b.CustomProperties["PlayerSlimeHealth"] - dmg;
        temp.Add("PlayerSlimeHealth", curhlth);
        b.SetCustomProperties(temp);
        Debug.Log(b.CustomProperties["PlayerSlimeName"] + " currently has " + b.CustomProperties["PlayerSlimeHealth"] + "!");
    }

    [PunRPC]
    public void DrawAttack(Player a, Player b)
    {
        Hashtable temp = new Hashtable();
        float modifier = 1;
        Debug.Log(a.CustomProperties["PlayerSlimeName"] + " attacked " + b.CustomProperties["PlayerSlimeName"] + "!");

        //Elemental weaknesses
        if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeElement"]) modifier = 1;
        else if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeStrElement"]) modifier = 0.5f;
        else if ((int)a.CustomProperties["PlayerSlimeElement"] == (int)b.CustomProperties["PlayerSlimeWkElement"]) modifier = 2;
        else modifier = 1;

        //Critical Chance
        int critCHance = Random.Range(1, 3);
        bool crit = critCHance == 1 ? false : true;

        if (crit) modifier *= 1.75f;

        int dmg = (int)(((int)a.CustomProperties["PlayerSlimeSpeed"] - (((int)b.CustomProperties["PlayerSlimeDefense"])) / 2) * modifier);
        if (dmg <= 0) dmg =(int)( 1 * modifier);

        int curhlth = (int)b.CustomProperties["PlayerSlimeHealth"] - dmg;
        temp.Add("PlayerSlimeHealth", curhlth);
        b.SetCustomProperties(temp);
        Debug.Log(b.CustomProperties["PlayerSlimeName"] + " currently has " + b.CustomProperties["PlayerSlimeHealth"] + "!");
    }

    [PunRPC]
    public void CheckForWinner()
    {
        
        Hashtable updateStats = new Hashtable();

        if (LostGame)
        {
            this.LosePanel.SetActive(true);
            updateStats.Add("PlayerLosses",pi.losts++);
            PhotonNetwork.LocalPlayer.SetCustomProperties(updateStats);
        }
        else
        {
            this.WinPanel.SetActive(true);

            this.stradded = Random.Range(0, (int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeAttack"]);
            this.defadded = Random.Range(0, (int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeDefense"]);
            this.spdadded = Random.Range(0, (int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeSpeed"]);

            updateStats.Add("PlayerWins", pi.wins++);
            updateStats.Add("PlayerSlimeAttack", pi.mySlime.getAtk() + stradded);
            updateStats.Add("PlayerSlimeDefense", pi.mySlime.getDef() + defadded);
            updateStats.Add("PlayerSlimeSpeed", pi.mySlime.getSpd() + spdadded);

            int randItemDrops = Random.Range(0, 5);

            for(int i = 0; i < randItemDrops; i++)
            {
                pi.myInventory.Add(idata.GiveItem());
            }

            randItemDrops = Random.Range(0, 50);
            if(randItemDrops >= 45)
            {
                if((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 0)
                    pi.myInventory.Add(idata.GiveItem(15));
                else if ((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 1)
                    pi.myInventory.Add(idata.GiveItem(16));
                else if ((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 2)
                    pi.myInventory.Add(idata.GiveItem(17));
                else if ((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 3)
                    pi.myInventory.Add(idata.GiveItem(18));
                else if ((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 4)
                    pi.myInventory.Add(idata.GiveItem(19));
                else if ((int)PhotonNetwork.LocalPlayer.GetNext().CustomProperties["PlayerSlimeElement"] == 5)
                    pi.myInventory.Add(idata.GiveItem(20));
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(updateStats);

            this.WinPanel.GetComponent<WinPanel>().wintext.text += "\n Strength +" + stradded + "\n Defense +" + defadded + "\n Speed +" + spdadded + "\nYou also recieved a few items...";
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        LeavingRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        DisconnectPanel.SetActive(true);
    }

    public void LeavingRoom()
    {
        if (!GameSet)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.RemovedFromList = true;
            DisconnectPanel.SetActive(true);
        }
    }

    public void ReturnToLobby()
    {
        if (!LostGame)
        {
            pi.mySlime.setAtk((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeAttack"]);
            pi.mySlime.setDef((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeDefense"]);
            pi.mySlime.setSpd((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeSpeed"]);
            pi.mySlime.SlimeWins += 1;
            //pi.wins = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerWins"];
        }
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
        SceneManager.LoadScene("Fight");
    }

    public void ReturnToLobbyDisconnected()
    {
        
        pi.mySlime.setAtk(pi.mySlime.getAtk() + 3);
        pi.mySlime.setDef(pi.mySlime.getDef() + 3);
        pi.mySlime.setSpd(pi.mySlime.getSpd() + 3);
        pi.mySlime.SlimeWins += 1;
        //pi.wins++;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
        SceneManager.LoadScene("Fight");
    }

    public void ReturnToHomeDisconnected()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Main");
    }

    public void ReturnToHome()
    {
        
        if (!LostGame)
        {
            pi.mySlime.setAtk((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeAttack"]);
            pi.mySlime.setDef((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeDefense"]);
            pi.mySlime.setSpd((int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeSpeed"]);
            pi.mySlime.SlimeWins += 1;
            //pi.wins = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerWins"];
        }
        else
        {
            pi.slimeDied = true;
            //pi.losts = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerLosses"];
        }
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Main");
    }

    public int CheckIfLower(int num)
    {
        if (num > 5) return num;
        else return 5;
    }

    public void QuitMatch()
    {
        QuitPanel.SetActive(true);
    }

    public void OnQuitYes()
    {
        PlayerDisconnect.SetActive(true);
        LostGame = true;

        Hashtable updateStats = new Hashtable();

        int myAtk = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeAttack"];
        int myDef = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeDefense"];
        int mySpd = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerSlimeSpeed"];

        pi.losts++;
        pi.mySlime.setAtk(CheckIfLower(pi.mySlime.getAtk() - Random.Range(1, (myAtk / 3))));
        pi.mySlime.setDef(CheckIfLower(pi.mySlime.getDef() - Random.Range(1, (myDef / 3))));
        pi.mySlime.setSpd(CheckIfLower(pi.mySlime.getSpd() - Random.Range(1, (mySpd / 3))));

        PhotonNetwork.LocalPlayer.SetCustomProperties(updateStats);
        
    }

    public void OnQuitNo()
    {
        QuitPanel.SetActive(false);
    }

    IEnumerator rejoinLobby()
    {
        Hashtable StatChanged = new Hashtable();
        StatChanged.Add("PlayerSlimeAttack", pi.mySlime.getAtk());
        StatChanged.Add("PlayerSlimeDefense", pi.mySlime.getDef());
        StatChanged.Add("PlayerSlimeSpeed", pi.mySlime.getSpd());
        PhotonNetwork.LocalPlayer.SetCustomProperties(StatChanged);
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
}
