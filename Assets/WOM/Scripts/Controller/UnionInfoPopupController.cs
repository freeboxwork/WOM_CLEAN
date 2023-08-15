using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnionInfoPopupController : MonoBehaviour
{
    public GameObject popup;
    public Image imgUnionFace;
    public TextMeshProUGUI txtUinonName;
    public TextMeshProUGUI txtUinonGrade;
    public TextMeshProUGUI txtDamage;
    public TextMeshProUGUI txtDamageAfter;
    public TextMeshProUGUI txtSpawnTime;
    public TextMeshProUGUI txtMoveSpeed;
    public TextMeshProUGUI txtPassiveDamage;
    public TextMeshProUGUI txtReqirementCount;
    public Slider slider;
    public Button btnEquip;
    public Button btnLevelUp;
    public Button btnClose;
    public UnionSlot unionSlot;




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
            popup.SetActive(false);
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
            popup.SetActive(false);
        });
    }
    public void EnablePopup(UnionSlot slot, UnionData data, UnionInGameData inGameData)
    {
        // SET UI
        SetImgFace(slot.imgUnionFace.sprite);
        SetTxtUnionName(data.name);
        Debug.Log(data.textColor + $"{data.gradeName}</color>");
        SetTxtUinonGrade($"<#{data.textColor}>{data.gradeName}</color>");


        unionSlot = slot;

        // SET STAT UI
        SetTxtDamage(inGameData.damage.ToString());
        SetTxtDamageAfter(inGameData.damageNextLevel.ToString());
        SetTxtSpawnTime(inGameData.spawnTime.ToString());
        SetTxtMoveSpeed(inGameData.moveSpeed.ToString());
        var passiveDamage = $"{inGameData.passiveDamage}% + {inGameData.passiveDamageNextLevel}";
        SetTxtPassiveDamage(passiveDamage);
        SetSlider(slot.sliderReqirement.value);
        SetTxtReqirementCount(slot.txtReqirementCount.text);

        popup.SetActive(true);
    }

    void ReloadUiSet()
    {
        SetTxtDamage(unionSlot.inGameData.damage.ToString());
        SetTxtDamageAfter(unionSlot.inGameData.damageNextLevel.ToString());
        SetTxtSpawnTime(unionSlot.inGameData.spawnTime.ToString());
        SetTxtMoveSpeed(unionSlot.inGameData.moveSpeed.ToString());
        var passiveDamage = $"{unionSlot.inGameData.passiveDamage}% + {unionSlot.inGameData.passiveDamageNextLevel}";
        SetTxtPassiveDamage(passiveDamage);
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
        txtDamageAfter.text = value;
    }

    public void SetTxtSpawnTime(string value)
    {
        txtSpawnTime.text = value;
    }

    public void SetTxtMoveSpeed(string value)
    {
        txtMoveSpeed.text = value;
    }

    public void SetTxtPassiveDamage(string value)
    {
        txtPassiveDamage.text = value + "%";
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


}
