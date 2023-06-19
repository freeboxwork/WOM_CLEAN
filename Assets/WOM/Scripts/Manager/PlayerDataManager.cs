using System.Collections;
using UnityEngine;
using System.IO;

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
            // todo: 코드 리펙토림 및 정리
            saveData = new SaveData();
            saveData.isFirstConnect = false;
            saveData.stageIdx = saveDataTotal.saveDataStage.stageLevel;
            saveData.gold = saveDataTotal.saveDataGoods.gold;
            saveData.bone = saveDataTotal.saveDataGoods.bone;
            saveData.dice = saveDataTotal.saveDataGoods.dice;
            saveData.coal = saveDataTotal.saveDataGoods.coal;
            saveData.clearTicker = saveDataTotal.saveDataGoods.clearTicket;
            saveData.unionTicket = saveDataTotal.saveDataGoods.unionTicket;
            saveData.dnaTicket = saveDataTotal.saveDataGoods.dnaTicket;

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
    public int bone;
    public int gem;
    public int dice;
    public int coal;
    public int clearTicker;
    public int unionTicket;
    public int dnaTicket;

    public int dungeonLvGold;
    public int dungeonLvGem;
    public int dungeonLvDice;
    public int dungeonLvCoal;

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