using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
   
    }

    public IEnumerator InitPlayerData()
    {
        // set save & load path
        path = path = Path.Combine(Application.dataPath, fileName);

        // set game start time
        SetGamePlayStartDateTime();

        // load data
        yield return StartCoroutine(LoadPlayerData());

    }

   

    void SetGamePlayStartDateTime()
    {
        startDataTime = System.DateTime.Now;
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();

        
    }


    

    void Update()
    {

    }



    IEnumerator LoadPlayerData()
    {
        // first connect
        if (GetFirstConnectValue())
        {
            // 첫 접속은 0번 데이터로 셋팅.
            saveData = new SaveData();
            saveData.isFirstConnect = true;
            saveData.stageIdx = 0;
            saveData.upgradeLevelIdx = 0;
            saveData.gold = 0;
            saveData.offlineTime = "0";
            saveData.playingTime = "0";

            // set insect save data
            saveData.beeSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.bee);
            saveData.beetleSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.beetle);
            saveData.mentisSaveData = GetFirstConnectInsectData(EnumDefinition.InsectType.mentis);
        }
        else
        {
            // json data load

        }

        yield return null;
    }

    bool GetFirstConnectValue()
    {
        var isFirstConnect = !PlayerPrefs.HasKey(isFirstConnectKey);
        return isFirstConnect;
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
        saveData.offlineTime = GetOfflineTime();
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
        return "2";
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
    public int gold;
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