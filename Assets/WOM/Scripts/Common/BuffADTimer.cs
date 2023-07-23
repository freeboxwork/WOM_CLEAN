using System.Collections;
using UnityEngine;
using TMPro;

public class BuffADTimer : MonoBehaviour
{
    float defaultTime = 30 * 60;
    private float countdownTime = 30 * 60;  // 30 minutes to seconds
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI countdownTextAllwaysViwe;
    public BuffADSlot buffADSlot;

    public GameObject timerUI;

    void Start()
    {
        ResetAllwaysCountDownText();
    }

    public IEnumerator StartTimer()
    {
        timerUI.SetActive(true);
        buffADSlot.isUsingBuff = true;
        buffADSlot.addValue = buffADSlot.addValueBuff;
        // save data
        GlobalData.instance.saveDataManager.SetSaveDataBuffAD_Using(buffADSlot.buffADType, buffADSlot.isUsingBuff);
        while (countdownTime > 0)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60F);
            int seconds = Mathf.FloorToInt(countdownTime - minutes * 60);
            string leftTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            countdownText.text = leftTime + " SEC";  // Update the UI text
            countdownTextAllwaysViwe.text = leftTime;
            // save data
            GlobalData.instance.saveDataManager.SetSaveDataBuffAD_LeftTime(buffADSlot.buffADType, countdownTime);

            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1.0f;

        }
        buffADSlot.isUsingBuff = false;

        // save data
        GlobalData.instance.saveDataManager.SetSaveDataBuffAD_Using(buffADSlot.buffADType, buffADSlot.isUsingBuff);

        ResetAllwaysCountDownText();
        buffADSlot.addValue = buffADSlot.addValueDefualt;
        ResetCountdownTime();
        timerUI.SetActive(false);
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
        countdownTextAllwaysViwe.text = "00:00";
    }


}
