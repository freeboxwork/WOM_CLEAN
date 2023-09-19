using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";


    private RewardedAd rewardedAd;

    public static Admob instance;

    bool isInitialized = false;


    void Start()
    {
        SetInstance();

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            isInitialized = true;
            LoadRewardedAd();
            Debug.Log("Admob 초기화 완료");
        });

    }


    // set instance
    void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance);
            instance = this;
        }

    }

    void Init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            isInitialized = true;
        });
    }



    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        if(!isInitialized) 
        {
            Debug.Log("Admob 초기화 안됨");
            Init();
            return; 
        }
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            DestroyAd();
        }

//        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);

            });
    }
    void DestroyAd()
    {
        if (rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            rewardedAd.Destroy();
            rewardedAd = null;
        }
    }

    // public void ShowRewardedAd()
    // {
    //     const string rewardMsg =
    //         "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    //     if (rewardedAd != null && rewardedAd.CanShowAd())
    //     {
    //         rewardedAd.Show((Reward reward) =>
    //         {
    //             // TODO: Reward the user.
    //             Debug.Log("광고 시청 완료");
    //             Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
    //         });
    //     }
    //     else
    //     {
    //         Debug.LogError("Rewarded ad is not ready yet.");
    //     }
    // }

    public void ShowRewardedAdByType(EnumDefinition.RewardTypeAD adRewardType)
    {

        // check ad pass
        var passValue = GlobalData.instance.adManager.GetADpAssValue();
        if (passValue)
        {
            // 리워드 지급
            GlobalData.instance.adManager.RewardAd(adRewardType);
            return;
        }

        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // 리워드 지급
                GlobalData.instance.adManager.RewardAd(adRewardType);
                Debug.Log("광고 시청 완료");
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            Debug.Log("광고가 로드되지 않았습니다");
        }
    }

    public void ShowRewardedAdQusetOneDay(QuestData data)
    {

        // check ad pass
        var passValue = GlobalData.instance.adManager.GetADpAssValue();
        if (passValue)
        {
            // 리워드 지급
            GlobalData.instance.questManager.RewardAD_OneDayQuest(data);
            return;
        }

        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // 리워드 지급
                GlobalData.instance.questManager.RewardAD_OneDayQuest(data);
                Debug.Log("광고 시청 완료");
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            Debug.Log("광고가 로드되지 않았습니다");
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("광고 화면이 닫힘.");

            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            LoadRewardedAd();
        };
    }

}