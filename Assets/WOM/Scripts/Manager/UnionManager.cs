using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnionManager : MonoBehaviour
{
    public List<UnionSlot> unionSlots = new List<UnionSlot>();
    public List<UnionEquipSlot> unionEquipSlots = new List<UnionEquipSlot>();
    UnionSlot selectedSlot;
    public SpriteFileData spriteFileData;

    void Start()
    {
        // UnlockEquipSlots(GlobalData.instance.evolutionManager.evalutionLeveldx);
    }

    public void UnlockEquipSlots(int dataId)
    {
        var data = GlobalData.instance.dataManager.GetRewaedEvolutionGradeDataByID(dataId);

        // slot count 만큼 슬롯 열어줌
        for (int i = 0; i < data.slotCount; i++)
        {
            unionEquipSlots[i].UnLockSlot();
        }

    }

    // void Update()
    // {
    //     //만약에 키보드 q 키를 누르면
    //     if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         Debug.Log("add union");
    //         AddUnion(16);
    //     }
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         Debug.Log("add union");
    //         AddUnion(24);
    //     }
    // }

    // set slot data
    public IEnumerator Init()
    {
        var unionDatas = GlobalData.instance.dataManager.unionDatas.data;

        for (int i = 0; i < unionDatas.Count; i++)
        {
            int index = i;
            var data = unionDatas[index];
            var slot = unionSlots[index];

            slot.inGameData = new UnionInGameData();

            // set id
            slot.inGameData.unionIndex = data.unionIndex;
            // set type
            slot.unionGradeType = (EnumDefinition.UnionGradeType)System.Enum.Parse(typeof(EnumDefinition.UnionGradeType), data.gradeType);
            // set face image
            slot.SetUIImageUnion(spriteFileData.GetIconData(data.unionIndex));
            // set data
            slot.unionData = data;


            // TODO: 저장된 데이터에서 불러와야 함

            var saveData = GlobalData.instance.saveDataManager.GetSaveDataUnion(slot);


            /*
            // set level 
            slot.inGameData.level = 0;

            // set reqirement count
            slot.inGameData.LevelUpReqirementCount = data.reqirementCount;

            // set equip type
            slot.unionEquipType = EnumDefinition.UnionEquipType.NotEquipped;
            */

            // set level 
            slot.inGameData.level = saveData.level;

            // set union count
            slot.inGameData.unionCount = saveData.unionCount;

            // set equip type
            var equipType = saveData.isEquip == true ? EnumDefinition.UnionEquipType.Equipped : EnumDefinition.UnionEquipType.NotEquipped;

            slot.unionEquipType = equipType;

            // set reqirement count
            //slot.inGameData.LevelUpReqirementCount =   data.reqirementCount;

            slot.inGameData.LevelUpReqirementCount = GetUnionReqireCount(slot);




            // set slider value
            slot.SetSliderValue();

            // set ui
            slot.SetUITxtLevel();
            slot.SetUITxtUnionCount();
            slot.SetUITxtUnionEquipState();
            slot.SetUITxtGrade();
            if (slot.inGameData.unionCount > 0 || slot.inGameData.level > 0)
                slot.EnableSlot();

            // equip slot
            if (saveData.isEquip)
            {
                selectedSlot = slot;
                EquipSlot(unionEquipSlots[saveData.equipSlotId]);
            }

            // set BtnAction
            slot.btn.onClick.AddListener(() =>
            {
                GlobalData.instance.unionInfoPopupController.EnablePopup(slot, data, slot.inGameData);
            });


            // TODO: 저장된 데이터에서 불러와야 함
            // SET IN GAME DATA (  ) 
            slot.inGameData.damage = GetUnionDamage(slot); // data.damage;
            slot.inGameData.damageNextLevel = GetUnionDamageNextLevel(slot);
            slot.inGameData.spawnTime = data.spawnTime;
            slot.inGameData.moveSpeed = data.moveSpeed;
            slot.inGameData.passiveDamage = GetUnionPassiveDamage(slot); //data.passiveDamage;
            slot.inGameData.passiveDamageNextLevel = GetUnionPassiveDamageNextLevel(slot);
            slot.inGameData.unionGradeType = (EnumDefinition.UnionGradeType)System.Enum.Parse(typeof(EnumDefinition.UnionGradeType), data.gradeType);
            //Debug.Log(slot.inGameData.passiveDamage);

            slot.SetStarUI();

            yield return null;
        }

        // 전체 강화 버튼 활성, 비활성화
        EnableBtnTotalLevelUp();

        // 전체 추가 공격력 텍스트 UI 셋팅
        SetTxtTotalPassiveDamage();

        // 버튼 이벤트 설정
        SetBtnEvent();

        UnlockEquipSlots(GlobalData.instance.evolutionManager.evalutionLeveldx);

    }

    void SetBtnEvent()
    {
        UtilityMethod.SetBtnEventCustomTypeByID(7, () =>
        {
            TotlaUnionLevelUp();
        });
    }

    public void EnableEquipSlotBtns()
    {
        var data = GlobalData.instance.dataManager.GetRewaedEvolutionGradeDataByID(GlobalData.instance.evolutionManager.evalutionLeveldx);
        // slot count 만큼 슬롯 열어줌
        for (int i = 0; i < data.slotCount; i++)
        {
            unionEquipSlots[i].SetBtnEnableState(true);
            unionEquipSlots[i].EnableEffHighlight(true);
        }
    }

    // 유니온 장착
    public void EquipSlot(UnionEquipSlot equipSlot)
    {
        // 장착 슬롯에 이미 같은 유니온이 포함 되어 있는지 판단하여 이미 같은 유니온이 장착 되어 있다면 해제 한다.
        if (IsEquipedSlotBySelectSlot())
            UnEquipSameUnion();

        // 유니온 장착
        equipSlot.EquipUnion(selectedSlot);

        // 장착후 버튼 활성화 해제
        unionEquipSlots.ForEach(f => f.SetBtnEnableState(false));

        // Union Spwan
        GlobalData.instance.unionSpwanManager.UnionSpwan(selectedSlot, equipSlot.slotIndex);

        // // 일일 퀘스트 완료 : 유니온 소환
        // // EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.summonUnion);

    }


    bool IsEquipedSlotBySelectSlot()
    {
        return unionEquipSlots.Any(a => a.unionSlot == selectedSlot);
    }

    void UnEquipSameUnion()
    {
        foreach (var equipSlot in unionEquipSlots)
        {
            if (equipSlot.unionSlot == selectedSlot)
            {
                equipSlot.UnEquipSlot();

                // 스폰 타이머 해제
                var timarIndex = equipSlot.slotIndex;
                var timer = GlobalData.instance.unionSpwanManager.spwanTimerList[timarIndex];
                timer.TimerStop();
            }
        }

    }


    public void SetSelectedSlot(UnionSlot slot)
    {
        selectedSlot = slot;
    }

    public UnionSlot GetSelectedSlotData()
    {
        return selectedSlot;
    }


    UnionSlot GetUnionSlotByID(int id)
    {
        return unionSlots.FirstOrDefault(f => f.inGameData.unionIndex == id);
    }

    public UnionInGameData GetUnionInGameDataByID(int id)
    {
        return unionSlots.FirstOrDefault(f => f.inGameData.unionIndex == id).inGameData;
    }

    public void AddUnion(long unionId)
    {
        //var data = GlobalData.instance.dataManager.GetUnionDataByIdx(unionId);
        var slot = GetUnionSlotByID((int)unionId);

        // 유니온 최대 레벨 계산
        // var currentCount = slot.inGameData.unionCount;
        // var maxCount = data.maxLevel;
        // if (currentCount >= maxCount)
        // {
        //     Debug.Log("유니온 최대 레벨");
        //     return;
        // }

        if (slot.inGameData.isUnlock == false)
        {
            slot.EnableSlot();
        }

        // set slot data
        slot.AddUnion(1);
        slot.SetUITxtUnionCount();
        slot.SetSliderValue();
    }

    public void AddUnions(List<int> indexList)
    {
        for (int i = 0; i < indexList.Count; i++)
        {
            var id = indexList[i];
            AddUnion(id);
        }

        SetTxtTotalPassiveDamage();
        // 전체 강화 버튼 활성, 비활성화
        EnableBtnTotalLevelUp();
    }

    public double GetUnionDamage(UnionSlot slot)
    {
        var damage = slot.unionData.damage + (slot.inGameData.level * slot.unionData.addDamage);
        return damage;
    }
    public double GetUnionDamageNextLevel(UnionSlot slot)
    {
        var nextLevel = slot.inGameData.level + 1;
        var maxLevel = slot.unionData.maxLevel;
        // max level 체크
        if (nextLevel > maxLevel)
            return 0;

        var damage = slot.unionData.damage + ((nextLevel) * slot.unionData.addDamage);
        return damage;
    }

    public float GetUnionPassiveDamage(UnionSlot slot)
    {
        var passiveDamage = slot.unionData.passiveDamage + (slot.inGameData.level * slot.unionData.addPassiveDamage);
        return passiveDamage;
    }

    public float GetUnionPassiveDamageNextLevel(UnionSlot slot)
    {
        var nextLevel = slot.inGameData.level + 1;
        var maxLevel = slot.unionData.maxLevel;
        // max level 체크
        if (nextLevel > maxLevel)
            return 0;

        //var passiveDamage = slot.unionData.passiveDamage + (nextLevel * slot.unionData.addPassiveDamage);
        var passiveDamage = slot.unionData.addPassiveDamage;
        return passiveDamage;
    }

    public int GetUnionReqireCount(UnionSlot slot)
    {
        var reqieCount = slot.unionData.reqirementCount + (slot.inGameData.level * slot.unionData.addReqirementCount);
//        Debug.Log("필요수 : " + reqieCount);

        return reqieCount;
    }

    public bool IsValidLevelUpCount(UnionSlot slot)
    {
//        Debug.Log("현재보유수 : " + slot.inGameData.unionCount);
        return slot.inGameData.unionCount >= GetUnionReqireCount(slot);
    }

    public bool LevelUpUnion(UnionSlot slot)
    {
        var isValidLevelUp = IsValidLevelUpCount(slot);

        if (isValidLevelUp)
        {
            var cost = GetUnionReqireCount(slot);
            slot.PayUnion(cost);
            slot.LevelUp();
            slot.inGameData.LevelUpReqirementCount = GetUnionReqireCount(slot);
            slot.inGameData.damage = GetUnionDamage(slot);
            slot.inGameData.damageNextLevel = GetUnionDamageNextLevel(slot);
            slot.inGameData.passiveDamage = GetUnionPassiveDamage(slot);
            slot.inGameData.passiveDamageNextLevel = GetUnionPassiveDamageNextLevel(slot);
            slot.RelodUISet();

            EnableBtnTotalLevelUp();
            SetTxtTotalPassiveDamage();
        }
        else
        {
            Debug.Log($"레벨업에 필요한 유니온이 부족합니다. {slot.unionData.name} _ {slot.inGameData.unionCount}");
            // 유니온 부족 팝업
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("", 20);
        }
        return isValidLevelUp;
    }

    // 전체 유니온 레벨업
    void TotlaUnionLevelUp()
    {
        for (int i = 0; i < unionSlots.Count; i++)
        {
            var slot = unionSlots[i]; 
            while (IsValidLevelUpCount(slot))
            {
                LevelUpUnion(slot);
            }
            //if (IsValidLevelUpCount(slot))
            //    LevelUpUnion(slot);
        }
    }
    
    public float GetAllUnionPassiveDamage()
    {
        float totalPassiveValue = 0f;

        for(int i = 0; i < unionSlots.Count; i++)
        {
            if (unionSlots[i].inGameData.unionCount > 0 || unionSlots[i].inGameData.level > 0)
            {
                totalPassiveValue += unionSlots[i].inGameData.passiveDamage;
            }
        }
        return totalPassiveValue;
        //Debug.Log("totalPassiveValue : "+totalPassiveValue);
    }
    public void SetTxtTotalPassiveDamage()
    {
        var txtValue = $"전체 추가 공격력 : {GetAllUnionPassiveDamage()}%";
        UtilityMethod.SetTxtCustomTypeByID(80, txtValue);
    }
    /// <summary> 유니온 슬롯중 강화가 가능한 슬롯이 하나라도 있다면 전체 강화 버튼 활성화 없다면 비활성화 </summary>
    public void EnableBtnTotalLevelUp()
    {
        UtilityMethod.GetCustomTypeBtnByID(7).interactable = IsTotalLevelUp();
    }
    bool IsTotalLevelUp()
    {
        return unionSlots.Any(a => IsValidLevelUpCount(a));
    }
    public void AllDisableEquipSlotHighlightEff()
    {
        foreach (var equipSlot in unionEquipSlots)
        {
            equipSlot.EnableEffHighlight(false);
        }
    }


}
