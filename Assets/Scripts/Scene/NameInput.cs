using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo pReference;
    [SerializeField]
    private TextMeshProUGUI nameText, confirmText;
    [SerializeField]
    private GameObject ConfirmCanvas;

    public Button EnterBtn;
    // Start is called before the first frame update
    void Start()
    {
        pReference = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterButtonActive()
    {
        if (EnterBtn.isActiveAndEnabled == false)
            EnterBtn.gameObject.SetActive(true);
        else if (nameText.text == "")
            EnterBtn.gameObject.SetActive(false);
    }

    public void ConfirmationCanvas()
    {
        confirmText.text = "Are you sure you want to be called " + nameText.text + "? \n(You won't be able to change it later.)";
        ConfirmCanvas.SetActive(true);
    }

    public void Decline()
    {
        ConfirmCanvas.SetActive(false);
    }

    public void Confirm()
    {
        SetNameText();
        SceneManager.LoadScene("Main");
    }

    public void SetNameText()
    {
        pReference.SetPName(nameText.text);
    }
}
