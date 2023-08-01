using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineRewardPopupContoller : MonoBehaviour
{

    public GameObject popupObject;
    public TextMeshProUGUI txtNormalGoldCount;
    public TextMeshProUGUI txtNormalBoneCount;
    public TextMeshProUGUI txtADGoldCount;
    public TextMeshProUGUI txtADBoneCount;

    public Button btnRewardAD;
    public Button btnRewardNormal;

    public long rewardGold;
    public long rewardBone;


    public long rewardAdGold;
    public long rewardAdBone;

    // 광고는 5배 보상
    int adValue = 5;

    void Start()
    {
        SetButtonEvents();
    }



    public IEnumerator Init()
    {
        // 첫 실행이 아닐때만
        var key = GlobalData.instance.playerDataManager.offlineTimeKey;
        if (PlayerPrefs.HasKey(key))
        {
            var offlineTime = GlobalData.instance.playerDataManager.GetOfflineTimeValue();
            var hour = (int)offlineTime.TotalHours;
            if (hour >= 1)
            {
                // 최대 보상 8시간 제한
                if (hour > 8) hour = 8;

                Debug.Log("오프라인 보상 시간 : " + hour);

                // get current boss monster data
                var monData = GlobalData.instance.monsterManager.GetMonsterData(EnumDefinition.MonsterType.boss);


                var mGold = monData.gold * (1 + (GlobalData.instance.statManager.OfflineBonus() * 0.01f));
                var mBone = monData.bone * (1 + (GlobalData.instance.statManager.OfflineBonus() * 0.01f));

                rewardGold = (long)mGold * hour;
                rewardBone = (long)mBone * hour;

                // 광고는 5배 보상
                rewardAdGold = rewardGold * adValue;
                rewardAdBone = rewardBone * adValue;

                updateUI();
                popupObject.SetActive(true);

            }
            else
            {
                Debug.Log("1시간 미만 으로 획득할 오프라인 보상이 없음");
            }
        }
        yield return null;
    }



    void SetButtonEvents()
    {
        btnRewardNormal.onClick.AddListener(() =>
        {
            RewardNormal();
            // 보상 획득
            popupObject.SetActive(false);
        });
        btnRewardAD.onClick.AddListener(() =>
        {
            // 광고 시청
            Admob.instance.ShowRewardedAdByType(EnumDefinition.RewardTypeAD.adOffline);
            popupObject.SetActive(false);
        });
    }

    public void RewardNormal()
    {
        var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gold, EnumDefinition.RewardType.bone };
        var rewardValues = new long[] { rewardGold, rewardBone };
        PopupController.instance.InitPopups(rewardTypes, rewardValues);
    }

    public void RewardAD()
    {
        var rewardTypes = new EnumDefinition.RewardType[] { EnumDefinition.RewardType.gold, EnumDefinition.RewardType.bone };
        var rewardValues = new long[] { rewardAdGold, rewardAdBone };
        PopupController.instance.InitPopups(rewardTypes, rewardValues);
    }


    public void updateUI()
    {
        txtNormalGoldCount.text = rewardGold.ToString();
        txtNormalBoneCount.text = rewardBone.ToString();
        txtADGoldCount.text = rewardAdGold.ToString();
        txtADBoneCount.text = rewardAdBone.ToString();
    }

}
