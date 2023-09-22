using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class ShopManager : MonoBehaviour
{
    public List<ShopKeyProductSlot> shopKeyProductSlots = new List<ShopKeyProductSlot>();

    int lotteryCost1 = 100;    //유니온 1회 뽑기 비용
    int lotteryCost11 = 1000;    //유니온 10회 뽑기 비용
    int lotteryCost33 = 3000;    //유니온 30회 뽑기 비용
    int lotteryCostTicket1 = 1;    //티켓 1회 뽑기 비용
    int lotteryCostTicket10 = 10;//티켓 10회 뽑기 비용
    int lotteryCount30 = 33;    //10회당 뽑을 수 있는 횟수

    int lotteryCount10 = 11;    //10회당 뽑을 수 있는 횟수
    int lotteryCount1 = 1;    //1회당 뽑을 수 있는 횟수


    public IEnumerator Init()
    {
        SetButtonEvents();
        SetKeyProductSlots();

        // Set Shop Slot UI ( 필요시 사용 )
        // SetShopSlots(); 
        yield return null;
    }

    void SetKeyProductSlots()
    {
        for (int i = 0; i < shopKeyProductSlots.Count; i++)
        {
            var slot = shopKeyProductSlots[i];

            // load data
            slot.LoadData();
        }
    }

    public void ResetBuyKeyCount()
    {
        for (int i = 0; i < shopKeyProductSlots.Count; i++)
        {
            var slot = shopKeyProductSlots[i];

            slot.ResetKeyCount();

        }

    }


    private void SetButtonEvents()
    {
        #region  유니온 뽑기
        // UNION 1개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(34, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost1) == false) return;

            GlobalData.instance.evolutionManager.UnionLotteryGameStart(lotteryCount1, lotteryCost1, EnumDefinition.RewardType.gem);

        });

        // UNION 11개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(35, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost11) == false) return;

            GlobalData.instance.evolutionManager.UnionLotteryGameStart(lotteryCount10, lotteryCost11, EnumDefinition.RewardType.gem);
        });
        // UNION 33개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(74, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost33) == false) return;

            GlobalData.instance.evolutionManager.UnionLotteryGameStart(lotteryCount30, lotteryCost33, EnumDefinition.RewardType.gem);
        });

        // UNION 1개 티켓을 사용하여 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(70, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.unionTicket, lotteryCostTicket1) == false) return;

            GlobalData.instance.evolutionManager.UnionLotteryGameStart(lotteryCount1, lotteryCostTicket1, EnumDefinition.RewardType.unionTicket);
        });

        // UNION 11개 티켓을 사용하여 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(71, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.unionTicket, lotteryCostTicket10) == false) return;

            GlobalData.instance.evolutionManager.UnionLotteryGameStart(lotteryCount10, lotteryCostTicket10, EnumDefinition.RewardType.unionTicket);
        });
#endregion

        #region DNA 뽑기
        // DNA 1개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(36, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost1) == false) return;
            //GlobalData.instance.player.PayGem(lotteryCost1);

            GlobalData.instance.dnaManger.DNALotteryGameStart(lotteryCount1, lotteryCost1, EnumDefinition.RewardType.gem);
        });

        // DNA 11개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(37, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost11) == false) return;
            //GlobalData.instance.player.PayGem(lotteryCost11);

            GlobalData.instance.dnaManger.DNALotteryGameStart(lotteryCount10, lotteryCost11, EnumDefinition.RewardType.gem);
        });
        // DNA 33개 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(75, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.gem, lotteryCost33) == false) return;
            //GlobalData.instance.player.PayGem(lotteryCost11);

            GlobalData.instance.dnaManger.DNALotteryGameStart(lotteryCount30, lotteryCost33, EnumDefinition.RewardType.gem);
        });


        // DNA 1개 티켓을 사용하여 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(72, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.dnaTicket, lotteryCostTicket1) == false) return;
            //GlobalData.instance.player.PayDnaTicket(lotteryCostTicket1);

            GlobalData.instance.dnaManger.DNALotteryGameStart(lotteryCount1, lotteryCostTicket1, EnumDefinition.RewardType.dnaTicket);
        });

        // DNA 11개 티켓을 사용하여 뽑기
        UtilityMethod.SetBtnEventCustomTypeByID(73, () =>
        {
            if (GlobalData.instance.player.IsEnoughRewardGoods(RewardType.dnaTicket, lotteryCostTicket10) == false) return;
            //GlobalData.instance.player.PayDnaTicket(lotteryCostTicket10);

            GlobalData.instance.dnaManger.DNALotteryGameStart(lotteryCount10, lotteryCostTicket10, EnumDefinition.RewardType.dnaTicket);
        });
        #endregion
        



    }


}
