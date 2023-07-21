using System.Collections;
using UnityEngine;
using TMPro;

public class BuffADTimer : MonoBehaviour
{
    private float countdownTime = 30 * 60;  // 30 minutes to seconds
    public TextMeshProUGUI countdownText;
    public BuffADSlot buffADSlot;

    void Start()
    {

    }

    public IEnumerator StartTimer()
    {
        buffADSlot.addValue = buffADSlot.addValueBuff;
        while (countdownTime > 0)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60F);
            int seconds = Mathf.FloorToInt(countdownTime - minutes * 60);
            string leftTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            countdownText.text = leftTime + " SEC";  // Update the UI text
            // save data
            GlobalData.instance.saveDataManager.SetSaveDataBuffAD_LeftTime(buffADSlot.buffADType, countdownTime);

            yield return new WaitForSeconds(1.0f);
            countdownTime -= 1.0f;

        }
        buffADSlot.addValue = buffADSlot.addValueDefualt;
        ResetCountdownTime();
        //countdownText.text = "Countdown complete!";
    }

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
        countdownTime = 30 * 60;
    }


}
