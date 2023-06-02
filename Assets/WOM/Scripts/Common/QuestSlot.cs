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

    public void ActiveNotifyIcon(QuestData questData) // 전달된 값에 따라 알림 아이콘의 활성화/비활성화 여부를 설정합니다.
    {
        bool isActive = questData.qusetComplete && !questData.usingReward;
        imgNotifyIcon.gameObject.SetActive(isActive);
    }

    public void SetQuestName(string name) // 퀘스트 이름의 텍스트를 설정합니다.
    {
        txtQuestName.text = name;
    }

    public void SetTxtRewardValue(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtRewardValue.text = value;
    }

    public void SetQuestProgress(QuestData questData) // 전달된 퀘스트 데이터의 현재와 목표 값을 기반으로 프로그래스 바의 채우기 정도를 설정합니다.
    {
        float value = (float)questData.curCountValue / (float)questData.targetValue;
        imgQuestProgress.fillAmount = value;
    }

    public void SetQuestProgressCount(QuestData questData) // 퀘스트 진행 상황 카운트의 텍스트를 설정합니다.
    {
        string count = questData.curCountValue.ToString() + " / " + questData.targetValue.ToString();
        txtQuestProgressCount.text = count;
    }

    public void ActiveRewardButton(QuestData questData) // 전달된 값에 따라 보상 버튼을 활성화/비활성화 여부를 설정합니다.
    {
        bool isActive = questData.qusetComplete && !questData.usingReward;
        btnReward.gameObject.SetActive(isActive);
    }

    public void SetDoingText(QuestData questData) // 사용자가 수행 중인 퀘스트의 현재 동작에 대한 텍스트를 설정합니다.
    {
        var txtValue = questData.qusetComplete && questData.usingReward ? "보상완료" : "진행중";
        txtDoing.text = txtValue;
    }

    public void SetQuestTypeOneDay(EnumDefinition.QuestTypeOneDay type) // 사용자 지정 열거형을 사용하여 하루짜리 퀘스트 유형을 설정합니다.
    {
        questTypeOneDay = type;
    }

    // questData 를 인자로 받아서 세팅 하는 함수
    public void SetQuestData(QuestData data)
    {
        questData = data;
    }

    public void UpdateUI(QuestData data)
    {
        ActiveNotifyIcon(data);
        SetQuestProgress(data);
        SetQuestProgressCount(data);
        ActiveRewardButton(data);
    }



}
