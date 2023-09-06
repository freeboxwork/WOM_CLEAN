using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Windows;
using UnityEngine.UI;

public enum ProductTYPE
{

    commongem1,
    commongem2,
    commongem3,
    commongem4,
    vipgem1,
    vipgem2,
    vipgem3,
    vipgem4,
    starterrpackage,
    adbuffpass,
    dungeonpackage,
    battlepass,
    fastestpackage

}

public class InAppPurchaseManager : MonoBehaviour
{
    public class MyStoreProducts
    {
        public ShopProductNames productName;
        public bool isBuy;

        public MyStoreProducts(ShopProductNames name, bool buy)
        {
            productName = name;
            isBuy = buy;
        }
    }
    public GameObject[] productGameObject;
    private List<MyStoreProducts> consumableProducts = new List<MyStoreProducts>();
    private List<MyStoreProducts> nonConsumableProducts = new List<MyStoreProducts>();


    int commonGem1 = 1000;
    int commonGem2 = 5500;
    int commonGem3 = 12000;
    int commonGem4 = 39000;
    int vipGem1 = 2000;
    int vipGem2 = 11000;
    int vipGem3 = 24000;
    int vipGem4 = 78000;


    //bool initializationInProgress = false;
    public IEnumerator Init()
    {
        if (!IAPManager.Instance.IsInitialized())
        {
            IAPManager.Instance.InitializeIAPManager(InitializeResult);
        }
        CheckBuyNonConsuableProduct();
        IAPManager.Instance.RestorePurchases(ProductBought, RestoreDone);
        yield return null;
    }

    void CheckBuyNonConsuableProduct()
    {   
        if (nonConsumableProducts.Count > 0)
        {
            for (int i = 0; i < nonConsumableProducts.Count; i++)
            {
                if (nonConsumableProducts[i].isBuy)
                {
                    ProductDisableGameObject((ProductTYPE)Enum.Parse(typeof(ProductTYPE), nonConsumableProducts[i].productName.ToString()));
                }
            }
        }
    }


    public bool IsPayBuffInApp()
    {
        if (nonConsumableProducts.Count > 0)
        {
            for (int i = 0; i < nonConsumableProducts.Count; i++)
            {
                if (nonConsumableProducts[i].productName.Equals("adbuffpass") && nonConsumableProducts[i].isBuy)
                {
                   return true; 
                }
            }
        }

        return false;
    }
 


    public void BuyGemI() { IAPManager.Instance.BuyProduct(consumableProducts[0].productName, ProductBought); }
    public void BuyGemII() { IAPManager.Instance.BuyProduct(consumableProducts[1].productName, ProductBought); }
    public void BuyGemIII() { IAPManager.Instance.BuyProduct(consumableProducts[2].productName, ProductBought); }
    public void BuyGemIIII() { IAPManager.Instance.BuyProduct(consumableProducts[3].productName, ProductBought); }
    public void BuyVIPGemI() { IAPManager.Instance.BuyProduct(nonConsumableProducts[0].productName, ProductBought); }
    public void BuyVIPGemII() { IAPManager.Instance.BuyProduct(nonConsumableProducts[1].productName, ProductBought); }
    public void BuyVIPGemIII() { IAPManager.Instance.BuyProduct(nonConsumableProducts[2].productName, ProductBought); }
    public void BuyVIPGemIIII() { IAPManager.Instance.BuyProduct(nonConsumableProducts[3].productName, ProductBought); }
    public void BuyStartPackage() { IAPManager.Instance.BuyProduct(nonConsumableProducts[4].productName, ProductBought); }
    public void BuyAdBuffPackage() { IAPManager.Instance.BuyProduct(nonConsumableProducts[5].productName, ProductBought); }
    public void BuyDungeonPackage() { IAPManager.Instance.BuyProduct(nonConsumableProducts[6].productName, ProductBought); }
    public void BuyBattlePass() { IAPManager.Instance.BuyProduct(nonConsumableProducts[7].productName, ProductBought); }
    public void BuyFastestPackage() { IAPManager.Instance.BuyProduct(nonConsumableProducts[8].productName, ProductBought); }

