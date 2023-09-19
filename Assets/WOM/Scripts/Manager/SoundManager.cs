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
    public AudioSource playerSfxInGame;   // ���� ���ݰ� ���� ȿ����
    public AudioSource playerSfxUiPlayer; // UI ���� ���Ǵ� ȿ����

    public List<AudioClip> bgmList = new List<AudioClip>();
    public List<AudioClip> sfxList = new List<AudioClip>();

    float bgmVolume = 1f;
    float sfxVolume = 1f;

    public bool bgmOn;
    public bool sfxOn;

    const string bgmOnOffKey = "bgmOnOff";
    const string sfxOnOffKey = "sfxOnOff";

    int poolSize = 20; // ������Ʈ Ǯ ũ��
    public Transform poolParent;

    public GameObject spawnPrefab; // ���带 ����� ������
    public GameObject upgradePrefab; // ���带 ����� ������
    public GameObject hitPrefab; // ��Ʈ ���带 ����� ������

    private List<GameObject> spawnPool = new List<GameObject>();
    private List<GameObject> upgradePool = new List<GameObject>();
    private List<GameObject> hitPool = new List<GameObject>();
    

    public IEnumerator Init()
    {
        bgmOn = IsBgmOn();
        sfxOn = IsSfxOn();
        yield return new WaitForEndOfFrame();
        MakeAudioPool();

        bgmVolume = bgmOn ? 1f : 0f;    
        sfxVolume = sfxOn ? 1f : 0f;

        // set bgm clip
        SetBgmClip(EnumDefinition.BGM_TYPE.BGM_Main);

        if (bgmOn)
            PlayBgm();


        // Set Setting Popup UI
        yield return new WaitForEndOfFrame();
        GlobalData.instance.settingPopupController.SetUI();
    }


    void MakeAudioPool()
    {

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ob = Instantiate(spawnPrefab);
            //���� ����� ����� ��ü�� ��ġ �θ�� ����
            ob.transform.SetParent(poolParent);
            //��Ȱ��ȭ �ʱ�ȭ
            ob.SetActive(false);
            //Ǯ���� ����� ����Ʈ�� ����
            spawnPool.Add(ob);
        }

        for (int i = 0; i < poolSize; i++)
        {

            GameObject ob = Instantiate(upgradePrefab);
            //���� ����� ����� ��ü�� ��ġ �θ�� ����
            ob.transform.SetParent(poolParent);
            //��Ȱ��ȭ �ʱ�ȭ
            ob.SetActive(false);
            //Ǯ���� ����� ����Ʈ�� ����
            upgradePool.Add(ob);
        }
        for (int i = 0; i < poolSize; i++)
        {

            GameObject ob = Instantiate(hitPrefab);
            //���� ����� ����� ��ü�� ��ġ �θ�� ����
            ob.transform.SetParent(poolParent);
            //��Ȱ��ȭ �ʱ�ȭ
            ob.SetActive(false);
            //Ǯ���� ����� ����Ʈ�� ����
            hitPool.Add(ob);
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

    GameObject GetHitAudioTypePool()
    {
        foreach (GameObject sound in hitPool)
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
            source.volume = sfxVolume * 0.5f;
            source.Play();
        }

    }

    // ���� �� ���� ��� �Լ�
    public void PlayAttackSound()
    {
        GameObject sound = GetSpawnAudioTypePool();
        if (sound != null)
        {
            sound.SetActive(true);
            var source = sound.GetComponent<AudioSource>();
            source.volume = sfxVolume * 0.3f;
            source.Play();
        }
    }
    //���� ��Ʈ ����
    public void PlayMonsterHitSound()
    {
        GameObject sound = GetHitAudioTypePool();
        if (sound != null)
        {
            sound.SetActive(true);
            var source = sound.GetComponent<AudioSource>();
            source.volume = sfxVolume;
            source.Play();
        }
    }

    public void BGM_OnOff()
    {
        bgmOn = !bgmOn;

        var volume = bgmOn ? 1 : 0;
        bgmVolume = volume;
        playerBgm.volume = bgmVolume;
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
        sfxVolume = volume;
        var saveValue = sfxOn ? "on" : "off";
        PlayerPrefs.SetString(sfxOnOffKey, saveValue);
    }

    public void SetBgmClip(EnumDefinition.BGM_TYPE bgmType)
    {
        playerBgm.clip = bgmList[(int)bgmType];
    }

    public void PlayBgm()
    {
        playerBgm.volume = bgmVolume;
        playerBgm.Play();
    }

    public void PlayBGM(EnumDefinition.BGM_TYPE bgmType)
    {
        playerBgm.clip = bgmList[(int)bgmType];
        playerBgm.volume = bgmVolume;

        playerBgm.Play();
    }

    public void PlaySfxInGame(EnumDefinition.SFX_TYPE sfxType)
    {
        playerSfxInGame.clip = sfxList[(int)sfxType];
        playerSfxInGame.volume = sfxVolume;
        playerSfxInGame.Play();
    }

    public void PlaySfxUI(EnumDefinition.SFX_TYPE sfxType)
    {
        playerSfxUiPlayer.clip = sfxList[(int)sfxType];
        playerSfxUiPlayer.volume = sfxVolume;

        playerSfxUiPlayer.Play();
    }
}
