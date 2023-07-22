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
        }
        else
        {
            leftCount = 3;
            PlayerPrefs.SetInt(buffADType.ToString(), leftCount);
        }
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
            // 광고 재생
            // 광고 재생 완료 후
            // 광고 재생 완료 후 버프 적용

            //buffTimer.ReloadTimer(30 * 60);
            //leftCount -= 1;
        }
        else
        {
            Debug.Log("광고 재생 횟수 초과");
        }
    }

    public void BuffTimerStart()
    {
        leftCount -= 1;
        PlayerPrefs.SetInt(buffADType.ToString(), leftCount);

        buffTimer.gameObject.SetActive(true);
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




}
