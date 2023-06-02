using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // union spwan timer 와 1:1 매칭
   
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
            // 전체 장착 슬롯의 하이라이트 끄기
            GlobalData.instance.unionManager.AllDisableEquipSlotHighlightEff();
        });
    }
    
    //장착 해제 



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
    }

    public void EquipUnion(UnionSlot _unionSlot)
    {
        if (unionSlot != null)
        {
            unionSlot.unionEquipType = EnumDefinition.UnionEquipType.Equipped;
            unionSlot.SetUITxtUnionEquipState();
            unionSlot.SetEquipSlot(this);
        }

        unionSlot = _unionSlot;
        SetUI();
    }

    public void UnEquipSlot()
    {
        unionSlot.unionEquipType = EnumDefinition.UnionEquipType.NotEquipped;
        unionSlot.SetEquipSlot(null);
        unionSlot = null;
        imgUnionFace.sprite = spriteUnionEmpty;
    }

    public void EnableEffHighlight(bool value)
    {
        objEquipHighlight.gameObject.SetActive(value);
    }
    
}
