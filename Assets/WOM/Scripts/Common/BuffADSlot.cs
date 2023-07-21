using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffADSlot : MonoBehaviour
{
    // 광고 남은 수 표시
    public TextMeshProUGUI txtLeftCount;
    public Button bntAD;
    public BuffADTimer buffTimer;
    public EnumDefinition.BuffADType buffADType;
    public bool isUsingBuff = false;
    public int leftCount = 3;

    public double addValue = 1f;
    public double addValueDefualt = 1.0f;
    public double addValueBuff = 1.5f;


    void Start()
    {

    }

    void SetBtnEvent()
    {
        bntAD.onClick.AddListener(AdCheck);
    }

    void AdCheck()
    {
        if (leftCount > 0)
        {
            // 광고 재생
            // 광고 재생 완료 후
            // 광고 재생 완료 후 버프 적용
            isUsingBuff = true;
            //buffTimer.ReloadTimer(30 * 60);
            //leftCount -= 1;
        }
    }

    void ButtTimerStart()
    {
        leftCount -= 1;
        buffTimer.gameObject.SetActive(true);
        buffTimer.StartCoroutine(buffTimer.StartTimer());
    }

    void UpdateUI()
    {

    }




}
