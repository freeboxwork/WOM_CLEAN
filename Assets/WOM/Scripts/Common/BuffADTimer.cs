using System.Collections;
using UnityEngine;
using TMPro;


public class BuffADTimer : MonoBehaviour
{
    float defaultTime = 30 * 60;
    private float countdownTime = 30 * 60;  // 30 minutes to seconds
    public TextMeshProUGUI countdownText;
    public BuffADSlot buffADSlot;
    public GameObject timerUI;
    public CanvasGroup canvasGroup;

    void Start()
    {
        ResetAllwaysCountDownText();
        canvasGroup.alpha = 0f;
    }

    public IEnumerator StartTimer()
    {
        canvasGroup.alpha = 1;
        timerUI.SetActive(true);
        buffADSlot.isUsingBuff = true;
        buffADSlot.addValue = buffADSlot.addValueBuff;
        // save data
        GlobalData.instance.saveDataManager.SetSaveDataBuffAD_Using(buffADSlot.buffADType, buffADSlot.isUsingBuff);
        while (countdownTime > 0)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60F);
            int seconds = Mathf.FloorToInt(countdownTime - minutes * 60);
            //string leftTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            string leftTime = $"{minutes}:{seconds}";//string.Format("{0:0}m", minutes);

            countdownText.text = leftTime;// Update the UI text
            // save data
            GlobalData.instance.saveDataManager.SetSaveDataBuffAD_LeftTime(buffADSlot.buffADType, countdownTime);

            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1.0f;

        }
        timerUI.SetActive(false);

        buffADSlot.isUsingBuff = false;
        canvasGroup.alpha = 0f;

        // save data
        GlobalData.instance.saveDataManager.SetSaveDataBuffAD_Using(buffADSlot.buffADType, buffADSlot.isUsingBuff);

        ResetAllwaysCountDownText();
        buffADSlot.addValue = buffADSlot.addValueDefualt;
        ResetCountdownTime();
    }

    // 게임 종료후 재 접속시 남은 시간에 따라 타이머 시작
    public void ReloadTimer(float time)
    {
        countdownTime = time;
        StartCoroutine(StartTimer());
    }

    public void SetCountdownTime(float time)
    {
        countdownTime = time;
    }

    public void ResetCountdownTime()
    {
        countdownTime = defaultTime;
    }

    void ResetAllwaysCountDownText()
    {
        canvasGroup.alpha = 0f;
    }

    public void SetTxtBuffPass()
    {
        canvasGroup.alpha = 1;
        SetBuffPassUI();
    }

    public void TimerEnd()
    {
        StopAllCoroutines();
    }

    void SetBuffPassUI()
    {
        //버프중임을 알리는 화면 활성화
        timerUI.SetActive(true);
        countdownText.text = "무제한";

    }

}
