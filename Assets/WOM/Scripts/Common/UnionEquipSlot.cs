using UnityEngine;
using UnityEngine.UI;

public class UnionEquipSlot : MonoBehaviour
{
    public int slotIndex = 0;
    public Image imgFrame;
    public Image imgUnionFace;
    public Image imgUnlock;
    public Sprite spriteUnionEmpty;
    public Button btnSlot;
    public bool isUnLock = false;
    public UnionSlot unionSlot;
    public GameObject objEquipHighlight;


    // union spwan timer ??? 1:1 Îß§Ïπ≠

    void Start()
    {
        SetBtnEvent();
        SetBtnEnableState(false);
    }

    void SetBtnEvent()
    {
        btnSlot.onClick.AddListener(() =>
        {
            GlobalData.instance.unionManager.EquipSlot(this);
            // ?†ÑÏ≤? ?û•Ï∞? ?ä¨Î°??ùò ?ïò?ù¥?ùº?ù¥?ä∏ ?ÅÑÍ∏?
            GlobalData.instance.unionManager.AllDisableEquipSlotHighlightEff();
        });
    }

    //?û•Ï∞? ?ï¥?†ú 


    public void SetBtnEnableState(bool value)
    {
        btnSlot.enabled = value;
    }

    public void UnLockSlot()
    {
        isUnLock = true;
        imgUnlock.gameObject.SetActive(false);
    }

    public void SetUI()
    {
        imgUnionFace.sprite = unionSlot.imgUnionFace.sprite;
        unionSlot.unionEquipType = EnumDefinition.UnionEquipType.Equipped;
        unionSlot.SetUITxtUnionEquipState();
        //ΩΩ∑‘ «¡∑π¿” ¿Ø¥œø¬ µÓ±ﬁø° ∏¬∞‘ ∫Ø∞Ê
        imgFrame.sprite = GlobalData.instance.spriteDataManager.GetUIIcon(unionSlot.unionGradeType.ToString());
    }
    void SetEquitSlot()
    {
        unionSlot.SetEquipSlot(this);
    }

    public void EquipUnion(UnionSlot _unionSlot)
    {
        if (unionSlot != null)
        {
            unionSlot.unionEquipType = EnumDefinition.UnionEquipType.NotEquipped;
            unionSlot.SetUITxtUnionEquipState();
            unionSlot.SetEquipSlot(null);
        }

        unionSlot = _unionSlot;
        SetUI();
        SetEquitSlot();

    }

    public void UnEquipSlot()
    {
        unionSlot.unionEquipType = EnumDefinition.UnionEquipType.NotEquipped;
        unionSlot.SetUITxtUnionEquipState();
        unionSlot.SetEquipSlot(null);
        unionSlot = null;
        imgUnionFace.sprite = spriteUnionEmpty;
        imgFrame.sprite = GlobalData.instance.spriteDataManager.GetUIIcon("defaultFrame");
    }

    public void EnableEffHighlight(bool value)
    {
        objEquipHighlight.gameObject.SetActive(value);
    }

}
