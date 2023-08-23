using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public enum AudioPoolTYPE
{
    Upgrade,
    Spawn
}

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

    int poolSize = 20; // 오브젝트 풀 크기
    public Transform poolParent;

    public GameObject spawnPrefab; // 사운드를 재생할 프리팹
    public GameObject upgradePrefab; // 사운드를 재생할 프리팹

    private List<GameObject> spawnPool = new List<GameObject>();
    private List<GameObject> upgradePool = new List<GameObject>();
    

    public IEnumerator Init()
    {
        bgmOn = IsBgmOn();
        sfxOn = IsSfxOn();
        yield return new WaitForEndOfFrame();
        MakeAudioPool();


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


    void MakeAudioPool()
    {

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ob = Instantiate(spawnPrefab);
            //실제 사용할 오디오 객체의 위치 부모로 세팅
            ob.transform.SetParent(poolParent);
            //비활성화 초기화
            ob.SetActive(false);
            //풀링에 사용할 리스트에 삽입
            spawnPool.Add(ob);
        }

        for (int i = 0; i < poolSize; i++)
        {

            GameObject ob = Instantiate(upgradePrefab);
            //실제 사용할 오디오 객체의 위치 부모로 세팅
            ob.transform.SetParent(poolParent);
            //비활성화 초기화
            ob.SetActive(false);
            //풀링에 사용할 리스트에 삽입
            upgradePool.Add(ob);
        }

    }


    GameObject GetUpgradeAudioTypePool()
    {
        foreach (GameObject sound in upgradePool)
        {
            if (!sound.activeInHierarchy)
            {
                return sound;
            }
        }
        return null;

    }
    GameObject GetSpawnAudioTypePool()
    {
        foreach (GameObject sound in spawnPool)
        {
            if (!sound.activeInHierarchy)
            {
                return sound;
            }
        }
        return null;

    }

    public void PlayUpgradeSound()
    {
        GameObject sound = GetUpgradeAudioTypePool();
        if (sound != null)
        {
            sound.SetActive(true);
            var source = sound.GetComponent<AudioSource>();
            source.volume = sfxOn ? 0.5f : 0;
            source.Play();
        }

    }

    // 공격 시 사운드 재생 함수
    public void PlayAttackSound()
    {
        GameObject sound = GetSpawnAudioTypePool();
        if (sound != null)
        {
            sound.SetActive(true);
            var source = sound.GetComponent<AudioSource>();
            source.volume = sfxOn ? 0.3f : 0;
            source.Play();
        }

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

    public void PlayBGM(EnumDefinition.BGM_TYPE bgmType)
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
