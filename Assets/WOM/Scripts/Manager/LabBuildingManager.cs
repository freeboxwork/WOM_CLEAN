using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LabBuildingManager : MonoBehaviour
{
    public List<LabBuildIngameData> labBuildIngameDataList = new List<LabBuildIngameData>();
    public List<LabBuildSlot> labBuildSlotList = new List<LabBuildSlot>();

    void Start()
    {

    }


    public IEnumerator Init()
    {
        // set data ( load data )
        var data = GlobalData.instance.saveDataManager.saveDataTotal.saveDataLabBuildingData.labBuildIngameDatas;
        for (int i = 0; i < data.Count; i++)
        {
            var buildData = data[i].CloneInstance();
            labBuildIngameDataList.Add(buildData);
        }

        yield return new WaitForEndOfFrame();

        SetSlotUI();
        yield return null;
    }

    void SetSlotUI()
    {
        for (int i = 0; i < labBuildIngameDataList.Count; i++)
        {
            var data = labBuildIngameDataList[i];
            var slot = GetLabBuildSlot(data.goodsType);
            var maxLevel = GlobalData.instance.dataManager.labBuildingDatas.data.Max(x => x.level);
            var isMax = data.level >= maxLevel;
            slot.SetUI(data, isMax);
        }
    }

    public void LevelUpLabBuild(EnumDefinition.GoodsType goodsType)
    {
        var curData = GetInLabBuildGameData(goodsType);
        var maxLevel = GlobalData.instance.dataManager.labBuildingDatas.data.Max(x => x.level);
        // MAX CHECK
        if (curData.level >= maxLevel)
        {
            GlobalData.instance.globalPopupController.EnableGlobalPopup("", "최대 레벨입니다.");
        }
        else
        {
            // 금액 체크
            var curLevel = GlobalData.instance.dataManager.GetLabBuildingDataByLevel(curData.level);
            var price = curLevel.price;
            if (IsValidLevelUp(price))
            {
                // PAY COAL
                GlobalData.instance.player.PayCoal(price);
                var inGameData = GetInLabBuildGameData(goodsType);
                LevelUp(goodsType, ref inGameData);

                // is max level ? 
                var isMax = inGameData.level >= maxLevel;
                // Set UI
                var slot = GetLabBuildSlot(goodsType);
                slot.SetUI(inGameData, isMax);
            }
            else
            {
                // 재화 부족 팝업
                GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("", 22);
            }
        }
    }

    bool IsValidLevelUp(int price)
    {
        return GlobalData.instance.player.coal >= price;
    }

    public LabBuildIngameData GetInLabBuildGameData(EnumDefinition.GoodsType goodsType)
    {
        return labBuildIngameDataList.Where(x => x.goodsType == goodsType).FirstOrDefault();
    }

    void LevelUp(EnumDefinition.GoodsType goodsType, ref LabBuildIngameData data)
    {
        ++data.level;
        data.value = GlobalData.instance.dataManager.GetLabBuindingDataValueByGoodsType(data.level, goodsType);
        data.price = GlobalData.instance.dataManager.GetLabBuildingDataByLevel(data.level).price;

        // save data
        GlobalData.instance.saveDataManager.SaveDataLabBuildIngameData(data);
    }

    LabBuildSlot GetLabBuildSlot(EnumDefinition.GoodsType goodsType)
    {
        return labBuildSlotList.Where(x => x.goodsType == goodsType).FirstOrDefault();
    }

}
