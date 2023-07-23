using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ADRandomRewardPopupController : MonoBehaviour
{

    public Button btnAdReward;
    public TextMeshProUGUI txtLeftCount;
    public EnumDefinition.RewardTypeAD adRewardType;

    int leftCount;
    int totalCount = 5;

    float[] randomWeights;


    void Start()
    {
        LoadLoadLeftCount();
        SetRandomWeights();
        SetBtnEvent();
    }

    public void SetRandomWeights()
    {
        var data = GlobalData.instance.dataManager.rewardAdGemDats.data;
        randomWeights = new float[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            randomWeights[i] = data[i].probability;
        }
    }



    // load data
    void LoadLoadLeftCount()
    {
        if (PlayerPrefs.HasKey(adRewardType.ToString()))
        {
            leftCount = PlayerPrefs.GetInt(adRewardType.ToString());
            if (leftCount <= 0)
            {
                DisableButton();
            }
        }
        else
        {
            leftCount = totalCount;
            PlayerPrefs.SetInt(adRewardType.ToString(), leftCount);
        }

        UpdateUI();
    }

    void SetBtnEvent()
    {
        btnAdReward.onClick.AddListener(AdCheck);
    }

    void AdCheck()
    {
        if (leftCount > 0)
        {
            Admob.instance.ShowRewardedAdByType(adRewardType);
        }
        else
        {
            Debug.Log("광고 재생 횟수 초과");
        }
    }


    void DisableButton()
    {
        btnAdReward.interactable = false;
    }

    void UpdateUI()
    {
        txtLeftCount.text = leftCount.ToString() + "/" + totalCount.ToString();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         GetRandomReward();
    //     }
    // }


    public void GetRandomReward()
    {
        --leftCount;
        PlayerPrefs.SetInt(adRewardType.ToString(), leftCount);
        var data = GlobalData.instance.dataManager.rewardAdGemDats.data;
        var randomRewardIndex = UtilityMethod.GetWeightRandomValue(randomWeights);
        var rewardValue = data[(int)randomRewardIndex].rewardValue;
        PopupController.instance.InitPopup(EnumDefinition.RewardType.gem, rewardValue);

        UpdateUI();
    }

    // 광고 보기 카운트 초기화
    public void ResetLeftCount()
    {
        leftCount = totalCount;
        PlayerPrefs.SetInt(adRewardType.ToString(), leftCount);
        UpdateUI();
        btnAdReward.interactable = true;
    }

}
