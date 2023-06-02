using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playerBgm;
    public AudioSource playerSfxInGame;   // 몬스터 공격과 같은 효과음
    public AudioSource playerSfxUiPlayer; // UI 에서 사용되는 효과음

    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> sfxList = new List<AudioClip>();

    public bool bgmOn = true;
    public bool sfxOn = true;

    void Start()
    {
        PlayBgm(EnumDefinition.BGM_TYPE.BGM_01);
    }

    public void BGM_OnOff()
    {
        bgmOn = !bgmOn;
        if (bgmOn)
            playerBgm.Play();
        else
            playerBgm.Stop();
    }

    public void SFX_OnOff()
    {
        sfxOn = !sfxOn;
        var volume = sfxOn ? 1 : 0;
        playerSfxInGame.volume = volume;
        playerSfxUiPlayer.volume = volume;
    }

    public void PlayBgm(EnumDefinition.BGM_TYPE bgmType)
    {
        playerBgm.clip = bgmList[(int)bgmType];
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
