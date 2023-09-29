using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnionInfoPopupController : MonoBehaviour
{
    public Image imgUnionFace;
    public TextMeshProUGUI txtUinonName;
    public TextMeshProUGUI txtUinonGrade;
    public TextMeshProUGUI txtDamage;
    public TextMeshProUGUI txtDamageAfter;
    public TextMeshProUGUI txtSpawnTime;
    public TextMeshProUGUI txtMoveSpeed;
    public TextMeshProUGUI txtPassiveDamage;
    public TextMeshProUGUI txtCurrentPassiveDamage;
    public TextMeshProUGUI txtReqirementCount;
    public Slider slider;
    public Button btnEquip;
    public Button btnLevelUp;
    public Button btnClose;
    public UnionSlot unionSlot;

    public EnumDefinition.CanvasGroupTYPE canvasGroupType;

    public GameObject haveUnionBottomPanel;
    public GameObject noHaveUnionBottomPanel;


    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnEquip.onClick.AddListener(() =>
        {
            GlobalData.instance.unionManager.SetSelectedSlot(unionSlot);
            GlobalData.instance.unionManager.EnableEquipSlotBtns();
            HidePopup();
        });

        btnLevelUp.onClick.AddListener(() =>
        {

            if (GlobalData.instance.unionManager.LevelUpUnion(unionSlot))
            {
                ReloadUiSet();
            }
        });

        btnClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    public void EnablePopup(UnionSlot slot, UnionData data, UnionInGameData inGameData)
    {
        
        //현재 유니온의 레벨도 0이고 보유량도 0 인 경우 (유니온을 보유하고 있지 않은 경우)
        if(inGameData.unionCount <= 0 && inGameData.level <= 0)
        {
            haveUnionBottomPanel.SetActive(false);
            noHaveUnionBottomPanel.SetActive(true);
        }
        else
        {
            haveUnionBottomPanel.SetActive(true);
            noHaveUnionBottomPanel.SetActive(false);
        }



        // SET UI
        SetImgFace(slot.imgUnionFace.sprite);
        SetTxtUnionName(data.name);
        //Debug.Log(data.textColor + $"{data.gradeName}</color>");
        SetTxtUinonGrade($"<#{data.textColor}>{data.gradeName}</color>");


        unionSlot = slot;

        // SET STAT UI
        SetTxtDamage(UtilityMethod.ChangeSymbolNumber(inGameData.damage));
        SetTxtDamageAfter(UtilityMethod.ChangeSymbolNumber(inGameData.damageNextLevel));
        SetTxtSpawnTime(inGameData.spawnTime.ToString());
        SetTxtMoveSpeed(inGameData.moveSpeed.ToString());
        //var passiveDamage = $"{inGameData.passiveDamage}% (+ {inGameData.passiveDamageNextLevel})";
        SetTxtCurrentPassiveDamage(inGameData.passiveDamage.ToString());
        SetTxtPassiveDamage(inGameData.passiveDamageNextLevel.ToString());
        SetSlider(slot.sliderReqirement.value);
        SetTxtReqirementCount(slot.txtReqirementCount.text);

        ShowPopup();
    }

    void ReloadUiSet()
    {
        SetTxtDamage(UtilityMethod.ChangeSymbolNumber(unionSlot.inGameData.damage));
        SetTxtDamageAfter(UtilityMethod.ChangeSymbolNumber(unionSlot.inGameData.damageNextLevel));
        SetTxtSpawnTime(unionSlot.inGameData.spawnTime.ToString());
        SetTxtMoveSpeed(unionSlot.inGameData.moveSpeed.ToString());
        //var passiveDamage = $"{unionSlot.inGameData.passiveDamage}% (+ {unionSlot.inGameData.passiveDamageNextLevel})";
        SetTxtCurrentPassiveDamage(unionSlot.inGameData.passiveDamage.ToString());
        SetTxtPassiveDamage(unionSlot.inGameData.passiveDamageNextLevel.ToString());
        SetSlider(unionSlot.sliderReqirement.value);
        SetTxtReqirementCount(unionSlot.txtReqirementCount.text);
    }

    public void SetTxtUnionName(string value)
    {
        txtUinonName.text = value;
    }

    public void SetTxtUinonGrade(string value)
    {
        txtUinonGrade.text = value;
    }

    public void SetTxtDamage(string value)
    {
        txtDamage.text = value;
    }

    public void SetTxtDamageAfter(string value)
    {
        txtDamageAfter.text = $"(+ {value})";
    }

    public void SetTxtSpawnTime(string value)
    {
        txtSpawnTime.text = $"{value} s";
    }

    public void SetTxtMoveSpeed(string value)
    {
        txtMoveSpeed.text =  value;
    }
    public void SetTxtCurrentPassiveDamage(string value)
    {
        txtCurrentPassiveDamage.text =  $"{value} %";
    }
    public void SetTxtPassiveDamage(string value)
    {
        txtPassiveDamage.text =  $"(+ {value} %)";
    }

    public void SetTxtReqirementCount(string value)
    {
        txtReqirementCount.text = value;
    }

    public void SetImgFace(Sprite sprite)
    {
        imgUnionFace.sprite = sprite;
    }

    public void SetSlider(float value)
    {
        slider.value = value;
    }

    public void ShowPopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, true);
    }
    public void HidePopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, false);
    }


}
