using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public SaveData saveData;
    string fileName = "saveData.txt";
    string path;
    string isFirstConnectKey = "isFirstConnect";
    System.DateTime startDataTime;

    // save data key
    DateTime startDateTime;
    TimeSpan offlineTime;

    public string offlineTimeKey = "offlineTime";
    const string playingTimeKey = "playingTime";


    //TODO: 코드 정리

    private void OnApplicationQuit()
    {
        SavePlayingTime();
        SaveOfflineTime();
    }

    void SaveOfflineTime()
    {
        var startOfflineTime = System.DateTime.Now;
        PlayerPrefs.SetString(offlineTimeKey, startOfflineTime.ToString());
    }
    void SavePlayingTime()
    {
        var endTime = System.DateTime.Now;
        var timeSpan = endTime - startDataTime;
        PlayerPrefs.SetString(playingTimeKey, timeSpan.Duration().ToString());
    }

    string LoadOfflineTime()
    {
        var offlineTime = PlayerPrefs.GetString(offlineTimeKey);
        var startOfflineTime = System.DateTime.Parse(offlineTime);
        var endTime = System.DateTime.Now;
        var timeSpan = endTime - startOfflineTime;
        this.offlineTime = timeSpan;
        //Debug.Log("offlineTime : " + this.offlineTime.Hours.ToString());
        return  this.offlineTime.Hours.ToString();
    }

    public string GetOfflineTimeValue()
    {
        return offlineTime.Hours.ToString();
    }

    public IEnumerator InitPlayerData()
    {
        // set save & load path
        path = path = Path.Combine(Application.dataPath, fileName);

        // load data
        yield return StartCoroutine(LoadPlayerData());

        // set game start time
        SetGamePlayStartDateTime();

    }



    void SetGamePlayStartDateTime()
    {
        startDataTime = System.DateTime.Now;
    }


    IEnumerator LoadPlayerData()
    {
        startDataTime = System.DateTime.Now;

        // first connect
        if (!GetFirstConnectValue())
        {
            // 첫 접속은 0번 데이터로 셋팅.
            saveData = new SaveData();
            saveData.isFirstConnect = true;
            saveData.stageIdx = 0;
            saveData.upgradeLevelIdx = 0;
            saveData.gold = 0;
            saveData.offlineTime = "0";
            saveData.playingTime = "0";

            // 최초 던전 키 2개씩 지급
            saveData.dungeonKeyGold = 2;
            saveData.dungeonKeyBone = 2;
            saveData.dungeonKeyDice = 2;
            saveData.dungeonKeyCoal = 2;
            saveData.dungeonKeyADGold = 1;
            saveData.dungeonKeyADBone = 1;
            saveData.dungeonKeyADDice = 1;
            saveData.dungeonKeyADCoal = 1;

            saveData.dungeonLvGold = 0;
            saveData.dungeonLvBone = 0;
            saveData.dungeonLvDice = 0;
            saveData.dungeonLvCoal = 0;

            // set insect save data
            saveData.beeSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.bee);
            saveData.beetleSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.beetle);
            saveData.mentisSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.mentis);

            PlayerPrefs.SetInt(isFirstConnectKey, 1);
        }
        else
        {
            var saveDataTotal = GlobalData.instance.saveDataManager.saveDataTotal;
            // json data load
            // todo: 코드 리펙토링 및 정리
            saveData = new SaveData();
            saveData.isFirstConnect = false;
            saveData.stageIdx = saveDataTotal.saveDataStage.stageLevel;
            saveData.gold = saveDataTotal.saveDataGoods.gold;
            saveData.gem = saveDataTotal.saveDataGoods.gem;
            saveData.bone = saveDataTotal.saveDataGoods.bone;
            saveData.dice = saveDataTotal.saveDataGoods.dice;
            saveData.coal = saveDataTotal.saveDataGoods.coal;
            saveData.clearTicker = saveDataTotal.saveDataGoods.clearTicket;
            saveData.unionTicket = saveDataTotal.saveDataGoods.unionTicket;
            saveData.dnaTicket = saveDataTotal.saveDataGoods.dnaTicket;
            saveData.dungeonKeyGold = saveDataTotal.saveDataGoods.dungeonKeyGold;
            saveData.dungeonKeyBone = saveDataTotal.saveDataGoods.dungeonKeyBone;
            saveData.dungeonKeyDice = saveDataTotal.saveDataGoods.dungeonKeyDice;
            saveData.dungeonKeyCoal = saveDataTotal.saveDataGoods.dungeonKeyCoal;
            saveData.dungeonKeyADGold = saveDataTotal.saveDataGoods.dungeonKeyADGold;
            saveData.dungeonKeyADBone = saveDataTotal.saveDataGoods.dungeonKeyADBone;
            saveData.dungeonKeyADDice = saveDataTotal.saveDataGoods.dungeonKeyADDice;
            saveData.dungeonKeyADCoal = saveDataTotal.saveDataGoods.dungeonKeyADCoal;

            saveData.dungeonLvGold = saveDataTotal.saveDataDungeonLevel.dungeonLvGold;
            saveData.dungeonLvBone = saveDataTotal.saveDataDungeonLevel.dungeonLvBone;
            saveData.dungeonLvDice = saveDataTotal.saveDataDungeonLevel.dungeonLvDice;
            saveData.dungeonLvCoal = saveDataTotal.saveDataDungeonLevel.dungeonLvCoal;

            saveData.evolutionLevel = saveDataTotal.saveDataEvolution.level_evolution;

            // 시간 데이터
            if (PlayerPrefs.HasKey(playingTimeKey))
            {
                saveData.playingTime = PlayerPrefs.GetString(playingTimeKey);
//                Debug.Log("playingTimeKey : " + saveData.playingTime);
            }
            if (PlayerPrefs.HasKey(offlineTimeKey))
            {
                saveData.offlineTime = LoadOfflineTime();
            }

            // todo: 저장 및 로드 로직 추가
            saveData.beeSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.bee);
            saveData.beetleSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.beetle);
            saveData.mentisSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.mentis);
        }

        yield return null;
    }

    bool GetFirstConnectValue()
    {
        return PlayerPrefs.HasKey(isFirstConnectKey);
    }


    // save data
    void SavePlayerData()
    {
        // set firstConnect
        //  PlayerPrefs.SetInt(isFirstConnectKey, 1);

        // Sample Code
        // TODO : 각 데이터에서 값 로드 하여 저장
        saveData = new SaveData();
        saveData.isFirstConnect = false;
        saveData.stageIdx = 0;
        saveData.beeSaveData = GetInsectSaveData();
        saveData.beetleSaveData = GetInsectSaveData();
        saveData.mentisSaveData = GetInsectSaveData();
        saveData.upgradeLevelIdx = 0;
        saveData.gold = 123;
        saveData.playingTime = GetPlayingTime();

        var json = JsonUtility.ToJson(saveData);

        //Debug.Log(json);

        // save file
        //File.CreateText(path);  
        File.WriteAllText(path, json, System.Text.Encoding.Default);
    }

    //TODO : 계산식 적용
    string GetOfflineTime()
    {
        return "1";
    }

    //TODO : 계산식 적용
    string GetPlayingTime()
    {
        var endTime = System.DateTime.Now;
        var timeSpan = endTime - startDataTime;
        return timeSpan.Duration().ToString();
    }

    InsectSaveData GetInsectSaveData()
    {
        InsectSaveData data = new InsectSaveData();
        data.evolutionIdx = 1;
        data.upgradeLevel = 1;
        data.evolutionTechTree = "1,2,3";
        data.evolutionLastData = new EvolutionData();
        return data;
    }

    InsectSaveData GetFirstConnectInsectData(EnumDefinition.InsectType insectType)
    {
        InsectSaveData data = new InsectSaveData();
        data.evolutionIdx = 0;
        data.upgradeLevel = 0;
        data.evolutionTechTree = "";
        var evolData = GlobalData.instance.dataManager.GetEvolutionDataById(insectType, 0);
        data.evolutionLastData = evolData.CopyInstance();
        return data;
    }

    // load data
    void LoadData()
    {
        var existFile = File.Exists(path);
        if (existFile)
        {
            var json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
    }
}


