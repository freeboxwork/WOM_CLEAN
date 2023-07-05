using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class WomSystemMnager : MonoBehaviour
{

    public GameObject powerSavingPopup;
    public Button btnPowerSavingOff;


    void Start()
    {
        AddBtnEvents();
    }

    void AddBtnEvents()
    {
        btnPowerSavingOff.onClick.AddListener(() =>
        {
            powerSavingPopup.SetActive(false);
            PowerSavingMone(EnumDefinition.PowerSavingMode.off);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 종료 팝업
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(18, QuitPopupApply, QuitPopupCancel);

        }
    }


    void QuitPopupCancel()
    {
        // 팝업 닫기
        Debug.Log("게임종료 팝업 닫기");
    }

    void QuitPopupApply()
    {
        StartCoroutine(GlobalData.instance.saveDataManager.SaveDataToFileCoroutine());
    }

    public void PowerSavingModeOn()
    {
        powerSavingPopup.SetActive(true);
        PowerSavingMone(EnumDefinition.PowerSavingMode.on);
    }

    public void PowerSavingMone(EnumDefinition.PowerSavingMode mode)
    {
        var powerSavingValue = mode == EnumDefinition.PowerSavingMode.on ? 3 : 1;
        OnDemandRendering.renderFrameInterval = powerSavingValue;

    }




}
