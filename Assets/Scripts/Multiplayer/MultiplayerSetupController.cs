using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MultiplayerSetupController : MonoBehaviour
{
    public static MultiplayerSetupController msc;
    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    // Start is called before the first frame update
    void Awake()
    {
        if(MultiplayerSetupController.msc == null)
        {
            MultiplayerSetupController.msc = this;
        }
        else
        {
            if(MultiplayerSetupController.msc != null)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
    }
    // Update is called once per frame
    
}
