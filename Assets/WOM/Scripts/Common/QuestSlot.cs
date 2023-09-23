using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlot : MonoBehaviour
{

    public QuestData questData;
    public Image imgNotifyIcon;
    public Image imgRewardIcon;
    public TextMeshProUGUI txtRewardValue;
    public TextMeshProUGUI txtQuestName;
    public Image imgQuestProgress;
    public TextMeshProUGUI txtQuestProgressCount;
    public Button btnReward;
    public Button btnAD;
    public TextMeshProUGUI txtDoing;
    public EnumDefinition.QuestTypeOneDay questTypeOneDay;

    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnReward.onClick.AddListener(() =>
        {
            // 리워드 획득
            EventManager.instance.RunEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDay, questData);
            ActiveNotifyIcon(questData);
            ActiveRewardButton(questData);
            SetDoingText(questData);

            // 광고 버튼 활성화
            ActiveADButton(questData);
        });

        btnAD.onClick.AddListener(() =>
        {
            // 광고 시청
            EventManager.instance.RunEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDayAD, questData);
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.showAd);

        });
    }
    public void SetNotifyIcon(Sprite sprite) // 알림 아이콘의 스프라이트를 설정합니다.
    {
        imgNotifyIcon.sprite = sprite;
    }

    public void SetRewardIcon(Sprite sprite) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        imgRewardIcon.sprite = sprite;
    }

    /// <summary>
    /// 전달된 값에 따라 알림 아이콘의 활성화/비활성화 여부를 설정합니다.
    /// </summary>
    public void ActiveNotifyIcon(QuestData questData)
    {
        bool isActive = questData.qusetComplete && !questData.usingReward;
        imgNotifyIcon.gameObject.SetActive(isActive);
    }
    /// <summary>
    /// 퀘스트 이름의 텍스트를 설정합니다.
    /// </summary>
    /// <param name="name"></param>
    public void SetQuestName(string name)
    {
        txtQuestName.text = name;
    }
    /// <summary>
    /// 보상 가치의 텍스트를 설정합니다.
    /// </summary>
    public void SetTxtRewardValue(string value)
    {
        txtRewardValue.text = value;
    }
    /// <summary>
    /// 전달된 퀘스트 데이터의 현재와 목표 값을 기반으로 프로그래스 바의 채우기 정도를 설정합니다.
    /// </summary>
    public void SetQuestProgress(QuestData questData)
    {
        float value = (float)questData.curCountValue / (float)questData.targetValue;
        imgQuestProgress.fillAmount = value;
    }
    /// <summary>
    /// 퀘스트 진행 상황 카운트의 텍스트를 설정합니다.
    /// </summary>
    public void SetQuestProgressCount(QuestData questData)
    {
        string count = questData.curCountValue.ToString() + " / " + questData.targetValue.ToString();
        txtQuestProgressCount.text = count;
    }
    /// <summary>
    /// 전달된 값에 따라 보상 버튼을 활성화/비활성화 여부를 설정합니다.
    /// </summary>
    public void ActiveRewardButton(QuestData questData)
    {
        bool isActive = questData.qusetComplete && !questData.usingReward;
        btnReward.gameObject.SetActive(isActive);
    }
    /// <summary>
    /// 사용자가 수행 중인 퀘스트의 현재 동작에 대한 텍스트를 설정합니다.
    /// </summary>
    public void SetDoingText(QuestData questData)
    {
        var txtValue = questData.qusetComplete && questData.usingReward ? "보상완료" : "진행중";
        txtDoing.text = txtValue;
    }

    public void ActiveADButton(QuestData questData)
    {
        bool isActive = questData.qusetComplete && questData.usingReward && !questData.usingRewardAD;
        btnAD.gameObject.SetActive(isActive);
    }
    /// <summary>
    /// 사용자 지정 열거형을 사용하여 하루짜리 퀘스트 유형을 설정합니다.
    /// </summary>
    public void SetQuestTypeOneDay(EnumDefinition.QuestTypeOneDay type) // 
    {
        questTypeOneDay = type;
    }

    // questData 를 인자로 받아서 세팅 하는 함수
    public void SetQuestData(QuestData data)
    {
        questData = data;
        questData.questSlot = this;
    }

    public void UpdateUI(QuestData data)
    {
        ActiveNotifyIcon(data);
        SetQuestProgress(data);
        SetQuestProgressCount(data);
        ActiveRewardButton(data);
    }



}
