using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    public StageData stageData;
    public SpriteRenderer rdBg;
    public List<Sprite> bgImages = new List<Sprite>();
    public BackgroundAnimController bgAnimController;

    public BackgroundSpriteFileData bgSpriteFileData;

    int dbSaveCount = 0;

    const string dbStageTableName = "stageInfo";
    void Start()
    {

    }

    public void PlayAnimBgScroll(float distance)
    {
        StartCoroutine(bgAnimController.ScrollAnim_BG(distance));
    }

    public IEnumerator Init(int stageId)
    {
        yield return null;
        SetStageData(stageId);
        SetBgImage();
    }

    public IEnumerator SetStageById(int stageIdx)
    {
        // 일일 퀘스트 완료 : 스테이지 클리어
        EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.progressStage);

        // set data
        SetStageData(stageIdx, out bool isBgImgChange);

        // 배경 변경  
        if (isBgImgChange)
        {
            var nextBgImg = GetCurrentBgImg();
            yield return StartCoroutine(bgAnimController.TransitinBG(nextBgImg));
        }
        else
        {
            yield return new WaitForSeconds(1f);

        }


        // dbSaveCount 0  초기에 한번 저장하고 ,  10을 넘을때 마다 스테이지 정보 서버 저장 한 뒤 dbSaveCount 초기화

        if (dbSaveCount >= 6 || dbSaveCount == 0)
        {
            dbSaveCount = 1;
            GlobalData.instance.backEndDataManager.SaveUserStageInfoData();
        }
        dbSaveCount++;

        yield return null;
    }

    void SetStageData(int stageId)
    {
        var data = GlobalData.instance.dataManager.GetStageDataById(stageId);
        stageData = data.CopyInstance();

        // Set Stage Name
        var stageName = stageData.stageName;
        GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);
        GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.normal, stageName);

    }

    void SetStageData(int stageId, out bool isBgImgChange)
    {
        var data = GlobalData.instance.dataManager.GetStageDataById(stageId);

        // 배경 변경 여부 판단
        isBgImgChange = stageData.bgId != data.bgId;

        stageData = data.CopyInstance();

        // Set Stage Name
        var stageName = stageData.stageName;
        GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);
        GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.normal, stageName);
    }

    public void SetBgImage()
    {
        bgAnimController.ResetBg();
        bgAnimController.SetBgTex_Back(GetCurrentBgImg().texture);
    }

    public void SetDungeonBgImage(int bgId)
    {
        bgAnimController.SetBgTex_Back(GetBgImgById(bgId).texture);
    }


    Sprite GetCurrentBgImg()
    {
        var idx = stageData.bgId;
        return (GetBgImgById(idx));
    }


    Sprite GetBgImgById(int id)
    {
        return bgSpriteFileData.background[id];
    }

    public int GetStageId()
    {
        return stageData.stageId;
    }
}
