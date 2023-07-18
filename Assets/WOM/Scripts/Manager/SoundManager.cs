using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playerBgm;
    public AudioSource playerSfxInGame;   // 몬스터 공격과 같은 효과음
    public AudioSource playerSfxUiPlayer; // UI 에서 사용되는 효과음

    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> sfxList = new List<AudioClip>();

    public bool bgmOn;
    public bool sfxOn;

    const string bgmOnOffKey = "bgmOnOff";
    const string sfxOnOffKey = "sfxOnOff";

    void Start()
    {

    }

    public IEnumerator Init()
    {
        bgmOn = IsBgmOn();
        sfxOn = IsSfxOn();
        yield return new WaitForEndOfFrame();

        // set bgm clip
        SetBgmClip(EnumDefinition.BGM_TYPE.BGM_Main);

        if (bgmOn)
            PlayBgm();

        // set sfx volume
        var volume = sfxOn ? 1 : 0;
        playerSfxInGame.volume = volume;
        playerSfxUiPlayer.volume = volume;

        // Set Setting Popup UI
        yield return new WaitForEndOfFrame();
        GlobalData.instance.settingPopupController.SetUI();
    }

    public void BGM_OnOff()
    {
        bgmOn = !bgmOn;
        if (bgmOn)
        {
            playerBgm.Play();
            PlayerPrefs.SetString(bgmOnOffKey, "on");
        }

        else
        {
            playerBgm.Stop();
            PlayerPrefs.SetString(bgmOnOffKey, "off");
        }
    }

    bool IsBgmOn()
    {
        if (!PlayerPrefs.HasKey(bgmOnOffKey))
            return true;

        var bgmOnOff = PlayerPrefs.GetString(bgmOnOffKey);
        return bgmOnOff == "on" ? true : false;
    }

    bool IsSfxOn()
    {
        if (!PlayerPrefs.HasKey(sfxOnOffKey))
            return true;

        var sfxOnOff = PlayerPrefs.GetString(sfxOnOffKey);
        return sfxOnOff == "on" ? true : false;
    }

    public void SFX_OnOff()
    {
        sfxOn = !sfxOn;
        var volume = sfxOn ? 1 : 0;
        playerSfxInGame.volume = volume;
        playerSfxUiPlayer.volume = volume;

        var saveValue = sfxOn ? "on" : "off";
        PlayerPrefs.SetString(sfxOnOffKey, saveValue);
    }

    public void SetBgmClip(EnumDefinition.BGM_TYPE bgmType)
    {
        playerBgm.clip = bgmList[(int)bgmType];
    }

    public void PlayBgm()
    {
        playerBgm.Play();
    }

    public void PlaySfxInGame(EnumDefinition.SFX_TYPE sfxType)
    {
        playerSfxInGame.clip = sfxList[(int)sfxType];
        playerSfxInGame.Play();
    }

    public void PlaySfxUI(EnumDefinition.SFX_TYPE sfxType)
    {
        playerSfxUiPlayer.clip = sfxList[(int)sfxType];
        playerSfxUiPlayer.Play();
    }
}
