using UnityEngine;
using UnityEngine.UI;
using Google.Play.Review;

public class SettingPopupController : MonoBehaviour
{

    public Button btnBgmOnOff;
    public Button btnSfxOnOff;
    public Button btnGoToReview;
    public Button btnPowerSaving;
    public Button btnSetting;
    public Button btnClose;

    public WomSystemMnager womSystemMnager;
    public GameObject popupSetting;

    public OnOffSliderSlot onOffSliderSlot_Bgm;
    public OnOffSliderSlot onOffSliderSlot_Sfx;
    public OnOffSliderSlot onOffSliderSlot_CloudeSave;

    void Start()
    {

    }

    public void SetUI()
    {

        //set bgm on off slider
        var isBgmOn = GlobalData.instance.soundManager.bgmOn;
        onOffSliderSlot_Bgm.Init(isBgmOn);

        //set sfx on off slider
        var isSfxOn = GlobalData.instance.soundManager.sfxOn;
        onOffSliderSlot_Sfx.Init(isSfxOn);


        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnBgmOnOff.onClick.AddListener(() =>
        {
            GlobalData.instance.soundManager.BGM_OnOff();

            var value = GlobalData.instance.soundManager.bgmOn;
            onOffSliderSlot_Bgm.SetSliderValue(value);
        });

        btnSfxOnOff.onClick.AddListener(() =>
        {
            GlobalData.instance.soundManager.SFX_OnOff();

            var value = GlobalData.instance.soundManager.sfxOn;
            onOffSliderSlot_Sfx.SetSliderValue(value);
        });

        btnPowerSaving.onClick.AddListener(() =>
        {
            womSystemMnager.PowerSavingModeOn();
        });

        btnClose.onClick.AddListener(() =>
        {
            popupSetting.SetActive(false);
        });

        btnSetting.onClick.AddListener(() =>
        {
            popupSetting.SetActive(true);
        });
        btnGoToReview.onClick.AddListener(() =>
        {
            Review();
        });

    }


    void Review()
    {
        Application.OpenURL("market://details?id=com.DonnieNest.WOM");
    }



}


