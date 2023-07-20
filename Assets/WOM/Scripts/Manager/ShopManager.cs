using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class ShopManager : MonoBehaviour
{
    public List<ShopSlot> shopSlots = new List<ShopSlot>();
    LotteryPageType curLotteryPageType;
    void Start()
    {

    }


    public IEnumerator Init()
    {
        SetButtonEvents();

        // Set Shop Slot UI ( 필요시 사용 )
        // SetShopSlots(); 
        yield return null;
    }

    void SetShopSlots()
    {
        for (int i = 0; i < shopSlots.Count; i++)
        {
            var slot = shopSlots[i];

            // set ui
        }
    }



    private void SetButtonEvents()
    {
        // UNION 1 ( 34 ) -> gem 사용
        UtilityMethod.SetBtnEventCustomTypeByID(34, () =>
        {
            // 유니온 뽑기 버튼 셋트 활성화
            EnableLotteryBtnsSet(LotteryPageType.UNION);
            // 뽑기 1회 시작
            GlobalData.instance.evolutionManager.UnionLotteryGameStart(1, 10, EnumDefinition.RewardType.gem);

        });

        // UNION 11 -> gem 사용
        UtilityMethod.SetBtnEventCustomTypeByID(35, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.UNION);
            GlobalData.instance.evolutionManager.UnionLotteryGameStart(11, 100, EnumDefinition.RewardType.gem);
        });


        // UNION 1 -> unionTicket 사용
        UtilityMethod.SetBtnEventCustomTypeByID(70, () =>
        {
            Debug.Log("UNION 1 -> unionTicket 사용");
            EnableLotteryBtnsSet(LotteryPageType.UNION);
            GlobalData.instance.evolutionManager.UnionLotteryGameStart(1, 1, EnumDefinition.RewardType.unionTicket);
        });

        // UNION 11 -> unionTicket 사용
        UtilityMethod.SetBtnEventCustomTypeByID(71, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.UNION);
            GlobalData.instance.evolutionManager.UnionLotteryGameStart(11, 10, EnumDefinition.RewardType.unionTicket);
        });


        // DNA 1
        UtilityMethod.SetBtnEventCustomTypeByID(36, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.DNA);
            GlobalData.instance.dnaManger.DNALotteryGameStart(1, 10, EnumDefinition.RewardType.gem);
        });

        // DNA 11
        UtilityMethod.SetBtnEventCustomTypeByID(37, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.DNA);
            GlobalData.instance.dnaManger.DNALotteryGameStart(11, 100, EnumDefinition.RewardType.gem);
        });

        // DNA 1 -> dnaTicket 사용
        UtilityMethod.SetBtnEventCustomTypeByID(72, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.DNA);
            GlobalData.instance.dnaManger.DNALotteryGameStart(1, 1, EnumDefinition.RewardType.dnaTicket);
        });

        // DNA 11 -> dnaTicket 사용
        UtilityMethod.SetBtnEventCustomTypeByID(73, () =>
        {
            EnableLotteryBtnsSet(LotteryPageType.DNA);
            GlobalData.instance.dnaManger.DNALotteryGameStart(11, 10, EnumDefinition.RewardType.dnaTicket);
        });

        // FREE GEM 1
        UtilityMethod.SetBtnEventCustomTypeByID(38, () => { });

        // FREE GEM 10
        UtilityMethod.SetBtnEventCustomTypeByID(39, () => { });

        // 뽑기 페이지 닫기 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(44, () =>
        {
            //GlobalData.instance.uiController.EnableMenuPanel(MenuPanelType.shop);
            //UtilityMethod.GetCustomTypeGMById(12).gameObject.SetActive(true);
        });


    }

    ShopSlot GetShopSlotByType(ShopSlotType type)
    {
        return shopSlots.FirstOrDefault(f => f.shopSlotType == type);
    }

    void EnableLotteryBtnsSet(LotteryPageType lotteryPageType)
    {
        var unionPage = lotteryPageType == EnumDefinition.LotteryPageType.UNION;
        UtilityMethod.GetCustomTypeGMById(8).SetActive(unionPage);
        UtilityMethod.GetCustomTypeGMById(9).SetActive(!unionPage);
    }


}