    private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            
            if (product.productType == ProductType.Consumable)
            {
                if (product.productName == "commongem1")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, commonGem1);
                }
                else if (product.productName == "commongem2")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, commonGem2);
                }
                else if (product.productName == "commongem3")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, commonGem3);
                }
                else if (product.productName == "commongem4")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, commonGem4);
                }

            }

            if (product.productType == ProductType.NonConsumable)
            {
                if (product.productName == "vipgem1")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem1);
                    ProductDisableGameObject(ProductTYPE.vipgem1);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "vipgem2")
                {

                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem2);
                    ProductDisableGameObject(ProductTYPE.vipgem2);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "vipgem3")
                {

                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem3);
                    ProductDisableGameObject(ProductTYPE.vipgem3);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "vipgem4")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem4);
                    ProductDisableGameObject(ProductTYPE.vipgem4);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "starterrpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket };
                    var rewardValues = new long[] { 3000, 10, 5, 3 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.starterrpackage);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "adbuffpass")
                {
                    var adKey = GlobalData.instance.adManager.adPassKey;
                    var buffKey = GlobalData.instance.adManager.buffPassKey;
                    PlayerPrefs.SetInt(adKey, 1);
                    PlayerPrefs.SetInt(buffKey, 1);
                    // 버프 패스 실행
                    GlobalData.instance.adManager.BuyBuffPass();
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.clearTicket };
                    var rewardValues = new long[] { 10000, 10 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.adbuffpass);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "dungeonpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.goldKey, EnumDefinition.RewardType.boneKey, EnumDefinition.RewardType.diceKey, EnumDefinition.RewardType.coalKey, EnumDefinition.RewardType.clearTicket };
                    var rewardValues = new long[] { 10, 10, 10, 10, 2 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.dungeonpackage);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "battlepass")
                {

                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem };
                    var rewardValues = new long[] { 1000 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    // UNLOCK BATTLE PASS SLOT
                    var saveKey = GlobalData.instance.questManager.keyBuyBattlePass;
                    PlayerPrefs.SetInt(saveKey, 1);
                    GlobalData.instance.questManager.questPopup.AllUnlockBattlePassSlotItem();

                    ProductDisableGameObject(ProductTYPE.battlepass);
                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }
                else if (product.productName == "fastestpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket, EnumDefinition.RewardType.union };
                    var rewardValues = new long[] { 50000, 50, 30, 50, 40 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.fastestpackage);
                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                }


            }


        }
        else
        {
            //an error occurred in the buy process, log the message for more details
            Debug.Log("Buy product failed: " + message);

        }


    }

    private void InitializeResult(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        //initializationInProgress = false;
        consumableProducts = new List<MyStoreProducts>();
        nonConsumableProducts = new List<MyStoreProducts>();

        if (status == IAPOperationStatus.Success)
        {
            //loop through all products and check which one are bought to update our variables
            for (int i = 0; i < shopProducts.Count; i++)
            {
                //construct a different list of each category of products, for an easy display purpose, not required
                switch (shopProducts[i].productType)
                {
                    case ProductType.Consumable:
                        consumableProducts.Add(new MyStoreProducts(IAPManager.Instance.ConvertNameToShopProduct(shopProducts[i].productName), shopProducts[i].active));
                        break;

                    case ProductType.NonConsumable:
                        nonConsumableProducts.Add(new MyStoreProducts(IAPManager.Instance.ConvertNameToShopProduct(shopProducts[i].productName), shopProducts[i].active));

                        break;
                }
            }
        }
    }

    private void RestoreDone()
    {
        if (IAPManager.Instance.debug)
        {
            Debug.Log("Restore done");
            GleyEasyIAP.ScreenWriter.Write("Restore done");
        }
    }

    void ProductDisableGameObject(ProductTYPE product)
    {
       switch(product)
       {

            case ProductTYPE.vipgem1:
                productGameObject[0].SetActive(true);
                break;
            case ProductTYPE.vipgem2:
                productGameObject[1].SetActive(true);
                break;
            case ProductTYPE.vipgem3:
                productGameObject[2].SetActive(true);
                break;
            case ProductTYPE.vipgem4:
                productGameObject[3].SetActive(true);
                break;
            case ProductTYPE.starterrpackage:
                productGameObject[4].SetActive(true);
                break;
            case ProductTYPE.adbuffpass:
                productGameObject[5].SetActive(true);
                break;
            case ProductTYPE.dungeonpackage:
                productGameObject[6].SetActive(true);
                break;
            case ProductTYPE.battlepass:
                productGameObject[7].SetActive(true);
                break;
            case ProductTYPE.fastestpackage:
                productGameObject[8].SetActive(true);

                break;

            default: break;
        }


    }


}
