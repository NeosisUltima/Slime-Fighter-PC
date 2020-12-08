using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    //Refer to PlayerScript
    [SerializeField]
    private PlayerInfo pReference;
    [SerializeField]
    private List<SlimeSpriteList> ssl = new List<SlimeSpriteList>();
    //Baically used for scorekeeping, savedata, etc.
    private bool HasEgg;
    private bool EggHatched,SlimeNamed;
    private bool EggTapped = false;

    public ItemDatabase id;

    [SerializeField]
    private Button SlimeButton;
    [SerializeField]
    private TextMeshProUGUI atkStat, defStat, spdStat,SlimeName,OwnerName;
    [SerializeField]
    private Sprite baseEgg;
    [SerializeField]
    private List<Sprite> slimeElementImages = new List<Sprite>();
    [SerializeField]
    private List<Sprite> slimeEggImages = new List<Sprite>();

    private float timer = 15.0f;
    private int myCurrentIteration;
    private int myCurrentSlimeEvolution;
    //Name Input of Slime
    public Canvas NameCanvas;
    public TextMeshProUGUI NameInput;
    public Button EnterBtn;

    // Start is called before the first frame update

    void Start()
    {
        pReference = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        if (!pReference.LoggedInFirstTime) pReference.LoggedInFirstTime = true;
        if (pReference.mySlime.getNme() != "") { SlimeButton.image.sprite = ssl[pReference.mySlime.getSlimeEvolutionStage() - 1].SlimeStage[pReference.mySlime.getSlimeIndex()]; }
        else
        {
            pReference.SlimeEggTapped = false;
            SlimeButton.image.sprite = baseEgg;
        }
        
        id.BuildItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (pReference.slimeDied)
        {
            SlimeNamed = false;
            SlimeButton.image.sprite = baseEgg;
            pReference.slimeDied = false;
        }

        if (pReference.SlimeEggTapped && SlimeNamed && !pReference.SlimeHatched) timer -= Time.deltaTime;
        if (timer < 0)
        {
            SlimeButton.image.sprite = ssl[pReference.mySlime.slimeIteration - 1].SlimeStage[pReference.mySlime.getSlimeIndex()];
            EggHatched = true;
            pReference.SlimeHatched = EggHatched;
        }

        if (pReference.SlimeHatched == true)
        {
            atkStat.text = pReference.mySlime.getAtk().ToString();
            defStat.text = pReference.mySlime.getDef().ToString();
            spdStat.text = pReference.mySlime.getSpd().ToString();
        }
        else
        {
            atkStat.text = "??";
            defStat.text = "??";
            spdStat.text = "??";
        }

        
    }

    public void EnterButtonActive()
    {
        if (EnterBtn.isActiveAndEnabled == false)
            EnterBtn.gameObject.SetActive(true);
        else if (NameInput.text == "")
            EnterBtn.gameObject.SetActive(false);
    }

    public void SetSlimeName()
    {
        if (pReference.mySlime.getNme() != "") pReference.mySlime = new Slime(pReference.mySlime, NameInput.text, pReference.presetElem);
        else pReference.mySlime = new Slime(NameInput.text, pReference.presetElem);
        pReference.presetElem = -1;
        myCurrentIteration = pReference.mySlime.slimeIteration;
        SlimeButton.image.sprite = slimeEggImages[pReference.mySlime.getSlimeIndex()];
        myCurrentSlimeEvolution = pReference.mySlime.getSlimeEvolutionStage();
        NameCanvas.gameObject.SetActive(false);
        SlimeNamed = true;
    }

    //Tap Blank Egg to change into element egg which hatches into new Slime
    public void EggClicked()
    {
        if (pReference.SlimeEggTapped == false)
        {
            EggTapped = true;
            pReference.SlimeEggTapped = EggTapped;
            NameCanvas.gameObject.SetActive(true);
        }
    }

    

    public void Load(string scene)
    {
        if (pReference.SlimeHatched)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
