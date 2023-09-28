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

    public string environment = "production";

    //bool initializationInProgress = false;
    public IEnumerator Init()
    {
        if (!IAPManager.Instance.IsInitialized())
        {
            IAPManager.Instance.InitializeIAPManager(InitializeResult);
        }
        //IOS만 해당가능
        //IAPManager.Instance.RestorePurchases(ProductBought, RestoreDone);


        yield return new WaitForSeconds(1f);
        CheckBuyNonConsuableProduct();

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
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip1 = true;
                }
                else if (product.productName == "vipgem2")
                {

                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem2);
                    ProductDisableGameObject(ProductTYPE.vipgem2);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip2 = true;
                }
                else if (product.productName == "vipgem3")
                {

                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem3);
                    ProductDisableGameObject(ProductTYPE.vipgem3);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip3 = true;
                }
                else if (product.productName == "vipgem4")
                {
                    PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, vipGem4);
                    ProductDisableGameObject(ProductTYPE.vipgem4);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip4 = true;
                }
                else if (product.productName == "starterrpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket };
                    var rewardValues = new float[] { 3000, 10, 5, 3 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.starterrpackage);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyStaterPack = true;

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
                    var rewardValues = new float[] { 2000, 3 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.adbuffpass);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyAdRemove = true;
                }
                else if (product.productName == "dungeonpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.goldKey, EnumDefinition.RewardType.boneKey, EnumDefinition.RewardType.diceKey, EnumDefinition.RewardType.coalKey, EnumDefinition.RewardType.clearTicket };
                    var rewardValues = new float[] { 10, 10, 10, 10, 2 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.dungeonpackage);

                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyDungeonPack = true;
                }
                else if (product.productName == "battlepass")
                {

                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem };
                    var rewardValues = new float[] { 1000 };
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
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyBattlePass = true;
                }
                else if (product.productName == "fastestpackage")
                {
                    var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket, EnumDefinition.RewardType.union };
                    var rewardValues = new float[] { 50000, 50, 30, 50, 34 };
                    PopupController.instance.InitPopups(rewardTypes, rewardValues);
                    ProductDisableGameObject(ProductTYPE.fastestpackage);
                    if (product.active)
                    {
                        nonConsumableProducts.First(cond => cond.productName.ToString() == product.productName).isBuy = true;
                    }
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyFastestPack = true;
                }


            }


        }
        else
        {
            //an error occurred in the buy process, log the message for more details
            Debug.Log("Buy product failed: " + message);

        }
        GlobalData.instance.saveDataManager.SaveDataToFile();

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

                        var type = IAPManager.Instance.ConvertNameToShopProduct(shopProducts[i].productName);
                        nonConsumableProducts.Add(new MyStoreProducts(type, shopProducts[i].active));

                        if(shopProducts[i].active)
                        {
                            RewardBuyProduct(type);    
                            ProductDisableGameObject((ProductTYPE)Enum.Parse(typeof(ProductTYPE), shopProducts[i].productName.ToString()));
                        }

                        break;
                }
            }

            GlobalData.instance.saveDataManager.SaveDataToFile();


        }

    }

    void RewardBuyProduct(ShopProductNames type)
    {
        switch (type)
        {
            case ShopProductNames.vipgem1:
                if(!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip1)
                {
                GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, vipGem1);
                GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip1 = true;
                }
                break;
            case ShopProductNames.vipgem2:
                if (!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip2)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, vipGem2);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip2 = true;
                }

                break;
            case ShopProductNames.vipgem3:
                if (!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip3)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, vipGem3);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip3 = true;
                }
                break;
            case ShopProductNames.vipgem4:
                if (!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip4)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, vipGem4);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyVip4 = true;
                }
                break;
            case ShopProductNames.starterrpackage:
                if (!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyStaterPack)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, 3000);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.unionTicket, 10);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.dnaTicket, 5);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.clearTicket, 3);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyStaterPack = true;
                }

                break;
            case ShopProductNames.adbuffpass:
                if(!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyAdRemove)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, 2000);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.clearTicket, 3);
                    var adKey = GlobalData.instance.adManager.adPassKey;
                    var buffKey = GlobalData.instance.adManager.buffPassKey;
                    PlayerPrefs.SetInt(adKey, 1);
                    PlayerPrefs.SetInt(buffKey, 1);
                    // 버프 패스 실행
                    GlobalData.instance.adManager.BuyBuffPass();
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyAdRemove = true;
                }

                break;
            case ShopProductNames.dungeonpackage:
                if(!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyDungeonPack)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.goldKey, 10);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.boneKey, 10);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.diceKey, 10);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.coalKey, 10);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.clearTicket, 2);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyDungeonPack = true;
                }
                break;
            case ShopProductNames.battlepass:
                if(!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyBattlePass)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, 1000);
                    // UNLOCK BATTLE PASS SLOT
                    var saveKey = GlobalData.instance.questManager.keyBuyBattlePass;
                    PlayerPrefs.SetInt(saveKey, 1);
                    GlobalData.instance.questManager.questPopup.AllUnlockBattlePassSlotItem();
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyBattlePass = true;
                }
                break;
            case ShopProductNames.fastestpackage:
                if(!GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyFastestPack)
                {
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.gem, 50000);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.unionTicket, 50);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.dnaTicket, 30);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.clearTicket, 50);
                    GlobalData.instance.rewardManager.RewardByType(EnumDefinition.RewardType.union, 34);
                    GlobalData.instance.saveDataManager.saveDataTotal.saveDataShop.isBuyFastestPack = true;
                }
                break;
            default: break;

        }
            GlobalData.instance.saveDataManager.SaveDataToFile();

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
