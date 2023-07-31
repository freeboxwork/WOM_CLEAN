using System;
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
    fastestpackage,
    dungeonpackage
}


public class InAppManager : MonoBehaviour, IStoreListener, IDetailedStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    public Button[] productButtons;

    void Start()
    {
        // Unity IAP 초기화
        InitializePurchasing();

        productButtons[(int)ProductTYPE.dungeonpackage].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.dungeonpackage.ToString());
        });
        productButtons[(int)ProductTYPE.starterrpackage].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.starterrpackage.ToString());
        });
        productButtons[(int)ProductTYPE.adbuffpass].onClick.AddListener(() =>
        {
            storeController.InitiatePurchase(ProductTYPE.adbuffpass.ToString());
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

        // Unity IAP 초기화
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // IAP 초기화 성공
        storeController = controller;
        extensionProvider = extensions;
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
            var rewardValues = new long[] { 3000, 10, 5, 2 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("초심자 패키지 구매 성공");
            //보석3000개 유니온티켓10 dna티켓5 소탕권2 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.adbuffpass.ToString())
        {

            //광고 패스, 버프무제한 
            // set pass value
            var adKey = GlobalData.instance.adManager.adPassKey;
            var buffKey = GlobalData.instance.adManager.buffPassKey;
            PlayerPrefs.SetInt(adKey, 1);
            PlayerPrefs.SetInt(buffKey, 1);
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.clearTicket };
            var rewardValues = new long[] { 10000, 5 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("광고 패스 패키지 구매 성공");
            //광고패스/버프무제한 보석10000개 소탕권5 지급

        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.fastestpackage.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gem, EnumDefinition.RewardType.unionTicket, EnumDefinition.RewardType.dnaTicket, EnumDefinition.RewardType.clearTicket, EnumDefinition.RewardType.union };
            var rewardValues = new long[] { 50000, 50, 20, 20, 40 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("초고속 성장 패키지 구매 성공");
            //보석50000개 유니온티켓50 dna티켓20 소탕권20 유니온index 40번 지급
        }
        else if (args.purchasedProduct.definition.id == ProductTYPE.dungeonpackage.ToString())
        {
            var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.goldKey, EnumDefinition.RewardType.boneKey, EnumDefinition.RewardType.diceKey, EnumDefinition.RewardType.coalKey, EnumDefinition.RewardType.clearTicket };
            var rewardValues = new long[] { 10, 10, 10, 10, 2 };
            PopupController.instance.InitPopups(rewardTypes, rewardValues);
            Debug.Log("던전 패키지 구매 성공");
            //던전키4종 10개씩 소탕권2개 지급
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
}






// public class InAppManager : MonoBehaviour, IDetailedStoreListener
// {

//     private static IStoreController m_StoreController;// Reference to the Purchasing system.
//     private static IExtensionProvider m_StoreExtensionProvider;// Reference to store-specific Purchasing subsystems.

//     private static string gem1 = "gem_1";

//     private static InAppManager instance;

//     public static InAppManager Instance
//     {
//         get
//         {
//             return instance;
//         }
//     }

//     void Start()
//     {
//         if (m_StoreController == null)
//         {
//             InitializePurchasing();
//         }

//     }

//     void Awake()
//     {
//         instance = this;

//         DontDestroyOnLoad(this);
//     }

//     public void InitializePurchasing()
//     {
//         // If we have already connected to Purchasing ...
//         if (IsInitialized())
//         {
//             // ... we are done here.
//             return;
//         }

//         // Create a builder, first passing in a suite of Unity provided stores.
//         var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//         // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.

//         builder.AddProduct(gem1, ProductType.Consumable);

//         //builder.AddProduct(autobox, ProductType.NonConsumable);


//         UnityPurchasing.Initialize(this, builder);
//     }

//     private bool IsInitialized()
//     {
//         // Only say we are initialized if both the Purchasing references are set.
//         return m_StoreController != null && m_StoreExtensionProvider != null;
//     }


//     public void BuyItem()
//     {    
//         BuyProductID(gem1);
//     }

//     // 실제 구매가 실행되는 함수. 매개변수로 상품의 프로젝트ID 를 받는다.
//     void BuyProductID(string productId)
//     {
//         // If the stores throw an unexpected exception, use try..catch to protect my logic here.
//         try
//         {
//             // If Purchasing has been initialized ...
//             if (IsInitialized())
//             {
//                 // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
//                 Product product = m_StoreController.products.WithID(productId);
//                 // If the look up found a product for this device's store and that product is ready to be sold ... 
//                 if (product != null && product.availableToPurchase)
//                 {
//                     Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//                     m_StoreController.InitiatePurchase(product);
//                 }
//                 // Otherwise ...
//                 else
//                 {
//                     // ... report the product look-up failure situation  
//                     Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//                 }
//             }
//             // Otherwise ...
//             else
//             {
//                 // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
//                 Debug.Log("BuyProductID FAIL. Not initialized.");
//             }
//         }
//         // Complete the unexpected exception handling ...
//         catch (Exception e)
//         {
//             // ... by reporting any unexpected exception for later diagnosis.
//             Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
//         }
//     }
//     // 구매복원 
//     // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
//     public void RestorePurchases()
//     {
//         // If Purchasing has not yet been set up ...
//         if (!IsInitialized())
//         {
//             // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//             Debug.Log("RestorePurchases FAIL. Not initialized.");
//             return;
//         }

//         // If we are running on an Apple device ... 
//         if (Application.platform == RuntimePlatform.IPhonePlayer ||
//             Application.platform == RuntimePlatform.OSXPlayer)
//         {
//             // ... begin restoring purchases
//             Debug.Log("RestorePurchases started ...");

//             // Fetch the Apple store-specific subsystem.
//             var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

//             apple.RestoreTransactions((bool success, string additionalInfo) =>
//             {
//                 if (success)
//                 {
//                     // 트랜잭션 복원 성공
//                     // additionalInfo 매개변수에 추가 정보가 포함될 수 있습니다.
//                     // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
//                     Debug.Log("RestorePurchases continuing: " + success + ". If no further messages, no purchases available to restore.");
//                 }
//                 else
//                 {
//                     // 트랜잭션 복원 실패
//                 }
//             });


//         }
//         // Otherwise ...
//         else
//         {
//             // We are not running on an Apple device. No work is necessary to restore purchases.
//             Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//         }
//     }
//     //  
//     // --- IStoreListener
//     //
//     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//     {
//         // Purchasing has succeeded initializing. Collect our Purchasing references.
//         Debug.Log("OnInitialized: PASS");

//         // Overall Purchasing system, configured with products for this application.
//         m_StoreController = controller;
//         // Store specific subsystem, for accessing device-specific store features.
//         m_StoreExtensionProvider = extensions;
//     }

//     public void OnInitializeFailed(InitializationFailureReason error)
//     {
//         // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//         Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//     }
//     //실제 구매후 상품 지급
//     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//     {
//         bool t_IsBuy = true;


// #if !UNITY_EDITOR
//         var t_Validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

//         try
//         {
//             // 구매 처리 중 문제가 없다면 이쪽으로 진행됨
//             var t_Result = t_Validator.Validate(args.purchasedProduct.receipt);

//             foreach (IPurchaseReceipt productReceipt in t_Result)
//             {
//                 // 유니티 Analytics 에 전송                
//                 Analytics.Transaction(productReceipt.productID, args.purchasedProduct.metadata.localizedPrice, args.purchasedProduct.metadata.isoCurrencyCode, productReceipt.transactionID, null);
//             }
//         }
//         catch (IAPSecurityException)
//         {
//             // 구매 처리 중 오류 발생히 이쪽으로 진행됨
//             t_IsBuy = false;
//             Debug.Log("구매 실패");
//         }
// #endif
//         if (t_IsBuy)
//         {
//             // 정상 구매 성공시 처리할 내용을 작성하는 영역
//             // A consumable product has been purchased by this user.
//             if (String.Equals(args.purchasedProduct.definition.id, gem1, StringComparison.Ordinal))
//             {
//                 //보석 1000원
//                 Debug.Log("보석 I 구매 성공");
//             }


//             return PurchaseProcessingResult.Complete;


//         }

//         return PurchaseProcessingResult.Pending;

//         // Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
//     }

//     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//     {
//         // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
//         Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

//         Debug.Log(" product.transactionID : " + product.transactionID);

//         //Restore
//         if (failureReason == PurchaseFailureReason.DuplicateTransaction)
//         {
//             // if (String.Equals(product.definition.storeSpecificId, autobox, StringComparison.Ordinal))
//             // {
//             //     gameDataManager.GetDataInfo().isPackageType[(int)ProductPackageType.AutoBox] = true;
//             //     GameController.Instance.isBuyAutoBox = true;
//             //     AdvertiseManager.Instance.CheckClearGoldPurchase();
//             // }

//         }


//     }

//     void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
//     {
//         throw new NotImplementedException();
//     }

//     void IDetailedStoreListener.OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
//     {
//         throw new NotImplementedException();
//     }


//}