[System.Serializable]
public class SaveData
{
    public bool isFirstConnect;
    public int stageIdx;
    public InsectSaveData beeSaveData;
    public InsectSaveData beetleSaveData;
    public InsectSaveData mentisSaveData;
    public int upgradeLevelIdx;
    public int evolutionLevel;
    public float gold;
    public float bone;
    public float gem;
    public float dice;
    public float coal;
    public float clearTicker;
    public float unionTicket;
    public float dnaTicket;


    public float dungeonKeyGold;
    public float dungeonKeyGem;
    public float dungeonKeyDice;
    public float dungeonKeyCoal;
    public float dungeonKeyBone;

    public float dungeonKeyADGold;
    public float dungeonKeyADDice;
    public float dungeonKeyADCoal;
    public float dungeonKeyADBone;


    public int dungeonLvGold;
    public int dungeonLvBone;
    public int dungeonLvDice;
    public int dungeonLvCoal;





    // public int dungeonLvGold;
    // public int dungeonLvGem;
    // public int dungeonLvDice;
    // public int dungeonLvCoal;

    public string offlineTime;
    public string playingTime;


}

[System.Serializable]
public class InsectSaveData
{
    public int evolutionIdx;
    public int upgradeLevel;
    public string evolutionTechTree;
    public EvolutionData evolutionLastData;
}

/*
[System.Serializable]
public class Player
{
    public int stageIdx;
    public int upgradeLevelIdx;
    public int gold;
    public DateTime playTime;
    /// <summary> 등장한 몬스터 </summary>
    public MonsterBase currentMonster;

}
*/