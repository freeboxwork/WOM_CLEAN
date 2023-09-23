using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DNASlot : MonoBehaviour
{
    public ParticleSystem effLevelUp;
    public EnumDefinition.DNAType DNAType;

    public Image imgDnaFace;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtMaxLevel;
    public TextMeshProUGUI txtInfo;
    public TextMeshProUGUI txtHasCount;
    public TextMeshProUGUI txtProbability;
    public TextMeshProUGUI txtLevel;

    public DNAInGameData inGameData;
    public Button upgradeBtn;


    public Sprite noHave;
    public Sprite have;

    DNAManager dnaManager;

    void Start()
    {
        upgradeBtn.onClick.AddListener(OnClickUpgradeBtn);
    }

    public void SetTxtName(string value)
    {
        txtName.text = value;
    }

    public void SetTxtMaxLevel(int vlaue)
    {
        txtMaxLevel.text = $"Max Lv : {vlaue}";
    }

    public void SetTxtInfo(string front, string color, double power, string back)
    {
        //txtInfo.text = $"{front} <{color}> {power:F2} </color> {back}";
       
        txtInfo.text = $"{front} <{color}> {(power % 1 == 0 ? power.ToString("0") : power.ToString("0.0"))} </color> {back}";
        
    }
    public void SetTxtLevel(int lv)
    {
        txtLevel.text = $"레벨 {lv}";
    }
    public void SetTxtHasCount(int level)
    {
        txtHasCount.text = $"보유량 {level}개";
    }
    public void SetTxtProbability(int value)
    {
        if(value == 0)
        {
            txtProbability.text = $"MAX";
            upgradeBtn.GetComponent<Image>().sprite = noHave;

            return;
        }

        txtProbability.text = $"{value}%";
    }
    public void SetFace(Sprite sprite)
    {
        imgDnaFace.sprite = sprite;
    }
    public void PlayEffect()
    {
        GlobalData.instance.effectManager.PlayEffect();

        effLevelUp.Play();
    }
    public void SetDnaType(EnumDefinition.DNAType type)
    {
        DNAType = type;
    }

    public void OnClickUpgradeBtn()
    {
        dnaManager.UpgradeDNA(DNAType);
    }
    public void UpdateHaveDNACount(bool isHave)
    {
        if (isHave)
        {
            upgradeBtn.GetComponent<Image>().sprite = have;
        }
        else
        {
            upgradeBtn.GetComponent<Image>().sprite = noHave;
        }
    }

    public void Init(DNAManager manager)
    {
        dnaManager = manager;
    }

}
