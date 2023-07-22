using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffADSlot : MonoBehaviour
{
    // 광고 남은 수 표시
    public TextMeshProUGUI txtLeftCount;
    public Button bntAD;
    public BuffADTimer buffTimer;
    public EnumDefinition.RewardTypeAD buffADType;
    public bool isUsingBuff = false;
    public int leftCount = 3;
    public int totalCount = 3;

    public double addValue = 1f;
    public double addValueDefualt = 1.0f;
    public double addValueBuff = 1.5f;


    void Start()
    {
        LoadLoadLeftCount();
        SetBtnEvent();
    }

    // load data
    void LoadLoadLeftCount()
    {
        if (PlayerPrefs.HasKey(buffADType.ToString()))
        {
            leftCount = PlayerPrefs.GetInt(buffADType.ToString());
            if (leftCount <= 0)
            {
                DisableButton();
            }
        }
        else
        {
            leftCount = 3;
            PlayerPrefs.SetInt(buffADType.ToString(), leftCount);
        }

        UpdateUI();
    }

    void SetBtnEvent()
    {
        bntAD.onClick.AddListener(AdCheck);
    }

    void AdCheck()
    {
        if (leftCount > 0)
        {
            Admob.instance.ShowRewardedAdByType(buffADType);
        }
        else
        {
            Debug.Log("광고 재생 횟수 초과");
        }
    }

    public void BuffTimerStart()
    {
        --leftCount;
        PlayerPrefs.SetInt(buffADType.ToString(), leftCount);
        buffTimer.StartCoroutine(buffTimer.StartTimer());
        UpdateUI();
        if (leftCount <= 0)
        {
            DisableButton();
        }
    }

    void DisableButton()
    {
        bntAD.interactable = false;
    }

    void UpdateUI()
    {
        txtLeftCount.text = "일일 시청 가능 " + leftCount.ToString() + "/" + totalCount.ToString();
    }

    // 광고 보기 카운트 초기화
    public void ResetLeftCount()
    {
        leftCount = totalCount;
        PlayerPrefs.SetInt(buffADType.ToString(), leftCount);
        UpdateUI();
        bntAD.interactable = true;
    }


}
