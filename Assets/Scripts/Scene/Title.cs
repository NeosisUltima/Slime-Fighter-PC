using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo pd;
    
    // Start is called before the first frame update
    void Start()
    {
        pd = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ShiftFromTitleScreen()
    {
        pd.LoadPlayer();

        if(pd.pName == "")
        {
            SceneManager.LoadScene("InputName");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
