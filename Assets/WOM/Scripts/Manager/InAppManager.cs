using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
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


public class InAppManager : MonoBehaviour, IStoreListener, IDetailedStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    public Button[] productButtons;
    public GameObject[] productGameObject;

    void Start()
    {
        // Unity IAP 초기화
        InitializePurchasing();
        productButtons[(int)ProductTYPE.starterrpackage].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.starterrpackage.ToString());
        });
        productButtons[(int)ProductTYPE.adbuffpass].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.adbuffpass.ToString());
        });

        productButtons[(int)ProductTYPE.dungeonpackage].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.dungeonpackage.ToString());
        });
        productButtons[(int)ProductTYPE.battlepass].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.battlepass.ToString());
        });
        productButtons[(int)ProductTYPE.fastestpackage].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.fastestpackage.ToString());
        });

        productButtons[(int)ProductTYPE.commongem1].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.commongem1.ToString());
        });
        productButtons[(int)ProductTYPE.commongem2].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.commongem2.ToString());
        });
        productButtons[(int)ProductTYPE.commongem3].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.commongem3.ToString());
        });
        productButtons[(int)ProductTYPE.commongem4].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.commongem4.ToString());
        });

        productButtons[(int)ProductTYPE.vipgem1].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.vipgem1.ToString());
        });
        productButtons[(int)ProductTYPE.vipgem2].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.vipgem2.ToString());
        });
        productButtons[(int)ProductTYPE.vipgem3].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.vipgem3.ToString());
        });
        productButtons[(int)ProductTYPE.vipgem4].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.vipgem4.ToString());
        });


    }

    public void InitializePurchasing()
    {
        // IAP 초기화
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //상품 ID 등록

        //Consumable
        builder.AddProduct(ProductTYPE.commongem1.ToString(), ProductType.Consumable);
        builder.AddProduct(ProductTYPE.commongem2.ToString(), ProductType.Consumable);
        builder.AddProduct(ProductTYPE.commongem3.ToString(), ProductType.Consumable);
        builder.AddProduct(ProductTYPE.commongem4.ToString(), ProductType.Consumable);
        //NonConsumable
        builder.AddProduct(ProductTYPE.vipgem1.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.vipgem2.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.vipgem3.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.vipgem4.ToString(), ProductType.NonConsumable);

        builder.AddProduct(ProductTYPE.starterrpackage.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.adbuffpass.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.fastestpackage.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.dungeonpackage.ToString(), ProductType.NonConsumable);
        builder.AddProduct(ProductTYPE.battlepass.ToString(), ProductType.NonConsumable);

        // Unity IAP 초기화
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // IAP 초기화 성공
        storeController = controller;
        extensionProvider = extensions;

//        Debug.Log("인앱매니저 초기화 성공");
//        Debug.Log("스타트패키지 구매 : " + storeController.products.WithID(ProductTYPE.starterrpackage.ToString()).hasReceipt);
        
        //이미 구매한 상품인지 체크하여 패키지 패널을 비활성화 시킨다
        if (storeController.products.WithID(ProductTYPE.starterrpackage.ToString()).hasReceipt)
        {
            ProductDisableGameObject(ProductTYPE.starterrpackage);     
        }
        if (storeController.products.WithID(ProductTYPE.adbuffpass.ToString()).hasReceipt)
        {
            ProductDisableGameObject(ProductTYPE.adbuffpass);     

        }
        if (storeController.products.WithID(ProductTYPE.fastestpackage.ToString()).hasReceipt)
        {
            ProductDisableGameObject(ProductTYPE.fastestpackage);     

        }
        if (storeController.products.WithID(ProductTYPE.dungeonpackage.ToString()).hasReceipt)
        {
            ProductDisableGameObject(ProductTYPE.dungeonpackage);     

        }
        if (storeController.products.WithID(ProductTYPE.battlepass.ToString()).hasReceipt)
        {
            ProductDisableGameObject(ProductTYPE.battlepass);     
        }

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // IAP 초기화 실패
        Debug.Log("IAP initialization failed: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // 상품 구매 성공
        // 여기서 구매한 아이템을 처리하는 코드를 작성하세요.
        Debug.Log("Purchase successful: " + args.purchasedProduct.definition.id);

        if (args.purchasedProduct.definition.id == ProductTYPE.commongem1.ToString())
        {
            var rewardValue = 1000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("보석 1 구매 성공 : " + rewardValue);
            //보석1000개지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.commongem2.ToString())
        {
            var rewardValue = 5500;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("보석 2 구매 성공 : " + rewardValue);
            //보석5500개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.commongem3.ToString())
        {
            var rewardValue = 12000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("보석 3 구매 성공 : " + rewardValue);
            //보석12000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.commongem4.ToString())
        {
            var rewardValue = 39000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("보석 4 구매 성공 : " + rewardValue);
            //보석 39000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.vipgem1.ToString())
        {
            var rewardValue = 2000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("VIP 보석 1 구매 성공 : " + rewardValue);
            //보석2000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.vipgem2.ToString())
        {
            var rewardValue = 11000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("VIP 보석 2 구매 성공 : " + rewardValue);
            //보석11000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.vipgem3.ToString())
        {

            var rewardValue = 24000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("VIP 보석 3 구매 성공 : " + rewardValue);
            //보석 24000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.vipgem4.ToString())
        {
            var rewardValue = 78000;
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);
            Debug.Log("VIP 보석 4 구매 성공 : " + rewardValue);
            //보석78000개 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.starterrpackage.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket };
            var rewardValues = new long[] { 3000, 10, 5, 3 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("초심자 패키지 구매 성공");
            ProductDisableGameObject(ProductTYPE.starterrpackage);
            Debug.Log("스타트패키지 구매 : " + storeController.products.WithID(ProductTYPE.starterrpackage.ToString()).hasReceipt);
     
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.adbuffpass.ToString())
        {

            //광고 패스, 버프무제한 
            // set pass value
            var adKey = GlobalData.instance.adManager.adPassKey;
            var buffKey = GlobalData.instance.adManager.buffPassKey;
            PlayerPrefs.SetInt(adKey, 1);
            PlayerPrefs.SetInt(buffKey, 1);

            // 광고 카운트 초기화
            // GlobalData.instance.adManager.AllResetADLeftCount();

            // 버프 패스 실행
            GlobalData.instance.adManager.BuyBuffPass();

            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.clearTicket };
            var rewardValues = new long[] { 10000, 10 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("광고 패스 패키지 구매 성공");
            ProductDisableGameObject(ProductTYPE.adbuffpass);     

        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.fastestpackage.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket, EnumDefinition.RewardType.union };
            var rewardValues = new long[] { 50000, 50, 30, 50, 40 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("초고속 성장 패키지 구매 성공");
            ProductDisableGameObject(ProductTYPE.fastestpackage);     
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.dungeonpackage.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.goldKey, EnumDefinition.RewardType.boneKey, EnumDefinition.RewardType.diceKey, EnumDefinition.RewardType.coalKey, EnumDefinition.RewardType.clearTicket };
            var rewardValues = new long[] { 10, 10, 10, 10, 2 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("던전 패키지 구매 성공");
            ProductDisableGameObject(ProductTYPE.dungeonpackage);     
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.battlepass.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem };
            var rewardValues = new long[] { 1000 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);

            // UNLOCK BATTLE PASS SLOT
            var saveKey = GlobalData.instance.questManager.keyBuyBattlePass;
            PlayerPrefs.SetInt(saveKey, 1);
            GlobalData.instance.questManager.questPopup.AllUnlockBattlePassSlotItem();

            ProductDisableGameObject(ProductTYPE.battlepass);     

        }


        // 구매 완료 처리
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // 상품 구매 실패
        Debug.Log("Purchase failed: " + failureReason);

        if (failureReason == PurchaseFailureReason.DuplicateTransaction)
        {

        }

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        // IAP 초기화 실패
        Debug.Log("IAP initialization failed: " + error + ", " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

        Debug.Log($"구매 실패 - {product.definition.id}, {failureDescription.reason}");

    }

    void ProductDisableGameObject(ProductTYPE product)
    {
       switch(product)
       {
            case ProductTYPE.starterrpackage:
            productGameObject[(int)(ProductTYPE.starterrpackage -8)].SetActive(false);
            break;    
            case ProductTYPE.adbuffpass: 
            productGameObject[(int)(ProductTYPE.adbuffpass-8)].SetActive(false);

            break;
            case ProductTYPE.dungeonpackage:
            productGameObject[(int)(ProductTYPE.dungeonpackage-8)].SetActive(false);

            break;    
            case ProductTYPE.fastestpackage:
            productGameObject[(int)(ProductTYPE.fastestpackage-8)].SetActive(false);

            break;    
            case ProductTYPE.battlepass:
            productGameObject[(int)(ProductTYPE.battlepass-8)].SetActive(false);

            break;    
            default:break;
       }
    }


}


