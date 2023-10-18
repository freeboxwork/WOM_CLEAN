using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class TraningManager : MonoBehaviour
{
    public List<TraningSlot> traningSlots = new List<TraningSlot>();


    [SerializeField]
    List<GameObject> subPanels = new List<GameObject>();

     // 저장된 게임 데이터 로드 및 세팅
    IEnumerator LoadInGameData()
    {

        yield return null;
        int id = 0;
        foreach (SaleStatType statType in Enum.GetValues(typeof(SaleStatType)))
        {
            var slot = traningSlots[id];
            var elementData = GlobalData.instance.dataManager.GetTrainingElementData(statType);

            slot.traningInGameData = new TraningInGameData();
            slot.statType = statType;
            slot.goodsType = (GoodsType)Enum.Parse(typeof(GoodsType), elementData.goodsType);

            //저장된 데이터에서 불러옴 -> Save Data Manager 접근
            var saveData = GlobalData.instance.saveDataManager.GetTraningData(statType);

            slot.traningInGameData.level = saveData.level;

            // Get Value From data
            var data = GlobalData.instance.dataManager.GetSaleStatDataByTypeId(statType, slot.traningInGameData.level);

            slot.traningInGameData.value = data.value;
            slot.traningInGameData.unitName = data.unitName;
            slot.traningInGameData.trainingName = elementData.trainingName;
            slot.traningInGameData.description = elementData.description;

            id++;
        }
    }

    public void SetCustomLevel(EnumDefinition.SaleStatType statType, int level)
    {
        SetInGameStatLevel(statType, level);
        SetInGameStatValue(statType, level);
        SetUI_TraningSlot(statType);
    }


    public IEnumerator Init()
    {
        // Load In Game Data  저장된 게임 데이터 로드 및 세팅
        yield return StartCoroutine(LoadInGameData());

        // 구매 버튼 이벤트 세팅
        yield return StartCoroutine(SetBtnEvent());

        foreach (SaleStatType statType in Enum.GetValues(typeof(SaleStatType)))
        {

            SetUI_TraningSlot(statType);

            yield return null;

        }

        EnableBuyButtons();
    }

    IEnumerator SetBtnEvent()
    {
        foreach (var slot in traningSlots)
        {
            slot.btnBuy.onClick.AddListener(() =>
            {

                //Debug.Log("stat " + slot.statType);
                GlobalData.instance.saleManager.AddData(new SaleStatMsgData(slot.statType));

            });
        }

        yield return null;
    }

    float GetCostIntValue(SaleStatType statType, TraningInGameData inGameData)
    {
        var data = GlobalData.instance.dataManager.GetSaleStatDataByType(statType).data;
        if (data.Last().level > inGameData.level)
        {
            return GlobalData.instance.dataManager.GetSaleStatDataByTypeId(statType, inGameData.level + 1).salePrice;
        }
        else
        {
            return 0;
        }
    }
    float GetCostStrValue(SaleStatType statType, TraningInGameData inGameData)
    {
        var data = GlobalData.instance.dataManager.GetSaleStatDataByType(statType).data;
        if (data.Last().level > inGameData.level)
        {
            return GlobalData.instance.dataManager.GetSaleStatDataByTypeId(statType, inGameData.level + 1).salePrice;
        }
        else
        {
            return -1;
        }
    }



    public void SetUI_TraningSlot(SaleStatType statType)
    {
        var inGameData = GetTraningInGameData(statType);
        var slot = GetTraningInSlotByType(statType);

        // INFO TEXT
        var txtInfoValue = $"Lv{inGameData.level} {inGameData.trainingName}";
        slot.SetTxtInfo(txtInfoValue);

        // COST TEXT
        var txtCostValue = GetCostStrValue(statType, inGameData);
        slot.SetTxtCost(txtCostValue);

        // POWER TEXT
        var txtPower = "";

        if (statType == SaleStatType.talentGoldBonus || statType == SaleStatType.trainingDamage)
        {
            // 정수
            txtPower = Mathf.Round(inGameData.value).ToString();
        }
        else
        {
            // 소수점 둘째자리
            txtPower = UtilityMethod.FormatDoubleToOneDecimal(inGameData.value);
        }

        var txtPowerValue = string.Format("{0}{1}",txtPower,inGameData.unitName);

        var de = inGameData.description.Replace("<Power>", $"<#40ff80>{txtPowerValue}</color>");

        slot.SetTxtPower(de);


        //slot.SetDescription("");



        // max stat
        var lastData = GlobalData.instance.dataManager.GetSaleStatDataByType(statType).data.Last();
        
        if (inGameData.level == lastData.level)
        {
            SetUI_Max(statType);
        }
    }

    public void SetUI_Max(SaleStatType statType)
    {
        var slot = GetTraningInSlotByType(statType);
        slot.MaxStat();
    }



    public TraningSlot GetTraningInSlotByType(SaleStatType statType)
    {
        return traningSlots.FirstOrDefault(f => f.statType == statType);
    }

    public TraningInGameData GetTraningInGameData(SaleStatType statType)
    {
        return traningSlots.FirstOrDefault(f => f.statType == statType).traningInGameData;
    }

    public int GetInGameStatLevel(SaleStatType statType)
    {
        return GetTraningInGameData(statType).level;
    }

    public void SetInGameStatLevel(SaleStatType statType, int level)
    {
        GetTraningInGameData(statType).level = level;
    }

    public double GetInGameStatValue(SaleStatType statType)
    {
        return GetTraningInGameData(statType).value;
    }

    public void SetInGameStatValue(SaleStatType statType, float value)
    {
        GetTraningInGameData(statType).value = value;
    }

    public double GetStatPower(SaleStatType statType)
    {
        return GetTraningInGameData(statType).value;
    }

    /// <summary> 골드 , 뼈조각 획득 하거나 구매 했을때 버튼 상태 활성, 비활성화  </summary>
    public void EnableBuyButtons()
    {
        StartCoroutine(EnableBuyButtons_Cor());
    }

    IEnumerator EnableBuyButtons_Cor()
    {

        yield return new WaitForEndOfFrame();

        // 보유 금액으로 구매 가능한지 확인
        foreach (var slot in traningSlots)
        {
            var inGameData = GetTraningInGameData(slot.statType);
            var enableValue = IsValidPayItem(GetCostIntValue(slot.statType, inGameData), slot.goodsType);
            slot.BtnEnable(enableValue);
        }
    }

    // 구매 가능한지 확인
    bool IsValidPayItem(float price, GoodsType goodsType)
    {
        switch (goodsType)
        {
            case GoodsType.gold: return GlobalData.instance.player.gold >= price;
            case GoodsType.bone: return GlobalData.instance.player.bone >= price;
        }
        return false;
    }


}
