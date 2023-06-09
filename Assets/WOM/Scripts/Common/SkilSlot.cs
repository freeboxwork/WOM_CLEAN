using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SkilSlot : MonoBehaviour
{
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI txtMaxLevel;
    public TextMeshProUGUI txtCost;
    public Button btnPay;
    public EnumDefinition.SkillType skillType;
    public ParticleSystem effLevelUp;

    public GameObject infoCommon;
    public GameObject infoMax;

    void Start()
    {
        SetBtnEvent();
    }


    void SetBtnEvent()
    {
        btnPay.onClick.AddListener(() =>
        {
            GlobalData.instance.skillManager.LevelUpSkill(skillType);
            effLevelUp.Play();
        });
    }

    public void SetTxt_Level(string value)
    {
        txtLevel.text = value;
    }

    public void SetTxt_Name(string value)
    {
        txtName.text = value;
    }

    public void SetTxt_Description(string value)
    {
        txtDescription.text = value;
    }

    public void SetTxt_MaxLevel(string value)
    {
        //txtMaxLevel.text = value;
        txtMaxLevel.text = "";
    }

    public void SetTxt_Level_SkillName(string value)
    {
        txtLevel.text = value;
    }

    public void SetTxt_Cost(string value)
    {
        var txtSymbolValue = UtilityMethod.ChangeSymbolNumber(float.Parse(value));
        txtCost.text = txtSymbolValue;
    }

    // 초기 데이터 세팅
    //public void SetUIBySkillData(int currentLevel,SkillData data)
    //{
    //    SetTxt_Level(data.)
    //}

    public void MaxStat()
    {
        infoCommon.SetActive(false);
        infoMax.SetActive(true);
    }

}
